using GameControllers.GameBoard.Figures;
using GameControllers.GameLogic.MoveQueue;
using GameControllers.Players;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameControllers.GameLogic.Abilities
{
    public class FigureAbility : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField] private FiguresTypes _figureType;
        [SerializeField] private PlayersTypes _playerType;
        [SerializeField] private int _cost;
        [SerializeField] private AudioSource _improveSound;

        private Canvas _gameCanvas;
        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        public FiguresTypes FigureType => _figureType;
        public PlayersTypes PlayerType => _playerType;
        public int Cost => _cost;
        public AudioSource ImproveSound => _improveSound;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _gameCanvas = GameObject.FindWithTag("GameCanvas").GetComponent<Canvas>();
        }

        private void OnEnable()
        {
            MoveQueueHandler.OnChangeAbilitiesBlock += ReturnAbility;
        }

        private void OnDisable()
        {
            MoveQueueHandler.OnChangeAbilitiesBlock -= ReturnAbility;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _canvasGroup.blocksRaycasts = false;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.anchoredPosition += eventData.delta / _gameCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ReturnAbility(0);
        }

        private void ReturnAbility(PlayersTypes playerType)
        {
            transform.localPosition = Vector3.zero;
            _canvasGroup.blocksRaycasts = true;
        }

        public void DestroyAbility(PlayerController player)
        {
            player.UpdateMana(-_cost);
            Destroy(gameObject);
        }
    }
}

