using System;
using System.Collections.Generic;
using GameControllers.GameBoard;
using GameControllers.GameBoard.Figures;
using GameControllers.GameBoard.Figures.Models;
using GameControllers.GameLogic.Abilities;
using GameControllers.Handlers;
using StateMachine;

namespace GameControllers.Players.AI
{
    public class AIController : PlayerController
    {
        public static Action<Points, FiguresTypes, PlayersTypes> OnSpawnDropFigure;
        
        private readonly AIMana _aIMana;
        private readonly PlayersFigures _humanFigures;
        private readonly PlayersFigures _ownerFigures;
        private FigureMoveHandler _figureMoveHandler;
        private bool _isAttackOnFigure;
        private BaseStateMachine _baseStateMachine;
        
        public AIController(
            AIMana aIMana,
            PlayersFigures humanFigures,
            PlayersFigures ownerFigures)
            
        {
            _humanFigures = humanFigures;
            _ownerFigures = ownerFigures;
            _aIMana = aIMana;
            _aIMana.Init(StartMana);
        }

        public void InitMoveHandler(FigureMoveHandler figureMoveHandler)
        {
            _figureMoveHandler = figureMoveHandler;
        }

        public override void UpdateMana(int cost)
        {
            _aIMana.ChangeMana(cost);
        }

        public override int GetMana() => _aIMana.GetCurrentMana();

        public override void ChooseStrategy(FigureData figure, List<Points> currentMovePoints, List<Points> currentAttackPoints)
        {
            var randomStrategy = UnityEngine.Random.Range(0, 3);
            _isAttackOnFigure = false;

            if (randomStrategy - 2 == (int)StrategyType.UseAbility)
            {
                if (!IsUseAbilitySpawn())
                    ChooseStateMove(figure, currentMovePoints, currentAttackPoints);
            }
            else if (randomStrategy <= (int)StrategyType.Move)
            {
                ChooseStateMove(figure, currentMovePoints, currentAttackPoints);
            }                
        }

        private bool IsUseAbilitySpawn()
        {
            if (_aIMana.GetCurrentMana() - 30 >= 0)
            {
                var positionSpawn = GetSpawnPositionPawn();
                if (positionSpawn != null)
                {
                    _aIMana.ChangeMana(-30);
                    OnSpawnDropFigure?.Invoke(
                        positionSpawn, 
                        FiguresTypes.Pawn, 
                        PlayersTypes.SecondPlayer);

                    return true;
                }                
            }  

            return false;       
        }

        private Points GetSpawnPositionPawn()
        {
            var movePositionHuman = GetHumanMove();
            var spawnPoints = new List<Points>();

            for (var i = 0; i < GameBoardCreator.NumberColumns; i++)
            {
                for (var j = 0; j < GameBoardCreator.NumberRows; j++)
                {
                    var positionOnBoard = FigurePositionHandler.GetPositionOnBoard(i, j);
            
                    if (!_ownerFigures.Figures.ContainsKey(positionOnBoard) &&
                        !_humanFigures.Figures.ContainsKey(positionOnBoard) && 
                        !movePositionHuman.Contains(positionOnBoard))
                    {
                        spawnPoints.Add(new Points(i, j));
                    }
                }
            }

            if (spawnPoints.Count != 0)
            {
                var randomPosition = UnityEngine.Random.Range(0, spawnPoints.Count);
                return spawnPoints[randomPosition];
            }
            return null;
        }

        private void ChooseStateMove(FigureData figureData, List<Points> currentMovePoints, List<Points> currentAttackPoints)
        {
            if (currentAttackPoints.Count != 0)
            {
                var positionBestAttack = ChooseBestAttack(currentAttackPoints);
                DoMove(figureData, positionBestAttack);
            }
            else if (currentMovePoints.Count != 0)
            {                
                var bestMovePoints = ChooseBestMove(currentMovePoints, figureData.PositionOnBoard);

                if (bestMovePoints.Count == 0)
                {
                    if (!IsUseImproveFigure(false))
                        DoMove(figureData, GetRandomMove(currentMovePoints));
                    else 
                        OnFigureMove?.Invoke();
                }
                else
                {
                    if (_isAttackOnFigure)
                        DoMove(figureData, GetRandomMove(bestMovePoints));
                    else if (!IsUseImproveFigure(true))
                        DoMove(figureData, GetRandomMove(bestMovePoints));
                    else
                        OnFigureMove?.Invoke();
                }
            }
            else
            {
                OnFigureMove?.Invoke();
            }
        }

        private bool IsUseImproveFigure(bool isRandom)
        {
            var isImproved = false;
            var randomStrategy = UnityEngine.Random.Range(2, 4);

            if (randomStrategy == (int)StrategyType.ImproveFigure || !isRandom)
            {
                var improvers = AbilitiesController.GetImprovers();
                var index = -1;

                if (improvers[0].Cost >= improvers[1].Cost &&
                    _aIMana.GetCurrentMana() - improvers[0].Cost >= 0)
                    index = 0;
                else if (improvers[1].Cost > improvers[0].Cost &&
                    _aIMana.GetCurrentMana() - improvers[1].Cost >= 0)
                    index = 1;
                else if (_aIMana.GetCurrentMana() - improvers[0].Cost >= 0)
                    index = 0;
                else if (_aIMana.GetCurrentMana() - improvers[1].Cost >= 0)
                    index = 1;

                if (index != -1)
                {
                    foreach (var aiFigure in _ownerFigures.Figures)
                    {
                        if (aiFigure.Value.FigureType == FiguresTypes.Pawn)
                        {
                            aiFigure.Value.ImproveFigure(
                                improvers[index].ImageSecond,
                                improvers[index].FigureType,
                                improvers[index].MovePoints
                            );
                            _aIMana.ChangeMana(-improvers[index].Cost);
                            isImproved = true;
                            break;
                        }
                    }
                }
            }

            return isImproved;
        }

        private Points GetRandomMove(List<Points> points)
        {
            var indexMovePosition = UnityEngine.Random.Range(0, points.Count);
            return new Points(
                points[indexMovePosition].X, 
                points[indexMovePosition].Y);
        }

        private void DoMove(FigureData figureData, Points points)
        {
            var positionOnBoard = figureData.PositionOnBoard;

            figureData.Move(
                _ownerFigures,
                _humanFigures,
                points,
                positionOnBoard
            );
        }

        private List<Points> ChooseBestMove(List<Points> points, string currentPositionOnBoard)
        {
            var safeMovePoints = new List<Points>();

            var humanMovePoints = GetHumanMove();

            foreach (var point in points)
            {
                var positionOnBoard = FigurePositionHandler.GetPositionOnBoard(point.X, point.Y);

                if (humanMovePoints.Contains(currentPositionOnBoard))
                    _isAttackOnFigure = true;

                if (!humanMovePoints.Contains(positionOnBoard))
                    safeMovePoints.Add(point);
            }

            return safeMovePoints;
        }

        private HashSet<string> GetHumanMove()
        {
            var humanMovePoints = new HashSet<string>();

            foreach (var humanFigure in _humanFigures.Figures)
            {
                var moveAndAttackPoints = _figureMoveHandler.GetMovePositions(humanFigure.Value);
                var movePoints = moveAndAttackPoints.Item1;
                var attackPoints = moveAndAttackPoints.Item2;
                CheckMoveAndAttackPoints(movePoints, humanMovePoints);

                humanFigure.Value.TryGetComponent<Pawn>(out var pawn);
                if (pawn != null && humanFigure.Value.FigureType == FiguresTypes.Pawn)
                {
                    var pawnAttackPoints = new List<Points>();

                    foreach (var pawnAttackPoint in pawn.AttackPoints)
                    {
                        var positionStepX = pawnAttackPoint.X + pawn.PositionOnIndex.X;
                        var positionStepY = pawnAttackPoint.Y + pawn.PositionOnIndex.Y;
                        var positionStepOnBoard = _figureMoveHandler.GetCheckRange(positionStepX, positionStepY);
                        
                        if (positionStepOnBoard != null)
                        {
                            pawnAttackPoints.Add(new Points(positionStepX, positionStepY));
                        }
                    }

                    CheckMoveAndAttackPoints(pawnAttackPoints, humanMovePoints);
                }
                else 
                {
                    CheckMoveAndAttackPoints(attackPoints, humanMovePoints);
                }
            }

            return humanMovePoints;
        }

        private void CheckMoveAndAttackPoints(List<Points> points, HashSet<string> humanMovePoints)
        {
            foreach (var point in points)
            {
                var positionOnBoard = FigurePositionHandler.GetPositionOnBoard(point.X, point.Y);

                if (!humanMovePoints.Contains(positionOnBoard))
                    humanMovePoints.Add(positionOnBoard);
            }
        }

        private Points ChooseBestAttack(List<Points> points)
        {
            Points pointQueen = null;
            Points pointRook = null;
            Points pointKnight = null;
            Points pointBishop = null;
            Points pointPawn = null;

            foreach (var point in points)
            {
                var positionOnBoard = FigurePositionHandler.GetPositionOnBoard(point.X, point.Y);

                if (!_humanFigures.Figures.ContainsKey(positionOnBoard))
                    continue;
                
                var typeEnemyFigure = _humanFigures.Figures[positionOnBoard].FigureType;

                switch (typeEnemyFigure)
                {
                    case FiguresTypes.King:
                        return point;
                    case FiguresTypes.Queen:
                        pointQueen = point;
                        break;
                    case FiguresTypes.Rook:
                        pointRook = point;
                        break;
                    case FiguresTypes.Knight:
                        pointKnight = point;
                        break;
                    case FiguresTypes.Bishop:
                        pointBishop = point;
                        break;
                    case FiguresTypes.Pawn:
                        pointPawn = point;
                        break;
                }
            }
            
            if (pointQueen != null)
                return pointQueen;
            if (pointRook != null)
                return pointRook;
            if (pointKnight != null)
                return pointKnight;
            if (pointBishop != null)
                return pointBishop;

            return pointPawn;
        }
    }
}

