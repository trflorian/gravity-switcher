using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

namespace Launcher
{
    /// <summary>
    /// Connect to master server
    /// </summary>
    public class Launcher : MonoBehaviourPunCallbacks
    {
        public const string PhotonNetworkGameVersion = "1";
        
        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        private void Start()
        {
            ConnectToMasterServer();
        }

        private void ConnectToMasterServer()
        {
            if (PhotonNetwork.IsConnected) return;
            
            PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = PhotonNetworkGameVersion;
        }

        public override void OnConnectedToMaster()
        {
            Debug.Log("Connected to master server");
        }


        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("Disconnected from master server with reason {0}", cause);
        }
    }
}
