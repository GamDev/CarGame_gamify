
namespace Gamify.SnakeGame.StateMachine
{
    public interface IState
    {
        void Begin(StateManagerBase stateManagerBase);
        void Exit();
    }
}
