using GameControllers.GameBoard;
using GameControllers.GameBoard.CellTypes;
using GameControllers.GameBoard.Figures;
using UnityEngine;

namespace GameControllers.GameLaunch.ControllersData
{
    public class GameBoardData : MonoBehaviour
    {
        [field: SerializeField] public Cell[] PrefabCells { get; private set; }
        [field: SerializeField] public SpawnPositionFiguresData[] PositionsFigures { get; private set; }
        [field: SerializeField] public FlexibleGridLayout GameBoardParent { get; private set; }
    }
}
