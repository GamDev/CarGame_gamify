using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Gamify.SnakeGame.StateMachine
{
    public class HomeState : MonoBehaviour,IState
    {
        [SerializeField] private CanvasGroup _canvasGroup;
         private StateManagerBase _stateManagerBase;
        public async UniTaskVoid DisplayScreen()
        {
            _canvasGroup.alpha = 0f;
            gameObject.SetActive(true);
            await _canvasGroup.DOFade(1f, 0.5f).AsyncWaitForCompletion();
        }
        public void OnStartGameButtonClicked()
        {
            
            _stateManagerBase.ChangeState(UIState.GamePlay);
        }
        public void Begin(StateManagerBase stateManagerBase)
        {
            _stateManagerBase = stateManagerBase;
            DisplayScreen().Forget();
        }
        public void Exit()
        {
              gameObject.SetActive(false);
        }
    }
}
