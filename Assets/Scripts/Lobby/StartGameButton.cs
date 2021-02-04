using System;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Lobby
{
    /// <summary>
    /// Only master client can control start game button
    /// </summary>
    public class StartGameButton : MonoBehaviour
    {
        private Button _startGameButton;
        
        private void Awake()
        {
            _startGameButton = GetComponent<Button>();
            _startGameButton.onClick.AddListener(StartGame);
            CheckMasterClient();
        }

        private void StartGame()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            PhotonNetwork.LoadLevel("Game");
        }

        private void Update()
        {
            CheckMasterClient();
        }

        private void CheckMasterClient()
        {
            _startGameButton.interactable = PhotonNetwork.IsMasterClient;
        }
    }
}
