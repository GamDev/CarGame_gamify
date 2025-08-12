using Gamify.SnakeGame.Car;

namespace Gamify.SnakeGame.CollidableEntities
{
    public interface ICollidableEntity
    {
        void OnCollide(CarMovement carMovement);
    }
}
