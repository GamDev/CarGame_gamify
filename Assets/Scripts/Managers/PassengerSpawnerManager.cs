using Gamify.SnakeGame.CollidableEntities;
using Gamify.SnakeGame.MessagePostSystem;
using UnityEngine;
using Zenject;

namespace Gamify.SnakeGame.Managers
{
    using UnityEngine;
    using Zenject;
    using System.Collections.Generic;

    public class PassengerSpawnerManager
    {
        private PassengerEntity.Factory _passengerEntityFactory;
        public Vector2 _spawnAreaMin = new Vector2(-1.3f, -1.5f);
        public Vector2 _spawnAreaMax = new Vector2(1.3f, 1.5f);
        private List<PassengerEntity> _passengerEntities = new();
        private List<Vector2> _currentForbiddenPositions;

        [Inject]
        private void Initialized(PassengerEntity.Factory passengerEntityFactory,
                                 ObstacleSpawnerManager obstacleSpawnerManager)
        {
            _passengerEntityFactory = passengerEntityFactory;
            MessagePost.Instance.Listen(MessageIDs.OnPassengerPicked, SpawnPassenger);
        }

        public void SetForbiddenPositions(List<Vector2> forbiddenPositions)
        {
            _currentForbiddenPositions = forbiddenPositions;
        }
        public void SpawnPassenger()
        {
            Vector2 position;
            int safetyCounter = 0;

            do
            {
                position = new Vector2(
                    Mathf.Round(Random.Range(_spawnAreaMin.x, _spawnAreaMax.x)),
                    Mathf.Round(Random.Range(_spawnAreaMin.y, _spawnAreaMax.y))
                );

                safetyCounter++;
                if (safetyCounter > 100) break;
            }
            while (_currentForbiddenPositions.Contains(position));

            PassengerEntity passengerEntity = _passengerEntityFactory.Create();
            passengerEntity.UpdatePosition(position);
            _passengerEntities.Add(passengerEntity);
        }

        public void Reset()
        {
            foreach (PassengerEntity passengerEntity in _passengerEntities)
            {
                if (passengerEntity != null)
                {
                    passengerEntity.DestroyEntity();
                }
            }
            _passengerEntities.Clear();
        }
    }
}
