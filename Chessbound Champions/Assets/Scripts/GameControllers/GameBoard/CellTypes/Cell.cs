using System;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLogic.Abilities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameControllers.GameBoard.CellTypes
{
    public class Cell : MonoBehaviour, IDropHandler
    {
        private Points _positionOnIndex;
        private string _positionOnBoard;

        public static Action<Improver, string> OnImproveFigure;
        public static Action<PointerEventData, Points, string, Transform> OnSpawnFigure;

        public void InitPosition(
            int x, int y, 
            string positionOnBoard)
        {
            _positionOnIndex = new Points(x, y);
            _positionOnBoard = positionOnBoard;
        }

        public void OnDrop(PointerEventData eventData)
        {
            eventData.pointerDrag.TryGetComponent<Improver>(out var itemImprover);

            if (itemImprover != null)
                OnImproveFigure.Invoke(itemImprover, _positionOnBoard);
            else
                OnSpawnFigure.Invoke(eventData, _positionOnIndex, _positionOnBoard, transform);
        }
    }
}

