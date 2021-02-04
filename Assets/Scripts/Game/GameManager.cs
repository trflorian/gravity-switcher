using System.Linq;
using Photon.Pun;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Manage player instantiation and game state
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform[] spawnPoints;

        private static PlayerController LocalPlayerInstance;
        
        private void Start()
        {
            if(!LocalPlayerInstance) SpawnLocalPlayer();
            else
            {
                LocalPlayerInstance.transform.position = LocalPlayerSpawnPoint();
            }
            LocalPlayerInstance.photonView.RPC("Respawn", RpcTarget.All);
        }

        private Vector3 LocalPlayerSpawnPoint()
        {
            var playerIndex = 0;
            var orderedPlayers = PhotonNetwork.CurrentRoom.Players.OrderBy(v => v.Key);
            foreach (var pl in orderedPlayers)
            {
                if (pl.Value.IsLocal) break;
                playerIndex++;
            }

            return spawnPoints[playerIndex].position;
        }
        
        private void SpawnLocalPlayer()
        {
            LocalPlayerInstance = PhotonNetwork
                .Instantiate(playerPrefab.name, LocalPlayerSpawnPoint(), Quaternion.identity)
                .GetComponent<PlayerController>();
        }
    }
}
