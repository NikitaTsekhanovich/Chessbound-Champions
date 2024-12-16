using System;
using System.Collections.Generic;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLaunch.Properties;

namespace GameControllers.Players
{
    public abstract class PlayerController : IGameController
    {
        protected const int StartMana = 40;

        public static Action OnFigureMove;

        public abstract void UpdateMana(int cost);
        public abstract int GetMana();
        public abstract void ChooseStrategy(FigureData figure, List<Points> currentMovePoints, List<Points> currentAttackPoints);
        public virtual void KillController()
        {
            
        }
    }
}

