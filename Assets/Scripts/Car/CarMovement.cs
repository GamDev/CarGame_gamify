using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gamify.SnakeGame.CollidableEntities;
using Gamify.SnakeGame.Managers;
using Gamify.SnakeGame.MessagePostSystem;
using UnityEngine;
using Zenject;

namespace Gamify.SnakeGame.Car
{
    public class CarMovement : MonoBehaviour
    {

        public GameObject bodyPrefab;
        public float stepSize = 0.31f;
        [SerializeField] private float speed = 5f;
        private Vector2 direction = Vector2.right;
        private Vector2 nextDirection;
        private float moveTimer = 0f;
        private float moveInterval;
        private List<Transform> bodySegments = new List<Transform>();
        private List<Vector2> positionsHistory = new List<Vector2>();
        private bool _canMove = false;
        private InputManager _inputManager;
        private CarTail.Factory _carTailFactory;

        [Inject]
        private void Initialized(InputManager inputManager,
                                CarTail.Factory carTailFactory)
        {
            _inputManager = inputManager;
            _carTailFactory = carTailFactory;
        }

        public void Init()
        {
            moveInterval = 1f / Mathf.Max(1f, speed); // Prevent division by zero
            nextDirection = direction;
            Vector2 startPos = RoundToGrid(transform.position);
            transform.position = startPos;
            positionsHistory.Add(startPos);
        }
        public void StartCar()
        {
            _canMove = true;
            _inputManager.Reset();
        }
        void FixedUpdate()
        {

            if (!_canMove)
                return;

            nextDirection = _inputManager.CurrentDirection;

            moveTimer += Time.fixedDeltaTime;
            if (moveTimer >= moveInterval)
            {
                moveTimer -= moveInterval; // Maintain precision at high speeds
                MoveOneStep();
            }
            UpdatePositions();
        }
        private void UpdatePositions()
        {
            transform.position = positionsHistory[0];

            float headAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, headAngle);

            for (int i = 0; i < bodySegments.Count; i++)
            {
                Vector2 segmentPos = positionsHistory[i + 1];
                bodySegments[i].position = segmentPos;

                Vector2 dir = (i + 1 < positionsHistory.Count) ? positionsHistory[i] - positionsHistory[i + 1] : direction;
                float bodyAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                bodySegments[i].rotation = Quaternion.Euler(0f, 0f, bodyAngle);
            }
        }
        void MoveOneStep()
        {
            direction = nextDirection;
            Vector2 newHeadPos = RoundToGrid(positionsHistory[0] + direction * stepSize);
            positionsHistory.Insert(0, newHeadPos);

            while (positionsHistory.Count > bodySegments.Count + 1)
                positionsHistory.RemoveAt(positionsHistory.Count - 1);

        }

        public void Grow()
        {
            Vector2 tailPos = positionsHistory[positionsHistory.Count - 1];
            CarTail carTail = _carTailFactory.Create();
            carTail.UpdatePosition(tailPos);
            carTail.ResetScale();
            bodySegments.Add(carTail.transform);
            positionsHistory.Add(tailPos);

            Vector2 dir = (positionsHistory.Count >= 2) ? positionsHistory[positionsHistory.Count - 2] - tailPos : direction;
            float tailAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            carTail.UpdateRotation(tailAngle);
        }

        Vector2 RoundToGrid(Vector2 pos)
        {
            Vector2 rounded = new Vector2(
                Mathf.Round(pos.x / stepSize) * stepSize,
                Mathf.Round(pos.y / stepSize) * stepSize
            );
            return rounded;
        }

        public void LoseLife()
        {
            MessagePost.Instance.Broadcast(MessageIDs.OnLifeLose);
            Stop();
            Die().Forget();
        }
        private async UniTaskVoid Die()
        {
            await UniTask.Delay(2000);
            ClearBody();

            Destroy(gameObject);
        }
        public void Reset()
        {
            ClearBody();
            ResetPoistion();
            Stop();
        }
        public void Stop()
        {
            _canMove = false;
            moveTimer = 0f;
        }
        private void ClearBody()
        {
            foreach (Transform segment in bodySegments)
            {
                Destroy(segment.gameObject);
            }
            bodySegments.Clear();
        }
        public void ResetPoistion()
        {
            direction = Vector2.right;
            nextDirection = direction;
            Vector2 startPos = RoundToGrid(Vector2.zero);
            transform.position = startPos;
            positionsHistory.Clear();
            positionsHistory.Add(startPos);
        }
        public List<Vector2> GetOccupiedPositions()
        {
            return new List<Vector2>(positionsHistory);
        }
        public class Factory : PlaceholderFactory<CarMovement>
        {

        }
    }
}