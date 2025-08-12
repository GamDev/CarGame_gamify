using System.Linq;
using Gamify.SnakeGame.Data;
using Gamify.SnakeGame.LeaderBoard;
using Gamify.SnakeGame.MessagePostSystem;
using TMPro;
using UnityEngine;
using Zenject;

namespace Gamify.SnakeGame.StateMachine
{
    public class LeaderBoardState : MonoBehaviour, IState
    {
        [SerializeField] private Transform _playerParentTrans;
        [SerializeField] private TMP_Text _currentPlayerStatusTxt;
        private PlayerScoreView.Factory _playerScoreViewFactory;
        private RegistrationDataStorage _registrationDataStorage;
        private StateManagerBase _stateManagerBase;
        private JsonDataDownloader _jsonDataDownloader;

        [Inject]
        private void Initialized(PlayerScoreView.Factory playerScoreViewFactory,
                                 RegistrationDataStorage registrationDataStorage,
                                 JsonDataDownloader jsonDataDownloader)
        {
            _playerScoreViewFactory = playerScoreViewFactory;
            _registrationDataStorage = registrationDataStorage;
            _jsonDataDownloader = jsonDataDownloader;
        }

        public void DownloadJsonData()
        {
            string Json = _registrationDataStorage.GetJsonData();
            _jsonDataDownloader.DownloadJSON("PlayerData.json", Json);
        }
        public void PlayAgain()
        {
            MessagePost.Instance.Broadcast(MessageIDs.OnPlayAgain);
        }
        public void DisplayScoreBoard()
        {
            Clear();
            RegistrationDataList registrationDataList = _registrationDataStorage.LoadAllData();
            int count = 1;
            if (registrationDataList == null)
                return;

            var sortedPlayers = registrationDataList.players
                           .OrderByDescending(player => player.score)
                           .ToList();
            RegistrationData registrationData = registrationDataList.players
        .FirstOrDefault(x => x.guid.Equals(_registrationDataStorage.CurrentPlayerID));
            DisplayCurrentPlayedPlayerData(registrationData);

            foreach (RegistrationData item in sortedPlayers)
            {
                PlayerScoreView playerScoreView = _playerScoreViewFactory.Create();
                playerScoreView.SetParent(_playerParentTrans);
                playerScoreView.SetData(count.ToString(),
                $" {item.firstName} {item.lastName}", $"{item.score}");
                count++;
            }
        }
        private void DisplayCurrentPlayedPlayerData(RegistrationData registrationData)
        {
            if (registrationData == null)
                return;
            _currentPlayerStatusTxt.text = $" {registrationData.firstName} {registrationData.lastName} : score = {registrationData.score}";
        }
        public void Begin(StateManagerBase stateManagerBase)
        {
            _stateManagerBase = stateManagerBase;
            gameObject.SetActive(true);
            DisplayScoreBoard();
        }

        public void Exit()
        {
            gameObject.SetActive(false);
            Clear();
        }
        private void Clear()
        {
            foreach (Transform item in _playerParentTrans)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
