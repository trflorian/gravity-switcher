using Photon.Pun;
using TMPro;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Save and restore player name from/to input field
    /// </summary>
    public class PlayerNameInputField : MonoBehaviour
    {
        private const string PlayerNameKey = "PlayerName";
        
        private TMP_InputField _playerNameInputField;

        private void Awake()
        {
            _playerNameInputField = GetComponent<TMP_InputField>();
            _playerNameInputField.onValueChanged.AddListener(OnPlayerNameEdited);
            
            var savedPlayerName = PlayerPrefs.GetString(PlayerNameKey, null);
            if (savedPlayerName != null) _playerNameInputField.text = savedPlayerName;
        }

        private void OnPlayerNameEdited(string newPlayerName)
        {
            PlayerPrefs.SetString(PlayerNameKey, newPlayerName);
            PhotonNetwork.NickName = newPlayerName;
        }
    }
}
