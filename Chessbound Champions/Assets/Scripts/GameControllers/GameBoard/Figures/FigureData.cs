using System;
using System.Collections.Generic;
using DG.Tweening;
using GameControllers.Handlers;
using GameControllers.Players;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.GameBoard.Figures
{
    public class FigureData : MonoBehaviour
    {
        [SerializeField] private AudioSource _startMoveSound;
        [SerializeField] private AudioSource _endMoveSound;
        [SerializeField] private AudioSource _destroySound;
        [SerializeField] private Sprite _figureImage;
        [SerializeField] private FiguresTypes _figureType;
        [SerializeField] private PlayersTypes _playerType;
        [SerializeField] private List<Points> _movePoints = new ();
        private string _positionOnBoard;
        private Points _positionOnIndex;
        private float _cellSizeX;
        private float _cellSizeY;
        private Image _imageFigure;

        public Sprite FigureImage => _figureImage;
        public FiguresTypes FigureType => _figureType;
        public PlayersTypes PlayerType => _playerType;
        public string PositionOnBoard => _positionOnBoard;
        public List<Points> MovePoints => _movePoints;
        public Points PositionOnIndex => _positionOnIndex;

        public static Action<FigureData, string> OnChangePosition;
        public static Action OnFigureMove;

        private void Awake()
        {
            _imageFigure = GetComponent<Image>();
        }

        public void SetFigurePosition(string positionOnBoard, Points positionOnIndex)
        {
            _positionOnBoard = positionOnBoard;
            _positionOnIndex = positionOnIndex;
        }

        public void SetSize(float cellSizeX, float cellSizeY)
        {
            _cellSizeX = cellSizeX;
            _cellSizeY = cellSizeY;
        }

        public void ImproveFigure(
            Sprite figureImage, 
            FiguresTypes figureType, 
            List<Points> movePoints)
        {
            _imageFigure.sprite = figureImage;
            _figureImage = figureImage;
            _figureType = figureType;
            _movePoints = movePoints;
        }

        public void Move(
            PlayersFigures ownerFigures, 
            PlayersFigures enemyFigures,
            Points positionStep, 
            string positionFigureOnBoard)
        {
            var offsetX = _positionOnIndex.X - positionStep.X;
            var offsetY = _positionOnIndex.Y - positionStep.Y;

            var newLocalPosition = new Vector3(
                transform.localPosition.x - offsetY * _cellSizeX,
                24f,
                0);

            _startMoveSound.Play();

            var newPositionOnBoard = FigurePositionHandler.GetPositionOnBoard(positionStep.X, positionStep.Y);
            var newPositionOnIndex = new Points(positionStep.X, positionStep.Y);

            OnChangePosition.Invoke(this, newPositionOnBoard);
            ownerFigures.ChangePositionOnBoard(positionFigureOnBoard, newPositionOnBoard);
            SetFigurePosition(newPositionOnBoard, newPositionOnIndex);
            
            DOTween.Sequence()
                .Append(transform.DOLocalMove(newLocalPosition, 1f))
                .AppendCallback(() => UpdatePosition(enemyFigures, newPositionOnBoard));
        }

        private void UpdatePosition(PlayersFigures playersFigures, string newPositionOnBoard)
        {
            playersFigures.CheckEnemyFigure(newPositionOnBoard, _destroySound);
            OnFigureMove.Invoke();
            _endMoveSound.Play();
        }
    }
}

