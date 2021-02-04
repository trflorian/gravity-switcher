﻿using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Move game camera along with the game
    /// </summary>
    public class GameCamera : MonoBehaviour
    {
        public static float MoveSpeed = 4f;

        private void Update()
        {
            transform.position += new Vector3(1,0,0) * (Time.deltaTime * MoveSpeed);
            MoveSpeed += Time.deltaTime * 0.2f;
        }
    }
}
