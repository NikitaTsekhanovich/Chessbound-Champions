using System;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLogic.Abilities;
using GameControllers.GameLogic.MoveQueue;
using GameControllers.Players;
using UnityEngine;

namespace GameControllers.GameUI
{
    public class AbilitiesBlockSwitcher : MonoBehaviour
    {
        [SerializeField] private GameObject _firstPlayerBlock;
        [SerializeField] private GameObject _secondPlayerBlock;

        private void OnEnable()
        {
            MoveQueueHandler.OnChangeAbilitiesBlock += ChangeBlock;
        }

        private void OnDisable()
        {
            MoveQueueHandler.OnChangeAbilitiesBlock -= ChangeBlock;
        }

        private void ChangeBlock(PlayersTypes playerType)
        {
            if (playerType == PlayersTypes.FirstPlayer)
            {
                _firstPlayerBlock.SetActive(true);
                _secondPlayerBlock.SetActive(false);
            }
            else
            {
                _firstPlayerBlock.SetActive(false);
                _secondPlayerBlock.SetActive(true);
            }
        }
    }
}
