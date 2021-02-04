using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Lobby
{
    /// <summary>
    /// Lobby manager
    /// </summary>
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        public override void OnLeftRoom()
        {
            SceneManager.LoadScene("Launcher");
        }
    }
}
