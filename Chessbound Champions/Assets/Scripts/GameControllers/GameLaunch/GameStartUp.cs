using System.Collections;
using System.Collections.Generic;
using GameControllers.GameBoard;
using GameControllers.GameBoard.Figures;
using GameControllers.GameLaunch.ControllersData;
using GameControllers.GameLaunch.Properties;
using GameControllers.GameLogic;
using GameControllers.GameLogic.Abilities;
using GameControllers.GameLogic.MoveQueue;
using GameControllers.GameUI;
using GameControllers.Handlers;
using GameControllers.Players;
using GameControllers.Players.AI;
using GameControllers.Players.Human;
using MainMenu;
using Photon.Pun;
using SceneLoaderControllers;
using UnityEngine;

namespace GameControllers.GameLaunch
{
    public class GameStartUp : MonoBehaviourPun
    {
        [SerializeField] private GameBoardData _gameBoardData;
        [SerializeField] private FigureSpawnerData _figureSpawnerData;
        [SerializeField] private MoveQueueHandlerData _moveQueueHandler;
        [SerializeField] private GameStateControllerData _gameStateControllerData;
        [SerializeField] private AbilitiesControllerData _abilitiesControllerData;
        [SerializeField] private UIManaUpdater _uiManaUpdater;
        [SerializeField] private Coroutines _coroutines;
        [SerializeField] private FigureStorage _figureStorage;
        private readonly List<IGameController> _gameControllers = new ();
        
        private void OnEnable()
        {
            SceneDataLoader.OnLoadGame += StartLoadGame;
            PauseController.OnExitGame += ClearControllers;
        }

        private void OnDisable()
        {
            SceneDataLoader.OnLoadGame -= StartLoadGame;
            PauseController.OnExitGame -= ClearControllers;
        }

        private void StartLoadGame(GameSettings gameSettings)
        {
            _figureStorage.SetFigures();
            _coroutines.Initialize();
            StartCoroutine(WaitLoadGridLayout(gameSettings));
        }
        
        private IEnumerator WaitLoadGridLayout(GameSettings gameSettings)
        {
            yield return new WaitForEndOfFrame();
            LoadGameControllers(gameSettings);
        }

        private void LoadGameControllers(GameSettings gameSettings)
        {
            var firstPlayerFigures = new PlayersFigures();
            var firstPlayerMana = new HumanMana(_uiManaUpdater);
            var firstPlayerController = new HumanController(firstPlayerMana);
            _gameControllers.Add(firstPlayerController);

            var secondPlayerFigures = new PlayersFigures();
            FigureMoveHandler figureMoveHandler = null;
            MoveQueueHandler moveQueueHandler  = null;

            if (gameSettings.ModeGame == ModeGame.LocalMultiplayer)
            {
                var secondPlayerMana = new HumanMana(_uiManaUpdater);
                var secondPlayerController = new HumanController(secondPlayerMana);
                _gameControllers.Add(secondPlayerController);
                
                figureMoveHandler = GetCreateFigureMoveHandler(
                    firstPlayerFigures, 
                    secondPlayerFigures, 
                    firstPlayerController,
                    secondPlayerController);
                
                CreateAbilityImprove(
                    firstPlayerFigures, secondPlayerFigures, firstPlayerController, secondPlayerController);
                
                CreateAbilitySpawnFigure(
                    firstPlayerFigures, secondPlayerFigures, firstPlayerController, secondPlayerController);
                
                moveQueueHandler = 
                    CreateMoveQueueHandler(firstPlayerFigures, secondPlayerFigures, 
                        figureMoveHandler, firstPlayerController, secondPlayerController);
            }
            else if (gameSettings.ModeGame == ModeGame.Single)
            {
                var secondPlayerMana = new AIMana(_uiManaUpdater);
                var secondPlayerController = new AIController(
                    secondPlayerMana,
                    firstPlayerFigures,
                    secondPlayerFigures);
                _gameControllers.Add(secondPlayerController);
                
                figureMoveHandler = GetCreateFigureMoveHandler(
                    firstPlayerFigures, 
                    secondPlayerFigures, 
                    firstPlayerController,
                    secondPlayerController);
                
                if (gameSettings.ModeGame == ModeGame.Single)
                    secondPlayerController.InitMoveHandler(figureMoveHandler);
                
                CreateAbilityImprove(
                    firstPlayerFigures, secondPlayerFigures, firstPlayerController, secondPlayerController);

                CreateAbilitySpawnFigure(
                    firstPlayerFigures, secondPlayerFigures, firstPlayerController, secondPlayerController);
                
                moveQueueHandler = 
                    CreateMoveQueueHandler(firstPlayerFigures, secondPlayerFigures, 
                        figureMoveHandler, firstPlayerController, secondPlayerController);
            }

            var abilitiesController = new AbilitiesController(_abilitiesControllerData);
            _gameControllers.Add(abilitiesController);
            
            var figureSpawner = new FigureSpawner(
                _figureSpawnerData, 
                _gameBoardData.GameBoardParent,
                firstPlayerFigures,
                secondPlayerFigures,
                figureMoveHandler);
            _gameControllers.Add(figureSpawner);

            var gameBoardCreator = new GameBoardCreator(
                _gameBoardData,
                moveQueueHandler,
                figureSpawner);
            _gameControllers.Add(gameBoardCreator);

            var gameStateController = new GameStateController(_gameStateControllerData);
            _gameControllers.Add(gameStateController);
        }

        private MoveQueueHandler CreateMoveQueueHandler(PlayersFigures firstPlayerFigures, PlayersFigures secondPlayerFigures,
            FigureMoveHandler figureMoveHandler, HumanController firstPlayerController, PlayerController secondPlayerController)
        {
            var moveQueueHandler = new MoveQueueHandler(
                _moveQueueHandler,
                firstPlayerFigures,
                secondPlayerFigures,
                figureMoveHandler,
                firstPlayerController,
                secondPlayerController);
            _gameControllers.Add(moveQueueHandler);

            return moveQueueHandler;
        }

        private void CreateAbilitySpawnFigure(
            PlayersFigures firstPlayerFigures, 
            PlayersFigures secondPlayerFigures,
            HumanController firstPlayerController, 
            PlayerController secondPlayerController)
        {
            var abilitySpawnFigure = new AbilitySpawn(
                firstPlayerFigures,
                secondPlayerFigures,
                firstPlayerController,
                secondPlayerController);
            _gameControllers.Add(abilitySpawnFigure);
        }

        private void CreateAbilityImprove(
            PlayersFigures firstPlayerFigures, 
            PlayersFigures secondPlayerFigures,
            HumanController firstPlayerController, 
            PlayerController secondPlayerController)
        {
            var abilityImprove = new AbilityImprove(
                firstPlayerFigures,
                secondPlayerFigures,
                firstPlayerController,
                secondPlayerController);
            _gameControllers.Add(abilityImprove);
        }

        private FigureMoveHandler GetCreateFigureMoveHandler(
            PlayersFigures firstPlayerFigures,
            PlayersFigures secondPlayerFigures, 
            HumanController firstPlayerController,
            PlayerController secondPlayerController)
        {
            var figureMoveHandler = new FigureMoveHandler(
                firstPlayerFigures,
                secondPlayerFigures,
                firstPlayerController,
                secondPlayerController);
            
            return figureMoveHandler;
        }

        private void ClearControllers()
        {
            foreach (var gameController in _gameControllers)
                gameController.KillController();
            
            _gameControllers.Clear();
        }
    }
}
