using System;
using UnityEngine;
using UnityEngine.UI;
using MusicSystem;
using SceneLoaderControllers;

namespace MainMenu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject _education;
        [SerializeField] private Image _currentMusicImage;
        [SerializeField] private Image _currentEffectsImage;
        
        public static Action<ModeGame> OnStashModeGame;
        public static Action OnStartMultiplayer;

        private void Start()
        {
            Screen.orientation = ScreenOrientation.LandscapeRight;
        }

        public void StartSingleGame()
        {
            OnStashModeGame.Invoke(ModeGame.Single);
            LoadingScreenController.Instance.ChangeScene("Game");
        }
        
        public void StartLocalMultiplayerGame()
        {
            OnStashModeGame.Invoke(ModeGame.LocalMultiplayer);
            LoadingScreenController.Instance.ChangeScene("Game");
        }
        
        public void StartMultiplayerGame()
        {
            OnStashModeGame.Invoke(ModeGame.Multiplayer);
            OnStartMultiplayer.Invoke();
        }

        public void StartEducation()
        {
            _education.SetActive(true);
        }

        public void ChangeMusic()
        {
            MusicController.Instance.CheckMusicState(_currentMusicImage);
        }

        public void ChangeEffects()
        {
            MusicController.Instance.CheckSoundEffectsState(_currentEffectsImage);
        }
    }
}

