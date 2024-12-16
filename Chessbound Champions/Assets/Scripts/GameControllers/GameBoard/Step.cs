using System;
using GameControllers.GameBoard.Figures;
using GameControllers.Players;
using GameControllers.Players.Human;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.GameBoard
{
    [RequireComponent(typeof(Image))]
    public class Step : MonoBehaviour
    {
        [SerializeField] private Image _stepImage;
        private Points _currentPositionStep;
        private string _positionFigureOnBoard;
        private PlayersFigures _firstPlayerFigures;
        private PlayersFigures _secondPlayerFigures;

        public static Func<FigureData> GetCurrentFigure;

        public void Init(
            Points currentPositionStep, 
            string positionFigureOnBoard,
            PlayersFigures firstPlayerFigures,
            PlayersFigures secondPlayerFigures)
        {
            _currentPositionStep = currentPositionStep;
            _positionFigureOnBoard = positionFigureOnBoard;
            _firstPlayerFigures = firstPlayerFigures;
            _secondPlayerFigures = secondPlayerFigures;
        }
        
        public void OffImage() => _stepImage.color = new Color(0f, 0f, 0f, 0f);

        public void MoveFigure()
        {
            var currentFigure = GetCurrentFigure();
            
            if (currentFigure.PlayerType == PlayersTypes.FirstPlayer)
                _firstPlayerFigures.Figures[_positionFigureOnBoard].Move(
                    _firstPlayerFigures,
                    _secondPlayerFigures,
                    _currentPositionStep,
                    _positionFigureOnBoard);
            else 
                _secondPlayerFigures.Figures[_positionFigureOnBoard].Move(
                    _secondPlayerFigures,
                    _firstPlayerFigures,
                    _currentPositionStep,
                    _positionFigureOnBoard);
        }
    }
}

