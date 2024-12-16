using GameControllers.GameLogic.MoveQueue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.GameLaunch.ControllersData
{
    public class MoveQueueHandlerData : MonoBehaviour
    {
        [field: SerializeField] public Transform QueueUI { get; private set; }
        [field: SerializeField] public FigureIcon FigureIcon { get; private set; }
        [field: SerializeField] public Image FirstFigureIcon { get; private set; }
        [field: SerializeField] public TMP_Text FirstFigurePosition { get; private set; }
    }
}
