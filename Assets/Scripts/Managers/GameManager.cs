using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Gamify.SnakeGame.Car;
using Gamify.SnakeGame.EventLog;
using Gamify.SnakeGame.MessagePostSystem;
using Gamify.SnakeGame.StateMachine;
using UnityEngine;
using Zenject;

namespace Gamify.SnakeGame.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int obstacleCount = 2;
        [SerializeField] private Transform _canvasTrans;
        private PassengerSpawnerManager _passengerSpawnerManager;
        private ObstacleSpawnerManager _obstacleSpawnerManager;
        private StateManagerBase _stateManagerBase;
        public CarMovement.Factory _carMovementFactory;
        private EventLogSystem _eventLogSystem;

        private int _totalLife = 3;
        private int _score = 0;

        [Inject]
        private void Initialized(PassengerSpawnerManager passengerSpawnerManager,
                                 ObstacleSpawnerManager obstacleSpawnerManager,
                                 CarMovement.Factory carMovementFactory,
                                 EventLogSystem eventLogSystem,
                                 UIStateManager uIStateManager)
        {
            _passengerSpawnerManager = passengerSpawnerManager;
            _obstacleSpawnerManager = obstacleSpawnerManager;
            _carMovementFactory = carMovementFactory;
            _eventLogSystem = eventLogSystem;
            _stateManagerBase = uIStateManager;
        }
        void Start()
        {
            InitUI();
            MessagePost.Instance.Listen(MessageIDs.OnGameStarted, HandleOnGameStarted);
            MessagePost.Instance.Listen(MessageIDs.OnLifeLose, HandleOnLifeLose);
            MessagePost.Instance.Listen(MessageIDs.OnPassengerPicked, HandleOnPassengerPicked);
            MessagePost.Instance.Listen(MessageIDs.OnPlayAgain, HandleOnPlayAgain);
        }

        private void HandleOnPlayAgain()
        {
            _totalLife = 3;
            _eventLogSystem.Init();
           _stateManagerBase.ChangeState(UIState.GamePlay);
        }
        private async UniTaskVoid PlayAgain()
        {
            await UniTask.Delay(2000);
          
        }
        private void InitUI()
        {
            _stateManagerBase.Init(_canvasTrans);
            _stateManagerBase.ChangeState(UIState.Loading);
        }
        private void HandleOnGameStarted()
        {
            if (_totalLife >= 3)
                _eventLogSystem.Init();
            NewGame();
        }
        private void HandleOnLifeLose()
        {
            _totalLife--;
            _eventLogSystem.LifeLost();
            _eventLogSystem.ObstacleHit(); // Event Log
            MessagePost<int>.Instance.Broadcast(MessageIDs.OnLifeChanged, _totalLife);

            if (_totalLife <= 0)
            {
                GameOver().Forget();
                return;
            }
            ReplayGame().Forget();
        }
        private void HandleOnPassengerPicked()
        {
            _score++;
            _eventLogSystem.AddScore(_score);
            MessagePost<int>.Instance.Broadcast(MessageIDs.OnScoreChanged, _score);
        }
        private async UniTaskVoid GameOver()
        {
            _eventLogSystem.EndGame();
            await UniTask.Delay(4000);
            _stateManagerBase.ChangeState(UIState.GameOver);
            ResetGame();
            _stateManagerBase.ChangeState(UIState.GameOver);
        }
        private async UniTaskVoid ReplayGame()
        {
            await UniTask.Delay(4000);
            ResetGame();
            var gameplayState = (GamePlayState)_stateManagerBase.GetCurrentState();
             gameplayState.ReplayState();
        }
        private void NewGame()
        {
            CarMovement carMovement = _carMovementFactory.Create();
            carMovement.Init();
            List<Vector2> carPositions = carMovement.GetOccupiedPositions();
            _obstacleSpawnerManager.SpawnObstacles(obstacleCount, carPositions);
            List<Vector2> obstaclePositions = _obstacleSpawnerManager.ObstaclePositions;
            List<Vector2> forbiddenPositions = new List<Vector2>(carPositions);
            forbiddenPositions.AddRange(obstaclePositions);
            _passengerSpawnerManager.SetForbiddenPositions(forbiddenPositions);
            _passengerSpawnerManager.SpawnPassenger();
            carMovement.StartCar();
        }

        private void ResetGame()
        {
            _passengerSpawnerManager.Reset();
            _obstacleSpawnerManager.Reset();
        }

    }
}
