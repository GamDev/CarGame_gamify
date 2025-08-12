using System;

namespace Gamify.SnakeGame.StateMachine
{
    public class UIStateManager : StateManagerBase
    {
        UIState activeUIState = UIState.Loading;
        public override void ChangeState(Enum nextState)
        {
            statesList[(int)activeUIState].Exit();
            this.activeUIState = (UIState)nextState;
            statesList[(int)activeUIState].Begin(this);
        }
        public override IState GetCurrentState()
        {
            int stateIndex = (int)activeUIState;
            if (stateIndex >= 0 && stateIndex < statesList.Count)
                return statesList[stateIndex];
            return null;
        }
    }
}