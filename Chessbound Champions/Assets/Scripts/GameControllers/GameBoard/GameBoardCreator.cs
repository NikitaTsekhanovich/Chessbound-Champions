using GameControllers.GameBoard.CellTypes;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLaunch.ControllersData;
using GameControllers.GameLaunch.Properties;
using GameControllers.GameLogic.MoveQueue;
using GameControllers.Handlers;
using GameControllers.Players;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameControllers.GameBoard
{
    public class GameBoardCreator : IGameController
    {
        private readonly Cell[,] _gameBoard;
        
        public const int NumberColumns = 8;
        public const int NumberRows = 8;

        public GameBoardCreator(
            GameBoardData gameBoardData, 
            MoveQueueHandler moveQueueHandler,
            FigureSpawner figureSpawner)
        {
            SubscribeToActions();
            _gameBoard = new Cell[NumberColumns, NumberRows];
            
            PostCells(gameBoardData.PrefabCells, gameBoardData.GameBoardParent);
            ChooseSpawnPositionsFigures(gameBoardData.PositionsFigures, moveQueueHandler, figureSpawner);
        }
        
        private void SubscribeToActions()
        {
            FigureSpawner.OnGetCell += GetCell;
        }
        
        private Cell GetCell(Points points) => _gameBoard[points.X, points.Y];

        private void PostCells(Cell[] cells, FlexibleGridLayout gameBoardParent)
        {
            for (var i = 0; i < NumberColumns; i++)
            {
                for (var j = 0; j < NumberRows; j++)
                {
                    var cell = Object.Instantiate(cells[(j + i) % 2], gameBoardParent.transform, false);
                    
                    cell.transform.localScale = Vector3.one;
                    cell.name = $"{CellDataTypes.HorizontalTypes.GetValue(j)} {CellDataTypes.VerticalTypes.GetValue(i)}";
                    cell.InitPosition(i, j, FigurePositionHandler.GetPositionOnBoard(i, j));
                    
                    _gameBoard[i, j] = cell;
                }
            }
        }

        private void ChooseSpawnPositionsFigures(
            SpawnPositionFiguresData[] positionsFigures,
            MoveQueueHandler moveQueueHandler,
            FigureSpawner figureSpawner)
        {
            var index = Random.Range(0, positionsFigures.Length);
            SpawnFirstFigures(
                positionsFigures[index].PointsPawnPlayer,
                positionsFigures[index].PointKingPlayer,
                PlayersTypes.FirstPlayer,
                figureSpawner);
            SpawnFirstFigures(
                positionsFigures[index].PointsPawnEnemy,
                positionsFigures[index].PointKingEnemy,
                PlayersTypes.SecondPlayer,
                figureSpawner);

            moveQueueHandler.CreateQueueUI();
        }

        private void SpawnFirstFigures(
            Points[] pointsPawns, 
            Points pointKing,
            PlayersTypes playerType,
            FigureSpawner figureSpawner)
        {
            foreach (var points in pointsPawns)
                figureSpawner.GetSpawnFigure(points, FiguresTypes.Pawn, playerType);

            figureSpawner.GetSpawnFigure(pointKing, FiguresTypes.King, playerType);
        }

        public void KillController()
        {
            FigureSpawner.OnGetCell -= GetCell;
        }
    }
}
