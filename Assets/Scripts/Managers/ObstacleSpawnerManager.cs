using System.Collections.Generic;
using Gamify.SnakeGame.CollidableEntities;
using UnityEngine;
using Zenject;

namespace Gamify.SnakeGame.Managers
{
    public class ObstacleSpawnerManager
    {
        private ConeEntity.Factory _coneEntityFactory;
        private Vector2 spawnAreaMin = new Vector2(-1.3f, -1.5f);
        private Vector2 spawnAreaMax = new Vector2(1.3f, 1.5f);

        private List<Vector2> _obstaclePositions = new();
        private List<ConeEntity> _coneEntities = new();
        public List<Vector2> ObstaclePositions => _obstaclePositions;


        [Inject]
        private void Initailized(ConeEntity.Factory codeEntityFactory)
        {
            _coneEntityFactory = codeEntityFactory;
        }
        public void SpawnObstacles(int obstacleCount, List<Vector2> forbiddenPositions)
        {
            _obstaclePositions.Clear();
            _coneEntities.ForEach(c => c.DestroyEntity());
            _coneEntities.Clear();

            for (int i = 0; i < obstacleCount; i++)
            {
                Vector2 position;
                int safetyCounter = 0;

                do
                {
                    position = new Vector2(
                        Mathf.Round(Random.Range(spawnAreaMin.x, spawnAreaMax.x)),
                        Mathf.Round(Random.Range(spawnAreaMin.y, spawnAreaMax.y))
                    );

                    safetyCounter++;
                    if (safetyCounter > 100) break; // Prevent infinite loop
                }
                while (_obstaclePositions.Contains(position) || forbiddenPositions.Contains(position));

                _obstaclePositions.Add(position);

                ConeEntity coneEntity = _coneEntityFactory.Create();
                coneEntity.UpdatePosition(position);
                _coneEntities.Add(coneEntity);
            }
        }

        public void Reset()
        {
            foreach (ConeEntity coneEntity in _coneEntities)
            {
                if (coneEntity != null)
                    coneEntity.DestroyEntity();
            }
            _coneEntities.Clear();
            _obstaclePositions.Clear();
        }
    }
}
