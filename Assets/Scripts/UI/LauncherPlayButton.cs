using Photon.Pun;
using TMPro;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Change play button state
    /// </summary>
    public class LauncherPlayButton : MonoBehaviourPunCallbacks
    {
        private enum PlayButtonState
        {
            ConnectingToMaster, JoiningRoom, ReadyToPlay
        }
        
        private Button _playButton;
        private TMP_Text _playButtonText;

        private PlayButtonState _currentState;
        
        private void Awake()
        {
            _playButton = GetComponent<Button>();
            _playButtonText = _playButton.GetComponentInChildren<TMP_Text>();
            _currentState = PlayButtonState.ConnectingToMaster;
            
            var alreadyConnected = PhotonNetwork.IsConnectedAndReady;
            SetState(alreadyConnected ? PlayButtonState.ReadyToPlay : PlayButtonState.ConnectingToMaster);
            
            _playButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            SetState(PlayButtonState.JoiningRoom);
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnConnectedToMaster()
        {
            SetState(PlayButtonState.ReadyToPlay);
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SetState(PlayButtonState.ReadyToPlay);
        }

        private void SetState(PlayButtonState newState)
        {
            string buttonText;
            bool buttonInteractable;
            switch (newState)
            {
                case PlayButtonState.ConnectingToMaster:
                    buttonText = "Connecting...";
                    buttonInteractable = false;
                    break;
                case PlayButtonState.JoiningRoom:
                    buttonText = "Joining lobby...";
                    buttonInteractable = false;
                    break;
                case PlayButtonState.ReadyToPlay:
                    buttonText = "Play";
                    buttonInteractable = true;
                    break;
                default:
                    buttonText = "undefined";
                    buttonInteractable = false;
                    break;
            }
            _playButtonText.SetText(buttonText);
            _playButton.interactable = buttonInteractable;
            _currentState = newState;
        }
    }
}
