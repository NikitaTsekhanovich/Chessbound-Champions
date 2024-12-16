using System;
using MainMenu;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SceneLoaderControllers
{
    public class SceneDataLoader : MonoBehaviour
    {
        private ModeGame _modeGame;
        
        public static Action<GameSettings> OnLoadGame;
        public static SceneDataLoader Instance;

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            MenuController.OnStashModeGame += StashModeGame;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            MenuController.OnStashModeGame -= StashModeGame;
        }

        private void Start() 
        {        
            if (Instance == null) 
                Instance = this; 
            else 
                Destroy(this);  
        }
        
        private void StashModeGame(ModeGame modeGame)
        {
            _modeGame = modeGame;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == "StartLoading")
            {
                StartLoadingScene();
            }
            else if (scene.name == "MainMenu")
            {
                
            }
            else if (scene.name == "NetworkMenu")
            {
                
            }
            else if (scene.name == "Game")
            {
                var gameSettings = new GameSettings(_modeGame);
                OnLoadGame.Invoke(gameSettings);
            }
        }

        private void StartLoadingScene()
        {
            LoadingScreenController.Instance.ChangeScene("MainMenu");
        }
    }
}