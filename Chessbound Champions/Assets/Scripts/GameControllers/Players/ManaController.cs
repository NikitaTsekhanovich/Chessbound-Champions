using GameControllers.GameUI;
using UnityEngine;

namespace GameControllers.Players
{
    public abstract class ManaController
    {
        private readonly UIManaUpdater _uiManaUpdater;
        private int _currentMana;

        public const int ManaIncreasePerRound = 4;

        protected ManaController(UIManaUpdater uiManaUpdater)
        {
            _uiManaUpdater = uiManaUpdater;
        }

        public void Init(int currentMana)
        {
            _currentMana = currentMana;
        }

        public void ChangeMana(int cost)
        {
            _currentMana += cost;
            _uiManaUpdater.AnimationManaText(cost > 0 ? Color.green : Color.red, _currentMana);
        }

        public int GetCurrentMana() => _currentMana;
    }
}

