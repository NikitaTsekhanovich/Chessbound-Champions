using System;
using System.Collections.Generic;
using GameControllers.GameBoard.Figures;
using GameControllers.GameBoard.Figures.Models;
using GameControllers.Handlers;

namespace GameControllers.Players
{
    public class FigureMoveHandler
    {
        private readonly PlayersFigures _firstPlayerFigures;
        private readonly PlayersFigures _secondPlayerFigures;
        private readonly PlayerController _firstPlayer;
        private readonly PlayerController _secondPlayer;
        
        public FigureMoveHandler(
            PlayersFigures firstPlayerFigures, 
            PlayersFigures secondPlayerFigures,
            PlayerController firstPlayer,
            PlayerController secondPlayer)
        {
            _firstPlayerFigures = firstPlayerFigures;
            _secondPlayerFigures = secondPlayerFigures;
            _firstPlayer = firstPlayer;
            _secondPlayer = secondPlayer;
        }
        
        public void CalculateMovePosition(FigureData figure)
        {
            var movePoints = GetMovePositions(figure);
            
            if (figure.PlayerType == PlayersTypes.FirstPlayer)
                _firstPlayer.ChooseStrategy(figure, movePoints.Item1, movePoints.Item2);
            else 
                _secondPlayer.ChooseStrategy(figure, movePoints.Item1, movePoints.Item2);
        }

        public Tuple<List<Points>, List<Points>> GetMovePositions(FigureData figure)
        {
            var currentMovePoints = new List<Points>();
            var currentAttackPoints = new List<Points>();

            var indexAttack = 0;
            figure.TryGetComponent<Pawn>(out var pawn);
            
            foreach (var point in figure.MovePoints)
            {
                var positionStepX = point.X + figure.PositionOnIndex.X;
                var positionStepY = point.Y + figure.PositionOnIndex.Y;

                if (pawn != null && pawn.FigureType == FiguresTypes.Pawn)
                {
                    var positionAttackX = pawn.AttackPoints[indexAttack].X + figure.PositionOnIndex.X;
                    var positionAttackY = pawn.AttackPoints[indexAttack].Y + figure.PositionOnIndex.Y;

                    var positionStepOnBoard = GetCheckRange(positionStepX, positionStepY);
                    CanAddMovePoint(figure, positionStepX, positionStepY, currentMovePoints, null, positionStepOnBoard);

                    positionStepOnBoard = GetCheckRange(positionAttackX, positionAttackY);
                    CanAddMovePoint(figure, positionAttackX, positionAttackY, null, currentAttackPoints, positionStepOnBoard);

                    indexAttack++;
                }
                else if (figure.FigureType == FiguresTypes.King || figure.FigureType == FiguresTypes.Knight)
                {
                    var positionStepOnBoard = GetCheckRange(positionStepX, positionStepY);
                    CanAddMovePoint(figure, positionStepX, positionStepY, currentMovePoints, currentAttackPoints, positionStepOnBoard);
                }
                else 
                {
                    var canMoveNext = true;
                    var offset = 1;
                    while (canMoveNext)
                    {
                        positionStepX = point.X * offset + figure.PositionOnIndex.X;
                        positionStepY = point.Y * offset + figure.PositionOnIndex.Y;
                        var positionStepOnBoard = GetCheckRange(positionStepX, positionStepY);

                        canMoveNext = CanAddMovePoint(figure, positionStepX, positionStepY, currentMovePoints, currentAttackPoints, positionStepOnBoard);
                        
                        offset++;
                    }
                }
            }

            return Tuple.Create(currentMovePoints, currentAttackPoints);
        }

        private bool CanAddMovePoint(
            FigureData figure,
            int positionX, 
            int positionY,
            List<Points> currentMovePoints,
            List<Points> currentAttackPoints,
            string positionStepOnBoard)
        {
            if (positionStepOnBoard == null)
                return false;

            if (currentMovePoints != null &&
                !_secondPlayerFigures.Figures.ContainsKey(positionStepOnBoard) &&
                !_firstPlayerFigures.Figures.ContainsKey(positionStepOnBoard))
            {
                currentMovePoints.Add(new Points(positionX, positionY));
                return true;
            } 
            if (currentAttackPoints != null &&
                  ((_secondPlayerFigures.Figures.ContainsKey(positionStepOnBoard) && 
                    figure.PlayerType == PlayersTypes.FirstPlayer &&
                    _secondPlayerFigures.Figures[positionStepOnBoard].FigureType != FiguresTypes.Block) ||
                   (_firstPlayerFigures.Figures.ContainsKey(positionStepOnBoard) && 
                    figure.PlayerType == PlayersTypes.SecondPlayer &&
                    _firstPlayerFigures.Figures[positionStepOnBoard].FigureType != FiguresTypes.Block)))
            {
                currentAttackPoints.Add(new Points(positionX, positionY));
                return false;
            }

            return false;
        }

        public string GetCheckRange(
            int positionX, 
            int positionY)
        {
            if (positionX >= 0 && positionX < 8 &&
                positionY >= 0 && positionY < 8)
            {
                var positionStepOnBoard = FigurePositionHandler.GetPositionOnBoard(positionX, positionY);;
                return positionStepOnBoard;
            }
            return null;
        }
    }
}
