using Gamify.SnakeGame.Car;
using Gamify.SnakeGame.MessagePostSystem;
using UnityEngine;
using Zenject;

namespace Gamify.SnakeGame.CollidableEntities
{
    public class PassengerEntity : MonoBehaviour, ICollidableEntity
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
            carMovement.Grow();
            MessagePost.Instance.Broadcast(MessageIDs.OnPassengerPicked);
            DestroyEntity();
        }
        public void DestroyEntity()
        {
            Destroy(gameObject);
        }
        public class Factory : PlaceholderFactory<PassengerEntity>
        {

        }
    }
}
