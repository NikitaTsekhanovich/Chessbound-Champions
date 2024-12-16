using UnityEngine;

namespace GameControllers.GameLaunch.ControllersData
{
    public class GameStateControllerData : MonoBehaviour
    {
        [field: SerializeField] public GameObject WinScreen { get; private set; }
        [field: SerializeField] public GameObject LoseScreen { get; private set; }
        [field: SerializeField] public AudioSource WinSound { get; private set; }
        [field: SerializeField] public AudioSource LoseSound { get; private set; }
    }
}
