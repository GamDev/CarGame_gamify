using Gamify.SnakeGame.Car;
using UnityEngine;
using Zenject;

namespace Gamify.SnakeGame.CollidableEntities
{
    public class ConeEntity : MonoBehaviour, ICollidableEntity
    {
        private bool _isCollide = false;
        public Vector3 Position => transform.position;
        public void UpdatePosition(Vector3 position)
        {
            transform.position = position;
        }
        public void OnCollide(CarMovement carMovement)
        {
            if (_isCollide)
                return;
            _isCollide = true;
            carMovement.LoseLife();
            DestroyEntity();
        }
        public void DestroyEntity()
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
        public class Factory : PlaceholderFactory<ConeEntity>
        {

        }
    }
}
