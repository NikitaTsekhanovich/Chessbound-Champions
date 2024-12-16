using System;
using GameControllers.GameBoard.CellTypes;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLaunch.Properties;
using GameControllers.Players;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameControllers.GameLogic.Abilities
{
    public class AbilitySpawn : IGameController
    {
        private readonly PlayersFigures _firstPlayerFigures;
        private readonly PlayersFigures _secondPlayerFigures;
        private readonly PlayerController _firstPlayer;
        private readonly PlayerController _secondPlayer;
        
        public static Action<Points, FiguresTypes, PlayersTypes> OnSpawnDropFigure;
        public static Func<FigureData> OnGetCurrentFigure;

        public AbilitySpawn(
            PlayersFigures firstPlayerFigures, 
            PlayersFigures secondPlayerFigures,
            PlayerController firstPlayer,
            PlayerController secondPlayer)
        {
            SubscribeToActions();
            
            _firstPlayerFigures = firstPlayerFigures;
            _secondPlayerFigures = secondPlayerFigures;
            _firstPlayer = firstPlayer;
            _secondPlayer = secondPlayer;
        }

        private void SubscribeToActions()
        {
            Cell.OnSpawnFigure += CheckPlayerType;
        }

        private void CheckPlayerType(
            PointerEventData eventData, 
            Points positionOnIndex, 
            string positionOnBoard,
            Transform transformCell)
        {
            if (OnGetCurrentFigure.Invoke().PlayerType == PlayersTypes.FirstPlayer)
                SpawnFigure(eventData, positionOnIndex, positionOnBoard, transformCell, _firstPlayer);
            else
                SpawnFigure(eventData, positionOnIndex, positionOnBoard, transformCell, _secondPlayer);
        }

        private void SpawnFigure(
            PointerEventData eventData, 
            Points positionOnIndex, 
            string positionOnBoard,
            Transform transformCell,
            PlayerController player)
        {
            if (_firstPlayerFigures.Figures.ContainsKey(positionOnBoard) ||
                _secondPlayerFigures.Figures.ContainsKey(positionOnBoard))
                return;
            
            var otherItemAbility = eventData.pointerDrag.GetComponent<FigureAbility>();
            
            if (player.GetMana() - otherItemAbility.Cost < 0)
                return;
            
            var otherItemTransform = eventData.pointerDrag.transform;
            var otherItemImage = eventData.pointerDrag.GetComponent<Image>();
            otherItemTransform.SetParent(transformCell);
            otherItemTransform.localPosition = Vector3.zero;
            otherItemImage.raycastTarget = false;
            
            otherItemAbility.ImproveSound.Play();
            
            OnSpawnDropFigure.Invoke(
                positionOnIndex, 
                otherItemAbility.FigureType, 
                otherItemAbility.PlayerType);
            otherItemAbility.DestroyAbility(player);
        }

        public void KillController()
        {
            Cell.OnSpawnFigure -= CheckPlayerType;
        }
    }
}
