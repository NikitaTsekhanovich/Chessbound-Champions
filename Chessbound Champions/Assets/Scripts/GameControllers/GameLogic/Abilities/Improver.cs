using System.Collections.Generic;
using GameControllers.GameBoard.Figures;
using UnityEngine;

namespace GameControllers.GameLogic.Abilities
{
    public class Improver : FigureAbility
    {
        [SerializeField] private List<Points> _movePoints = new();
        [SerializeField] private Sprite _imageFirst;
        [SerializeField] private Sprite _imageSecond;
        private int _numberSlot;

        public List<Points> MovePoints => _movePoints;
        public Sprite ImageFirst => _imageFirst;
        public Sprite ImageSecond => _imageSecond;
        public int NumberSlot => _numberSlot;

        public void InitNumberSlot(int numberSlot)
        {
            _numberSlot = numberSlot;
        }
    }
}
