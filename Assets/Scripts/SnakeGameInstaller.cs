using Gamify.SnakeGame.CollidableEntities;
using Gamify.SnakeGame.Data;
using Gamify.SnakeGame.EventLog;
using Gamify.SnakeGame.LeaderBoard;
using Gamify.SnakeGame.Managers;
using Gamify.SnakeGame.Car;
using Gamify.SnakeGame.StateMachine;
using UnityEngine;
using Zenject;

public class SnakeGameInstaller : MonoInstaller
{
    [SerializeField] private GameObject _snakeBodyPrefab;
    [SerializeField] private GameObject _foodEntityPrefab;
    [SerializeField] private GameObject _conePrefab;
    [SerializeField] private GameObject _inputManagerPrefab;
    [SerializeField] private GameObject _carPrefab;
    [SerializeField] private GameObject _playerScorePrefab;


    public override void InstallBindings()
    {
        Container.BindFactory<ConeEntity, ConeEntity.Factory>().FromComponentInNewPrefab(_conePrefab);
        Container.BindFactory<PlayerScoreView, PlayerScoreView.Factory>().FromComponentInNewPrefab(_playerScorePrefab);
        Container.BindFactory<CarMovement, CarMovement.Factory>().FromComponentInNewPrefab(_carPrefab);
        Container.BindFactory<CarTail, CarTail.Factory>().FromComponentInNewPrefab(_snakeBodyPrefab);
        Container.BindFactory<PassengerEntity, PassengerEntity.Factory>().FromComponentInNewPrefab(_foodEntityPrefab);
        Container.Bind<InputManager>().FromComponentInNewPrefab(_inputManagerPrefab).AsSingle().NonLazy();
        Container.Bind<PassengerSpawnerManager>().AsSingle();
        Container.Bind<ObstacleSpawnerManager>().AsSingle();
        Container.Bind<RegistrationDataStorage>().AsSingle();
        Container.Bind<UIStateManager>().AsSingle();
        Container.Bind<EventLogSystem>().AsSingle();
        Container.Bind<JsonDataDownloader>().AsSingle();
    }
}