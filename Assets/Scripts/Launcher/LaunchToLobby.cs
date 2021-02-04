using Photon.Pun;
using Photon.Realtime;

namespace Launcher
{
    public class LaunchToLobby : MonoBehaviourPunCallbacks
    {
        private void LoadLobby()
        {
            if (!PhotonNetwork.IsMasterClient) return;
            
            PhotonNetwork.LoadLevel("Lobby");
        }
        
        public override void OnJoinedRoom()
        {
            LoadLobby();
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            PhotonNetwork.CreateRoom(null, new RoomOptions
            {
                MaxPlayers = 4
            });
        }
    }
}
