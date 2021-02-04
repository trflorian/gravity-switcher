using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using UnityEngine;
using UnityEngine.Events;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Game
{
    /// <summary>
    /// Manage player instantiation and game state
    /// </summary>
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static bool gameStarted;
        
        private const string StartTimeKey = "StartTime";
        private const int StartTimerCountdown = 3;
        
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform[] spawnPoints;

        private static PlayerController LocalPlayerInstance;

        private void Start()
        {
            gameStarted = false;
            
            if(!LocalPlayerInstance) SpawnLocalPlayer();
            else
            {
                LocalPlayerInstance.transform.position = LocalPlayerSpawnPoint();
            }
            LocalPlayerInstance.photonView.RPC("Respawn", RpcTarget.All);

            if (PhotonNetwork.IsMasterClient)
            {
                SetStartTimer();
            }
        }

        private IEnumerator StartCountdown(int startTime)
        {
            while (PhotonNetwork.ServerTimestamp - startTime < StartTimerCountdown * 1000)
            {
                yield return null;
            }

            gameStarted = true;
        }

        private void SetStartTimer()
        {
            var startTimerTable = new Hashtable {{StartTimeKey, PhotonNetwork.ServerTimestamp}};
            PhotonNetwork.CurrentRoom.SetCustomProperties(startTimerTable);
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

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            if (propertiesThatChanged.TryGetValue(StartTimeKey, out var val))
            {
                StartCoroutine(StartCountdown((int) val));
            }
        }
    }
}
