using System;
using MusicSystem;
using SceneLoaderControllers;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.GameUI
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject _education;
        [SerializeField] private Image _currentMusicImage;
        [SerializeField] private Image _currentEffectsImage;
        [SerializeField] private Sprite _musicOffImage;
        [SerializeField] private Sprite _effectsOffImage;

        public static Action OnExitGame;

        private void Start()
        {
            if (PlayerPrefs.GetInt(MusicDataKeys.MusicIsOnKey) == (int)ModeMusic.Off)
                _currentMusicImage.sprite = _musicOffImage;
            if (PlayerPrefs.GetInt(MusicDataKeys.EffectsIsOnKey) == (int)ModeMusic.Off)
                _currentEffectsImage.sprite = _effectsOffImage;
        }

        public void BackToMenu()
        {
            OnExitGame.Invoke();
            LoadingScreenController.Instance.ChangeScene("MainMenu");
        }

        public void RestartGame()
        {
            OnExitGame.Invoke();
            LoadingScreenController.Instance.ChangeScene("Game");
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

