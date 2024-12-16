using System;
using GameControllers.GameBoard.CellTypes;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLaunch.Properties;
using GameControllers.Players;
using UnityEngine;

namespace GameControllers.GameLogic.Abilities
{
    public class AbilityImprove : IGameController
    {
        private readonly PlayersFigures _firstPlayerFigures;
        private readonly PlayersFigures _secondPlayerFigures;
        private readonly PlayerController _firstPlayer;
        private readonly PlayerController _secondPlayer;
        
        public static Action OnImproveFigure;
        public static Action<int, PlayersTypes> OnRespawnImproverAbility;

        public AbilityImprove(
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
            Cell.OnImproveFigure += CheckPlayerType;
        }

        private void CheckPlayerType(
            Improver otherItemImprover, 
            string positionOnBoard)
        {
            if (otherItemImprover.PlayerType == PlayersTypes.FirstPlayer)
                ImproveFigure(
                    otherItemImprover, 
                    positionOnBoard, 
                    _firstPlayerFigures, 
                    _firstPlayer,
                    otherItemImprover.ImageFirst);
            else
                ImproveFigure(
                    otherItemImprover, 
                    positionOnBoard, 
                    _secondPlayerFigures, 
                    _secondPlayer,
                    otherItemImprover.ImageSecond);
        }
        
        private void ImproveFigure(
            Improver otherItemImprover, 
            string positionOnBoard,
            PlayersFigures playerFigures,
            PlayerController player,
            Sprite improveImage)
        {
            if (player.GetMana() - otherItemImprover.Cost < 0)
                return;
            
            if (playerFigures.Figures.ContainsKey(positionOnBoard))
            {
                var figureType = playerFigures.Figures[positionOnBoard].FigureType;
            
                if (figureType != FiguresTypes.King && 
                    figureType != otherItemImprover.FigureType && 
                    figureType != FiguresTypes.Block)
                {
                    playerFigures.Figures[positionOnBoard].ImproveFigure(
                        improveImage,
                        otherItemImprover.FigureType,
                        otherItemImprover.MovePoints);
            
                    otherItemImprover.ImproveSound.Play();

                    OnImproveFigure.Invoke();
                    otherItemImprover.DestroyAbility(player);
                    OnRespawnImproverAbility.Invoke(
                        otherItemImprover.NumberSlot,
                        otherItemImprover.PlayerType);
                }
            }
        }

        public void KillController()
        {
            Cell.OnImproveFigure -= CheckPlayerType;
        }
    }
}
