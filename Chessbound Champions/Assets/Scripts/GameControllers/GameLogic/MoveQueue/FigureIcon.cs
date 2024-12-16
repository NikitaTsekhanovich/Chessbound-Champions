using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameControllers.GameLogic.MoveQueue
{
    public class FigureIcon : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _position;

        public void SetData(Sprite icon, string position)
        {
            _icon.sprite = icon;
            _position.text = position;
        }
    }
}

