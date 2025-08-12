using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Gamify.SnakeGame.Data;
using Zenject;
using Gamify.SnakeGame.EventLog;

namespace Gamify.SnakeGame.StateMachine
{
    public class RegistrationFormState : MonoBehaviour, IState
    {

        [SerializeField] private TMP_InputField _firstNameInput;
        [SerializeField] private TMP_InputField _lastNameInput;
        [SerializeField] private TMP_InputField _emailInput;
        [SerializeField] private TMP_InputField _ageInput;
        [SerializeField] private Toggle _toggleTerms;
        [SerializeField] private TextMeshProUGUI _errorMessage;
         private RegistrationDataStorage _registrationDataStorage;
         private StateManagerBase _stateManagerBase;
        private EventLogSystem _eventLogSystem;

        [Inject]
        private void Initialized(RegistrationDataStorage registrationDataStorage,
                                 EventLogSystem eventLogSystem)
        {
            _registrationDataStorage = registrationDataStorage;
            _eventLogSystem = eventLogSystem;
        }


        private void ClearForm()
        {
            _firstNameInput.text = "";
            _lastNameInput.text = "";
            _emailInput.text = "";
            _ageInput.text = "";
            _toggleTerms.isOn = false;
            _errorMessage.text = "";
        }

        public void OnSaveButtonClicked()
        {
            string firstName = _firstNameInput.text.Trim();
            string lastName = _lastNameInput.text.Trim();
            string email = _emailInput.text.Trim();
            string ageText = _ageInput.text.Trim();


            if (string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(lastName) ||
                string.IsNullOrEmpty(email) ||
                string.IsNullOrEmpty(ageText))
            {
                ShowError("Please fill in all fields.");
                return;
            }

            if (!int.TryParse(ageText, out int age))
            {
                ShowError("Age must be a number.");
                return;
            }

            if (!email.Contains("@") || !email.Contains("."))
            {
                ShowError("Please enter a valid email address.");
                return;
            }

            if (!_toggleTerms.isOn)
            {
                ShowError("You must agree to the terms.");
                return;
            }

            ShowError("");
            EventData eventData = _eventLogSystem.GetEventData();
            RegistrationData data = new RegistrationData
            {
                firstName = _firstNameInput.text.Trim(),
                lastName = _lastNameInput.text.Trim(),
                email = _emailInput.text.Trim(),
                age = int.Parse(_ageInput.text.Trim()),
                agreedToTerms = _toggleTerms.isOn,
                livesLost = eventData.livesLost,
                obstaclesHit = eventData.obstaclesHit,
                gamePlayedTime = eventData.timePlayed,
                score = eventData.score
            };
            _registrationDataStorage.SaveNewUserData(data);
            _stateManagerBase.ChangeState(UIState.Leaderboard);
        }

        private void ShowError(string message)
        {
            _errorMessage.text = message;
        }
        public void Begin(StateManagerBase stateManagerBase)
        {
            _stateManagerBase = stateManagerBase;
            gameObject.SetActive(true);
            ClearForm();
        }

        public void Exit()
        {
            gameObject.SetActive(false);
        }
    }

}
