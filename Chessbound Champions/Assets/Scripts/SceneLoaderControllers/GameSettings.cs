using MainMenu;

namespace SceneLoaderControllers
{
    public class GameSettings
    {
        public ModeGame ModeGame { get; private set; }
        
        public GameSettings(ModeGame modeGame)
        {
            ModeGame = modeGame;
        }
    }
}
