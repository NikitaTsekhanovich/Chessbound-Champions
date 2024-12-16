using DG.Tweening;
using TMPro;
using UnityEngine;

namespace GameControllers.GameUI
{
    public class UIManaUpdater : MonoBehaviour
    {
        [SerializeField] private TMP_Text _currentHumanManaText;

        public void AnimationManaText(Color color, float currentMana)
        {
            DOTween.Sequence()
                .Append(_currentHumanManaText.DOColor(color, 0.3f))
                .AppendInterval(0.1f)
                .Append(_currentHumanManaText.DOColor(
                    new Color(
                        0.9764706f,
                        0.6117647f,
                        1f), 0.3f));
            
            _currentHumanManaText.text = $"{currentMana}";
        }
    }
}
