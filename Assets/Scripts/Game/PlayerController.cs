using System;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

namespace Game
{
    /// <summary>
    /// Local and remote player controller
    /// </summary>
    public class PlayerController : MonoBehaviourPun, IPunObservable
    {
        public static HashSet<Player> AlivePlayers = new HashSet<Player>();

        public event UnityAction<GravityDirection> GravitySwitchEvent; 
        
        public const float GravityScale = 2f;
        public const float FlipDirectionInitialVelocity = 4f;

        [SerializeField] private SpriteRenderer playerSpriteRenderer;
        [SerializeField] private Transform colliderObject;
        
        public enum GravityDirection
        {
            Down, Up
        }
        
        private Rigidbody2D _rigidbody;
        private GravityDirection _currentGravityDirection;

        private bool _isAlive;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _isAlive = true;
            _rigidbody = GetComponent<Rigidbody2D>();

#if !UNITY_EDITOR
            if (photonView.IsMine)
            {
                EnhancedTouchSupport.Enable();
                Touch.onFingerDown += OnScreenClicked;
            }
#endif
        }

#if !UNITY_EDITOR
        private void OnScreenClicked(Finger obj)
        {
            if (!photonView.IsMine) return;
            SwitchGravityDirection();
        }

        private void OnDestroy()
        {
            Touch.onFingerDown -= OnScreenClicked;
        }
#endif

        private void FixedUpdate()
        {
            if (GameManager.GameStarted)
            {
                var worldCenter = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0f));
                var distanceToCenter = worldCenter - transform.position;
                var additionalSpeed = distanceToCenter.x * 0.2f;
                
                var vel = _rigidbody.velocity;
                vel.x = GameCamera.MoveSpeed + additionalSpeed;
                _rigidbody.velocity = vel;
            }
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            if (!_isAlive) return;

            var playerScreenPos = _camera.WorldToViewportPoint(transform.position);
            if (playerScreenPos.y < -0.1f || playerScreenPos.y > 1.1f || playerScreenPos.x < 0.02f)
            {
                photonView.RPC("Die", RpcTarget.All);
            }
                
#if UNITY_EDITOR
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                SwitchGravityDirection();
            }
#endif
        }

        private void SwitchGravityDirection()
        {
            SetGravityDirection(_currentGravityDirection == GravityDirection.Down
                ? GravityDirection.Up
                : GravityDirection.Down);
        }

        private void SetGravityDirection(GravityDirection direction)
        {
            _currentGravityDirection = direction;
            var directionSign = direction == GravityDirection.Down ? 1f : -1f;
            _rigidbody.gravityScale = directionSign * GravityScale;
            playerSpriteRenderer.flipY = direction == GravityDirection.Up;
            
            var velocity = _rigidbody.velocity;
            velocity.y = -FlipDirectionInitialVelocity * directionSign;
            _rigidbody.velocity = velocity;

            colliderObject.transform.localScale = new Vector3(1, directionSign, 1);
            
            GravitySwitchEvent?.Invoke(_currentGravityDirection);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsReading)
            {
                var newGravityDirection = (GravityDirection)stream.ReceiveNext();
                SetGravityDirection(newGravityDirection);
            }
            else
            {
                stream.SendNext(_currentGravityDirection);
            }
        }

        [PunRPC]
        public void Die()
        {
            _rigidbody.isKinematic = true;
            _isAlive = false;
            gameObject.SetActive(false);

            int minPlayers = Mathf.Min(1, PhotonNetwork.CurrentRoom.PlayerCount - 1);
            if(AlivePlayers.Count >= minPlayers+1) AlivePlayers.Remove(photonView.Controller);
            if (PhotonNetwork.IsMasterClient)
            {
                if (AlivePlayers.Count <= minPlayers)
                {
                    // restart game
                    PhotonNetwork.LoadLevel("GameOver");
                }
            }
        }

        [PunRPC]
        public void Respawn()
        {
            AlivePlayers.Add(photonView.Controller);
            _rigidbody.isKinematic = false;
            _isAlive = true;
        }
    }
}
