using System.Collections.Generic;
using GameControllers.Players;
using UnityEngine;

namespace GameControllers.GameBoard.Figures
{
    public class FigureStorage : MonoBehaviour
    {
        [SerializeField] private List<FigureData> _figuresData = new ();
        public static Dictionary<PlayersTypes, Dictionary<FiguresTypes, FigureData>> Figures { get; private set; }

        public void SetFigures()
        {
            Figures = new Dictionary<PlayersTypes, Dictionary<FiguresTypes, FigureData>>();
            
            foreach (var figureData in _figuresData)
            {
                if (!Figures.ContainsKey(figureData.PlayerType))
                    Figures[figureData.PlayerType] = new Dictionary<FiguresTypes, FigureData>();
                
                Figures[figureData.PlayerType][figureData.FigureType] = figureData;
            }
        }
    }
}

