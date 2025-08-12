using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;

namespace Gamify.SnakeGame.StateMachine
{
    public class CountdownDisplay : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private TextMeshProUGUI _countdownText;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private float _displayDuration = 1f;
        [SerializeField] private GameObject _gameplayUI;
        

        public async UniTask PlayCountdownAsync()
        {
            _gameplayUI.SetActive(true);
            _canvasGroup.alpha = 0f;
            gameObject.SetActive(true);

            string[] countdownWords = { "3", "2", "1", "Go!" };

            foreach (string word in countdownWords)
            {
                _countdownText.text = word;

                await _canvasGroup.DOFade(1f, _fadeDuration).AsyncWaitForCompletion();

                await UniTask.Delay(System.TimeSpan.FromSeconds(_displayDuration));


                await _canvasGroup.DOFade(0f, _fadeDuration).AsyncWaitForCompletion();
            }

            gameObject.SetActive(false);
        }
    }
}
