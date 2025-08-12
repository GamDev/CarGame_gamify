using System;

namespace Gamify.SnakeGame.EventLog
{
    [Serializable]
    public class EventData
    {
        public int score;
        public float timePlayed;
        public int livesLost;
        public int powerUpsUsed;
        public int obstaclesHit;
    }
}
