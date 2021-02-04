using System;
using Game;
using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameOver
{
    /// <summary>
    /// CLean up at game over
    /// </summary>
    public class GameOverManager : MonoBehaviourPunCallbacks
    {
        private void Start()
        {
            PlayerController.AlivePlayers.Clear();
        }
        
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Launcher");
        }
    }
}
