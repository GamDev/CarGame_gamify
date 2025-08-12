using Gamify.SnakeGame.CollidableEntities;
using UnityEngine;

namespace Gamify.SnakeGame.Car
{
    public class SnakeCollisionDetector : MonoBehaviour
    {
        [SerializeField] private CarMovement _carMovement;
        void OnTriggerEnter2D(Collider2D other)
        {
            ICollidableEntity collidableEntity = other.GetComponent<ICollidableEntity>();
            collidableEntity.OnCollide(_carMovement);
        }
    }
}
