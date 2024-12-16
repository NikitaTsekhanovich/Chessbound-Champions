using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SceneLoaderControllers
{
    public class LoadingScreenController : MonoBehaviour
    {
        [SerializeField] private GraphicRaycaster _loadingScreenBlockClick;
        [SerializeField] private Image _background;
        [SerializeField] private Image _loadingImage;
        [SerializeField] private Sprite _loadingSprite1;
        [SerializeField] private Sprite _loadingSprite2;
        [SerializeField] private Sprite _loadingSprite3;
        private Coroutine _loadingTextAnimation;
        private const float DelayAnimation = 0.7f;

        public static LoadingScreenController Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public void ChangeScene(string nameScene)
        {
            _loadingScreenBlockClick.enabled = true;
            StartAnimationFade(nameScene);
        }

        private void StartAnimationFade(string nameScene)
        {
            _loadingTextAnimation = StartCoroutine(StartLoadingTextAnimation());
            _loadingImage.DOFade(1f, DelayAnimation);

            DOTween.Sequence()
                .Append(_background.DOFade(1f, DelayAnimation))
                .AppendInterval(1.5f)
                .AppendCallback(() => LoadScene(nameScene))
                .AppendInterval(0.3f)
                .OnComplete(() => EndAnimationFade());
        }

        private void LoadScene(string nameScene)
        {
            if (nameScene != "")
                SceneManager.LoadSceneAsync(nameScene);
            Time.timeScale = 1f;
        }

        private void EndAnimationFade()
        {
            _loadingImage.DOFade(0f, DelayAnimation);

            DOTween.Sequence()
                .Append(_background.DOFade(0f, DelayAnimation))
                .AppendCallback(() => StopCoroutine(_loadingTextAnimation))
                .AppendCallback(() => _loadingScreenBlockClick.enabled = false)
                .AppendCallback(() => Time.timeScale = 1f);
        }

        private IEnumerator StartLoadingTextAnimation()
        {
            while (true)
            {
                _loadingImage.sprite = _loadingSprite1;
                yield return new WaitForSeconds(0.3f);

                _loadingImage.sprite = _loadingSprite2;
                yield return new WaitForSeconds(0.3f);

                _loadingImage.sprite = _loadingSprite3;
                yield return new WaitForSeconds(0.3f);
            }
        }
    }
}