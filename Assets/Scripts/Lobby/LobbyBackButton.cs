using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Lobby
{
    /// <summary>
    /// Handle back button in lobby scene
    /// </summary>
    public class LobbyBackButton : MonoBehaviour
    {
        private void Awake()
        {
            var button = GetComponent<Button>();
            button.onClick.AddListener(OnBackButton);
        }

        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                OnBackButton();
            }
        }

        private void OnBackButton()
        {
            PhotonNetwork.LeaveRoom();
        }
    }
}
