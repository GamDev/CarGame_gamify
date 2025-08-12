using Cysharp.Threading.Tasks;
using Gamify.SnakeGame.Car;
using UnityEngine;

namespace Gamify.SnakeGame.CollidableEntities
{
    public class WallEntity : MonoBehaviour, ICollidableEntity
    {
        private bool _isCollide = false;
        public void OnCollide(CarMovement carMovement)
        {
            if (_isCollide)
                return;
            _isCollide = true;
            carMovement.LoseLife();
            ResetCollider().Forget();
        }
        private async UniTaskVoid ResetCollider()
        {
            await UniTask.Delay(2000);
            _isCollide = false; 
        }
    }
}
