using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamify.SnakeGame.MessagePostSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Gamify.SnakeGame.StateMachine
{
    public class GamePlayState : MonoBehaviour, IState
    {
        [SerializeField] private List<Image> _livesImages;
        [SerializeField] private TextMeshProUGUI _scoreText;
        [SerializeField] private Transform _target;
        [SerializeField] private float _scale = 0.95f;
        [SerializeField] private float _duration = 0.5f;
        [SerializeField] private int _loopCount = 4;
        [SerializeField] private CountdownDisplay _countdownDisplay;

        private void UpdateScoreText(int newScore)
        {
            _scoreText.text = newScore.ToString();
        }

        private void UpdateLivesDisplay(int currentLives)
        {
            Animate(_target, _scale, _duration).Forget();

            for (int i = 0; i < _livesImages.Count; i++)
            {
                _livesImages[i].color = (i < currentLives) ? Color.white : Color.gray;
            }
        }
        private async UniTaskVoid Animate(Transform obj, float targetScale, float duration)
        {
            Tween tween = obj.DOScale(targetScale, duration)
                    .SetLoops(_loopCount, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);

            await tween.AsyncWaitForCompletion();

        }

        public void Begin(StateManagerBase stateManagerBase)
        {
            MessagePost<int>.Instance.Listen(MessageIDs.OnScoreChanged, UpdateScoreText);
            MessagePost<int>.Instance.Listen(MessageIDs.OnLifeChanged, UpdateLivesDisplay);
            gameObject.SetActive(true);
            DoCountDown().Forget();
        }
        private async UniTask DoCountDown()
        {
            await _countdownDisplay.PlayCountdownAsync();
            MessagePost.Instance.Broadcast(MessageIDs.OnGameStarted);
        }
        public void Exit()
        {
            ResetLives();
            ResetScore();
            gameObject.SetActive(false);
        }
        private void ResetScore()
        {
            _scoreText.text = "";
        }
        private void ResetLives()
        {
            foreach (Image image in _livesImages)
            {
                image.color = Color.white;
            }
        }
        public void ReplayState()
        {
            DoCountDown().Forget();

        }
    }
}
