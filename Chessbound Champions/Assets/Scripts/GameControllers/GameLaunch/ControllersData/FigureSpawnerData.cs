using GameControllers.GameBoard;
using UnityEngine;

namespace GameControllers.GameLaunch.ControllersData
{
    public class FigureSpawnerData : MonoBehaviour
    {
        [field: SerializeField] public Transform[] FiguresParents { get; private set; }
        [field: SerializeField] public Step StepImage { get; private set; }
    }
}
