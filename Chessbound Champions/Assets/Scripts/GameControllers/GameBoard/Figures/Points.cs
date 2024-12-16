using UnityEngine;

namespace GameControllers.GameBoard.Figures
{
    [System.Serializable]
    public class Points
    {
        [SerializeField] private int _x;
        [SerializeField] private int _y;

        public int X => _x;
        public int Y => _y;

        public Points(int x, int y)
        {
            _x = x;
            _y = y;
        }
    }
}

