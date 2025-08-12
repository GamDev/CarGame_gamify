using UnityEngine;

namespace Gamify.SnakeGame.Managers
{
    public class InputManager : MonoBehaviour
    {
        public Vector2 CurrentDirection { get; private set; } = Vector2.right;
        private void Update()
        {
            HandleKeyboardInput();
        }
        private void HandleKeyboardInput()
        {
            if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                                             && CurrentDirection != Vector2.down)
                CurrentDirection = Vector2.up;
            else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                                                  && CurrentDirection != Vector2.up)
                CurrentDirection = Vector2.down;
            else if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                                                  && CurrentDirection != Vector2.right)
                CurrentDirection = Vector2.left;
            else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                                                  && CurrentDirection != Vector2.left)
                CurrentDirection = Vector2.right;
        }
        public void Reset()
        {
            CurrentDirection = Vector2.right;
        }
    }
}
