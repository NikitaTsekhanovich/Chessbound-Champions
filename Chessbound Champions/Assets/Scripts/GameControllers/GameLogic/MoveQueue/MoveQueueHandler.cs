using System;
using System.Collections.Generic;
using System.Linq;
using GameControllers.GameBoard;
using GameControllers.GameBoard.CellTypes;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLaunch.ControllersData;
using GameControllers.GameLaunch.Properties;
using GameControllers.GameLogic.Abilities;
using GameControllers.Players;
using GameControllers.Players.AI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace GameControllers.GameLogic.MoveQueue
{
    public class MoveQueueHandler : IGameController
    {
        private readonly Transform _queueUI;
        private readonly FigureIcon _figureIcon;
        private readonly Image _firstFigureIcon;
        private readonly TMP_Text _firstFigurePosition;
        private readonly PlayersFigures _firstPlayerFigures;
        private readonly PlayersFigures _secondPlayerFigures;
        private readonly FigureMoveHandler _figureMoveHandler;
        private readonly PlayerController _firstPlayer;
        private readonly PlayerController _secondPlayer;

        private Queue<FigureData> _queue = new ();
        private FigureData _currentFigure;
        private bool _isFirstFigure = true;
        private bool _gameIsEnd;

        public static Action<PlayersTypes> OnChangeAbilitiesBlock;

        public MoveQueueHandler(
            MoveQueueHandlerData moveQueueHandlerData,
            PlayersFigures firstPlayerFigures,
            PlayersFigures secondPlayerFigures,
            FigureMoveHandler figureMoveHandler,
            PlayerController firstPlayer,
            PlayerController secondPlayer)
        {
            SubscribeToActions();
            
            _queueUI = moveQueueHandlerData.QueueUI;
            _figureIcon = moveQueueHandlerData.FigureIcon;
            _firstFigureIcon = moveQueueHandlerData.FirstFigureIcon;
            _firstFigurePosition = moveQueueHandlerData.FirstFigurePosition;
            _firstPlayerFigures = firstPlayerFigures;
            _secondPlayerFigures = secondPlayerFigures;
            _figureMoveHandler = figureMoveHandler;
            _firstPlayer = firstPlayer;
            _secondPlayer = secondPlayer;
        }

        private void SubscribeToActions()
        {
            FigureData.OnFigureMove += MoveQueue;
            AIController.OnFigureMove += MoveQueue;
            AbilityImprove.OnImproveFigure += MoveQueue;
            PlayersFigures.OnChangeFigures += DeleteFigureOnQueue;
            FigureSpawner.OnChangeQueue += AddFigureOnQueue;
            FigureSpawner.OnGetCurrentFigure += GetCurrentFigure;
            GameStateController.OnChangeGameState += ChangeGameState;
            Step.GetCurrentFigure += GetCurrentFigure;
            AbilitySpawn.OnGetCurrentFigure += GetCurrentFigure;
        }

        private FigureData GetCurrentFigure() => _currentFigure;

        private void MoveQueue()
        {
            if (!_gameIsEnd)
            {
                UpdateQueue();
                CreateQueueUI();
            }
        }

        private void UpdateQueue()
        {
            if (_currentFigure.FigureType == FiguresTypes.Block)
            {
                if (_currentFigure.PlayerType == PlayersTypes.FirstPlayer)
                    _firstPlayerFigures.DeleteFigure(_currentFigure.PositionOnBoard);
                else
                    _secondPlayerFigures.DeleteFigure(_currentFigure.PositionOnBoard);
            }
            else
                _queue.Enqueue(_currentFigure);
        }

        private void UpdateCurrentPlayerMana()
        {
            if (_currentFigure.PlayerType == PlayersTypes.FirstPlayer)
                _firstPlayer.UpdateMana(ManaController.ManaIncreasePerRound);
            else
                _secondPlayer.UpdateMana(ManaController.ManaIncreasePerRound);
        }

        private void UpdateCurrentAbilityBlock()
        {
            OnChangeAbilitiesBlock.Invoke(_currentFigure.PlayerType);
        }

        public void CreateQueueUI()
        {
            ClearQueueUI();
            _isFirstFigure = true;

            foreach (var figure in GetQueue())
                InstantiateIcon(figure);

            _currentFigure = _queue.Dequeue();
            UpdateCurrentPlayerMana();
            UpdateCurrentAbilityBlock();

            if (_currentFigure.PlayerType == PlayersTypes.FirstPlayer)
                _figureMoveHandler.CalculateMovePosition(_firstPlayerFigures.Figures[_currentFigure.PositionOnBoard]);
            else 
                _figureMoveHandler.CalculateMovePosition(_secondPlayerFigures.Figures[_currentFigure.PositionOnBoard]);
        }

        private Queue<FigureData> GetQueue()
        {
            if (_queue.Count == 0)
            {
                foreach (var figure in _firstPlayerFigures.Figures.Zip(_secondPlayerFigures.Figures, 
                             (human, ai) => (human, ai)))
                {
                    _queue.Enqueue(figure.human.Value);
                    _queue.Enqueue(figure.ai.Value);
                }
            }

            return _queue;
        }

        private void DeleteFigureOnQueue(string position)
        {
            var tempQueue = new List<FigureData>(_queue);
            tempQueue = tempQueue.Where(val => val.PositionOnBoard != position).ToList();
            _queue = new Queue<FigureData>(tempQueue);
        }

        private void AddFigureOnQueue(FigureData figureData)
        {
            _queue.Enqueue(figureData);
            InstantiateIcon(figureData);
        }

        private void InstantiateIcon(FigureData figureData)
        {
            if (_isFirstFigure)
            {
                _firstFigureIcon.sprite = figureData.FigureImage;
                _firstFigurePosition.text = figureData.PositionOnBoard;
                _isFirstFigure = false;
            }
            else
            {
                var figureIcon = Object.Instantiate(_figureIcon, _queueUI, true);
                figureIcon.SetData(figureData.FigureImage, figureData.PositionOnBoard);
                figureIcon.transform.localScale = Vector3.one;
                figureIcon.transform.localPosition = new Vector3(
                    figureIcon.transform.localPosition.x,
                    figureIcon.transform.localPosition.y,
                    0
                );
            }
        }

        private void ClearQueueUI()
        {
            while (_queueUI.transform.childCount > 0) 
                Object.DestroyImmediate(_queueUI.transform.GetChild(0).gameObject);
        }

        private void ChangeGameState()
        {
            _gameIsEnd = true;
        }

        public void KillController()
        {
            FigureData.OnFigureMove -= MoveQueue;
            AIController.OnFigureMove -= MoveQueue;
            AbilityImprove.OnImproveFigure -= MoveQueue;
            PlayersFigures.OnChangeFigures -= DeleteFigureOnQueue;
            FigureSpawner.OnChangeQueue -= AddFigureOnQueue;
            FigureSpawner.OnGetCurrentFigure -= GetCurrentFigure;
            GameStateController.OnChangeGameState -= ChangeGameState;
            Step.GetCurrentFigure -= GetCurrentFigure;
            AbilitySpawn.OnGetCurrentFigure -= GetCurrentFigure;
        }
    }
}

