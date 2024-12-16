using System;
using System.Collections.Generic;
using GameControllers.GameBoard.Figures;
using UnityEngine;

namespace GameControllers.Players.Human
{
    public class HumanController : PlayerController
    {
        private readonly HumanMana _humanMana;
        
        public static Action<FigureData, List<Points>, List<Points>> OnShowSteps;

        public HumanController(HumanMana humanMana)
        {
            _humanMana = humanMana;
            _humanMana.Init(StartMana);
        }

        public override void UpdateMana(int cost)
        {
            _humanMana.ChangeMana(cost);
        }

        public override int GetMana() => _humanMana.GetCurrentMana();

        public override void ChooseStrategy(FigureData figure, List<Points> currentMovePoints, List<Points> currentAttackPoints)
        {
            if (currentMovePoints.Count != 0 || currentAttackPoints.Count != 0)
                OnShowSteps.Invoke(figure, currentMovePoints, currentAttackPoints);
            else
                OnFigureMove.Invoke();
        }
    }
}

