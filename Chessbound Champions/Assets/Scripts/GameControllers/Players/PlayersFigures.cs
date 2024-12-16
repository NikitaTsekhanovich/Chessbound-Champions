using System;
using System.Collections.Generic;
using GameControllers.GameBoard.Figures;
using UnityEngine;

namespace GameControllers.Players
{
    public class PlayersFigures
    {
        private readonly Dictionary<string, FigureData> _figures = new();
        public Dictionary<string, FigureData> Figures => _figures;
        
        public static Action<string> OnChangeFigures;
        public static Action<FiguresTypes, PlayersTypes> OnCheckGameState;

        public void AddFigure(string position, FigureData figureData) => _figures[position] = figureData;
        
        public void DeleteFigure(string position)
        {
            OnCheckGameState.Invoke(Figures[position].FigureType, Figures[position].PlayerType);
            UnityEngine.Object.Destroy(Figures[position].gameObject);
            Figures.Remove(position);
            OnChangeFigures.Invoke(position);
        }
        
        public void ChangePositionOnBoard(string positionFigureOnBoard, string newPositionOnBoard)
        {
            var figure = _figures[positionFigureOnBoard];
            _figures.Remove(positionFigureOnBoard);
            _figures[newPositionOnBoard] = figure;
        }
        
        public void CheckEnemyFigure(string positionOnBoard, AudioSource destroySound)
        {
            if (_figures.ContainsKey(positionOnBoard))
            {
                destroySound.Play();
                DeleteFigure(positionOnBoard);
            }
        }
    }
}
