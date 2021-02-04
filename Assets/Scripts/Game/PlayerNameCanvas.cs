using TMPro;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Display player info
    /// </summary>
    public class PlayerNameCanvas : MonoBehaviour
    {
        [SerializeField] private PlayerController playerController;
        [SerializeField] private TMP_Text playerNameText;

        private void Awake()
        {
            var player = playerController.photonView.Controller;
            playerNameText.SetText(player.NickName);
            playerNameText.color = player.IsLocal ? Color.white : Color.gray;

            playerController.GravitySwitchEvent += OnGravitySwitch;
        }

        private void OnDestroy()
        {
            playerController.GravitySwitchEvent -= OnGravitySwitch;
        }

        private void OnGravitySwitch(PlayerController.GravityDirection direction)
        {
            transform.localPosition = new Vector3(transform.localPosition.x, 
                direction == PlayerController.GravityDirection.Up ? -1 : 1, 
                transform.localPosition.z);
        }
    }
}
