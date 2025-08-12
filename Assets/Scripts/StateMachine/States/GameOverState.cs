using Cysharp.Threading.Tasks;
using DG.Tweening;
using Gamify.SnakeGame.StateMachine;
using UnityEngine;

namespace Gamify.SnakeGame.UI
{
    public class GameOverState : MonoBehaviour, IState
    {
        [SerializeField] private Transform _gameOverTxtTrans;
        StateManagerBase _stateManagerBase;
        public void Begin(StateManagerBase stateManagerBase)
        {
            _stateManagerBase = stateManagerBase;
            gameObject.SetActive(true);
             DisplayGameOver();
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }
        private void DisplayGameOver()
        {
            _gameOverTxtTrans.DOScale(1, .5f)
                   .SetEase(Ease.OutBack);

            DisplayRegistrationFormScreen().Forget();
           
        }
        private async UniTaskVoid DisplayRegistrationFormScreen()
        {
            await UniTask.Delay(2000);
            _stateManagerBase.ChangeState(UIState.RegistrationForm);
         }
    }
}
