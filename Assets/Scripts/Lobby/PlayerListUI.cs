using System;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

namespace Lobby
{
    public class PlayerListUI : MonoBehaviourPunCallbacks
    {
        private GameObject _listEntryPrefab;
        
        private void Awake()
        {
            _listEntryPrefab = transform.GetChild(0).gameObject;

            RefreshPlayerList();
        }

        private void RefreshPlayerList()
        {
            var playerList = PhotonNetwork.CurrentRoom.Players;

            if (playerList.Count == 0)
            {
                Debug.LogError("no players in current room!");
                _listEntryPrefab.SetActive(false);
            }
            else
            {
                for (int i = transform.childCount-1; i >= 1; i--)
                {
                    Destroy(transform.GetChild(i).gameObject);
                }

                var playerIndex = 0;
                foreach (var player in playerList.Values)
                {
                    var entryInstanceObject = playerIndex == 0 ? 
                        _listEntryPrefab : Instantiate(_listEntryPrefab, transform);
                    var playerText = entryInstanceObject.GetComponentInChildren<TMP_Text>();
                    playerText.SetText(player.NickName + (player.IsMasterClient ? " (Master)" : ""));
                    playerIndex++;
                }
            }
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            RefreshPlayerList();
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            RefreshPlayerList();
        }
    }
}
