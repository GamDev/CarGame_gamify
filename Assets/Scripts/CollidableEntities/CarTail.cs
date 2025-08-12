using Gamify.SnakeGame.Car;
using UnityEngine;
using Zenject;

namespace Gamify.SnakeGame.CollidableEntities
{
    public class CarTail : MonoBehaviour, ICollidableEntity
    {
       
        public Vector3 Position => transform.position;
        public void OnCollide(CarMovement carMovement)
        {
        
        }

        public void UpdatePosition(Vector3 vector3)
        {
            transform.position = vector3;
        }
        public void ResetScale()
        {
            transform.localScale = Vector3.one; //new Vector3(.6f, .6f, 0f);
        }
        public void UpdateRotation(float tailAngle)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, tailAngle);
        }
        public void DestroyEntity()
        {
            Destroy(gameObject);
        }

        public class Factory : PlaceholderFactory<CarTail>
        {

        }
    }
}
