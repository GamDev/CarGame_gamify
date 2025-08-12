using System;
using UnityEngine;

namespace Gamify.SnakeGame.Data
{
    public class RegistrationDataStorage
    {
        private const string PlayerPrefsKey = "RegistrationFormData_AllUsers";
        private string _currentPlayerID;
        public string CurrentPlayerID => _currentPlayerID;
        public void SaveNewUserData(RegistrationData newUserData)
        {
            RegistrationDataList allUsers = LoadAllData() ?? new RegistrationDataList();
            newUserData.guid = Guid.NewGuid().ToString();
            _currentPlayerID = newUserData.guid;
            allUsers.players.Add(newUserData);
            string json = JsonUtility.ToJson(allUsers);
            PlayerPrefs.SetString(PlayerPrefsKey, json);
            PlayerPrefs.Save();
            Debug.Log($"Saved new user with GUID: {newUserData.guid}");
        }

        public RegistrationDataList LoadAllData()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKey))
            {
                string json = PlayerPrefs.GetString(PlayerPrefsKey);
                return JsonUtility.FromJson<RegistrationDataList>(json);
            }
            return null;
        }
        public string GetJsonData()
        {
            if (PlayerPrefs.HasKey(PlayerPrefsKey))
            {
                string json = PlayerPrefs.GetString(PlayerPrefsKey);
                return json;
            }
            return null;
        }
    }
}
