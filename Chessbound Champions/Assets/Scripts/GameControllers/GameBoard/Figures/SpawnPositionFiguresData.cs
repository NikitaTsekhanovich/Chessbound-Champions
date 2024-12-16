using UnityEngine;

namespace GameControllers.GameBoard.Figures
{
    [CreateAssetMenu(fileName = "SpawnPositionFiguresData", menuName = "Spawn position figures/ Points")]
    public class SpawnPositionFiguresData : ScriptableObject
    {
        [SerializeField] private Points[] _pointsPawnPlayer = new Points[3];
        [SerializeField] private Points _pointKingPlayer;
        [SerializeField] private Points[] _pointsPawnEnemy = new Points[3];
        [SerializeField] private Points _pointKingEnemy;

        public Points[] PointsPawnPlayer => _pointsPawnPlayer;
        public Points PointKingPlayer => _pointKingPlayer;
        public Points[] PointsPawnEnemy => _pointsPawnEnemy;
        public Points PointKingEnemy => _pointKingEnemy;
    }
}

