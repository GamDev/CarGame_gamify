using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using DG.Tweening;

namespace Gamify.SnakeGame.StateMachine
{
    public class LoadingState : MonoBehaviour ,IState
    {
        [SerializeField] private CanvasGroup _loadingCanvas;
        [SerializeField] private Slider _progressBar;
        [SerializeField] private float _loadTime = 2f;
        [SerializeField] private Ease _progressEase = Ease.OutQuad;
         private StateManagerBase _stateManagerBase;

        private async UniTaskVoid ShowLoadingAsync()
        {
            _progressBar.value = 0f;
            _loadingCanvas.alpha = 0f;
            _loadingCanvas.gameObject.SetActive(true);

            await _loadingCanvas.DOFade(1f, 0.5f).AsyncWaitForCompletion();

            float elapsed = 0f;
            while (elapsed < _loadTime)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / _loadTime);
                _progressBar.DOValue(progress, 0.2f).SetEase(_progressEase);
                await UniTask.Yield();
            }
            await _loadingCanvas.DOFade(0f, 0.5f).AsyncWaitForCompletion();
            _loadingCanvas.gameObject.SetActive(false);
            _stateManagerBase.ChangeState(UIState.Home);
        }

        public void Begin(StateManagerBase stateManagerBase)
        {
            _stateManagerBase = stateManagerBase;
            gameObject.SetActive(true);
            ShowLoadingAsync().Forget();
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }
    }
}

