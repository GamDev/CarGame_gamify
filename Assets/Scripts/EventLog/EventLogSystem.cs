using UnityEngine;

namespace Gamify.SnakeGame.EventLog
{
    public class EventLogSystem
    {
        private EventData _eventData;
        private float _gameStartTime;
        public void Init()
        {
            _eventData = new EventData();
            _gameStartTime = Time.time;
        }
        public void AddScore(int amount) => _eventData.score = amount;
        public void LifeLost() => _eventData.livesLost++;
        public void PowerUpUsed() => _eventData.powerUpsUsed++;
        public void ObstacleHit() => _eventData.obstaclesHit++;
        public void EndGame()
        {
            _eventData.timePlayed = Time.time - _gameStartTime;
        }
        public EventData GetEventData() => _eventData;
    }
}
