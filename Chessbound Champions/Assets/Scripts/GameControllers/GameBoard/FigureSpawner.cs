using System;
using System.Collections;
using System.Collections.Generic;
using GameControllers.GameBoard.CellTypes;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLaunch.ControllersData;
using GameControllers.GameLaunch.Properties;
using GameControllers.GameLogic.Abilities;
using GameControllers.Handlers;
using GameControllers.Players;
using GameControllers.Players.AI;
using GameControllers.Players.Human;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameControllers.GameBoard
{
    public class FigureSpawner : IGameController
    {
        private readonly Transform[] _figuresParents;
        private readonly Step _stepImage;
        private readonly List<Step> _moveSteps;
        private readonly float _cellSizeX;
        private readonly float _cellSizeY;
        private readonly PlayersFigures _firstPlayerFigures;
        private readonly PlayersFigures _secondPlayerFigures;
        private readonly FigureMoveHandler _figureMoveHandler;

        public static Action<FigureData> OnChangeQueue;
        public static Action<FiguresTypes, PlayersTypes> OnDropAbility;
        public static Func<FigureData> OnGetCurrentFigure;
        public static Func<Points, Cell> OnGetCell;

        public FigureSpawner(
            FigureSpawnerData figureSpawnerData,
            FlexibleGridLayout gridLayout,
            PlayersFigures firstPlayerFigures,
            PlayersFigures secondPlayerFigures,
            FigureMoveHandler figureMoveHandler)
        {
            SubscribeToActions();
            
            _figuresParents = figureSpawnerData.FiguresParents;
            _stepImage = figureSpawnerData.StepImage;
            _cellSizeX = gridLayout.cellSize.x;
            _cellSizeY = gridLayout.cellSize.y;
            _firstPlayerFigures = firstPlayerFigures;
            _secondPlayerFigures = secondPlayerFigures;
            _moveSteps = new List<Step>();
            _figureMoveHandler = figureMoveHandler;
        }
        
        private void SubscribeToActions()
        {
            HumanController.OnShowSteps += ShowPathCurrentFigure;
            AbilitySpawn.OnSpawnDropFigure += SpawnDropFigure;
            AIController.OnSpawnDropFigure += SpawnDropFigure;
            FigureData.OnChangePosition += ChangeParentFigure;
        }
        
        public FigureData GetSpawnFigure(
            Points spawnPoints, 
            FiguresTypes figureType, 
            PlayersTypes playerType)
        {
            var cellTransform = OnGetCell.Invoke(spawnPoints).transform;

            var newFigure = Object.Instantiate(
                FigureStorage.Figures[playerType][figureType], 
                cellTransform);

            var positionOnBoard = FigurePositionHandler.GetPositionOnBoard(spawnPoints.X, spawnPoints.Y);
            var positionOnIndex = new Points (
                (int)CellDataTypes.HorizontalTypes.GetValue(spawnPoints.X),
                (int)CellDataTypes.VerticalTypes.GetValue(spawnPoints.Y));

            Coroutines.StartRoutine(WaitPositionCell(cellTransform, newFigure, positionOnBoard));
            newFigure.transform.localScale = Vector3.one;

            newFigure.SetFigurePosition(
                positionOnBoard,
                positionOnIndex);

            newFigure.SetSize(
                _cellSizeX,
                _cellSizeY);

            if (playerType == PlayersTypes.FirstPlayer)
                _firstPlayerFigures.AddFigure(positionOnBoard, newFigure);
            else
                _secondPlayerFigures.AddFigure(positionOnBoard, newFigure);

            return newFigure;
        }
        
        private void SpawnDropFigure(
            Points spawnPoints, 
            FiguresTypes figureType, 
            PlayersTypes playerType)
        {
            var newFigure = GetSpawnFigure(spawnPoints, figureType, playerType);
            OnDropAbility.Invoke(figureType, playerType);
            OnChangeQueue.Invoke(newFigure);
            _figureMoveHandler.CalculateMovePosition(OnGetCurrentFigure());
        }
        
        private IEnumerator WaitPositionCell(Transform cellTransform, FigureData newFigure, string positionOnBoard)
        {
            yield return new WaitForEndOfFrame();
            newFigure.transform.SetParent(_figuresParents[^1]);
            newFigure.transform.localPosition = new Vector3(cellTransform.localPosition.x, cellTransform.localPosition.y + 26f, 0);
            newFigure.transform.SetParent(_figuresParents[positionOnBoard[1] - '0' - 1]);
        }
        
        private void ShowPathCurrentFigure(FigureData figure, List<Points> movePoints, List<Points> attackPoints)
        {
            ClearMoveSteps();
            InstantiateSteps(figure, movePoints, false);
            InstantiateSteps(figure, attackPoints, true);
        }
        
        private void ClearMoveSteps()
        {
            foreach (var moveStep in _moveSteps)
                Object.Destroy(moveStep.gameObject);
            
            _moveSteps.Clear();
        }
        
        private void InstantiateSteps(FigureData figure, List<Points> points, bool isAttackPoints)
        {
            foreach (var point in points)
            {
                var step = Object.Instantiate(
                    _stepImage,
                    OnGetCell.Invoke(point).transform);
                step.transform.SetParent(OnGetCell.Invoke(point).transform);
                step.transform.localScale = Vector3.one;

                step.Init(
                    new Points(point.X, point.Y),
                    figure.PositionOnBoard,
                    _firstPlayerFigures,
                    _secondPlayerFigures);

                if (isAttackPoints)
                    step.OffImage();

                _moveSteps.Add(step);
            }
        }
        
        private void ChangeParentFigure(FigureData newFigure, string positionOnBoard)
        {
            ClearMoveSteps();
            newFigure.transform.SetParent(_figuresParents[positionOnBoard[1] - '0' - 1]);
        }

        public void KillController()
        {
            HumanController.OnShowSteps -= ShowPathCurrentFigure;
            AbilitySpawn.OnSpawnDropFigure -= SpawnDropFigure;
            AIController.OnSpawnDropFigure -= SpawnDropFigure;
            FigureData.OnChangePosition -= ChangeParentFigure;
        }
    }
}
