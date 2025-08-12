using System;
using System.Collections.Generic;

namespace Gamify.SnakeGame.Data
{
    [Serializable]
    public class RegistrationData
    {
        public string guid;
        public string firstName;
        public string lastName;
        public string email;
        public int age;
        public bool agreedToTerms;
        public int score;
        public float gamePlayedTime;
        public int livesLost;
        public int obstaclesHit;
    }

    [Serializable]
    public class RegistrationDataList
    {
        public List<RegistrationData> players = new List<RegistrationData>();
    }
}
