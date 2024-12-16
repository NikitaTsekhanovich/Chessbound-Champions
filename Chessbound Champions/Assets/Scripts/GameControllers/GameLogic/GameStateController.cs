using System;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLaunch.ControllersData;
using GameControllers.GameLaunch.Properties;
using GameControllers.Players;
using UnityEngine;

namespace GameControllers.GameLogic
{
    public sealed class GameStateController : IGameController
    {
        private readonly GameObject _winScreen;
        private readonly GameObject _loseScreen;
        private readonly AudioSource _winSound;
        private readonly AudioSource _loseSound;

        public static Action OnChangeGameState;
        public static Action OnSetWin;
        public static Action OnSetMatch;

        public GameStateController(GameStateControllerData gameStateControllerData)
        {
            _winScreen = gameStateControllerData.WinScreen;
            _loseScreen = gameStateControllerData.LoseScreen;
            _winSound = gameStateControllerData.WinSound;
            _loseSound = gameStateControllerData.LoseSound;

            SubscribeToActions();
        }

        private void SubscribeToActions()
        {
            PlayersFigures.OnCheckGameState += CheckGameState;
        }

        private void CheckGameState(FiguresTypes figureType, PlayersTypes playerType)
        {
            if (figureType == FiguresTypes.King)
            {
                if (playerType == PlayersTypes.FirstPlayer)
                {
                    _loseScreen.SetActive(true);
                    _loseSound.Play();
                }
                else if (playerType == PlayersTypes.SecondPlayer)
                {
                    _winScreen.SetActive(true);
                    _winSound.Play();
                    OnSetWin?.Invoke();
                }

                OnSetMatch.Invoke();
                OnChangeGameState.Invoke();
            }
        }

        public void KillController()
        {
            PlayersFigures.OnCheckGameState -= CheckGameState;
        }
    }
}

