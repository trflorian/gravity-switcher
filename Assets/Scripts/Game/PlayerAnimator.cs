using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Handle animation state for player
    /// </summary>
    public class PlayerAnimator : MonoBehaviour
    {
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.enabled = GameManager.GameStarted;
        }
    }
}
