using System;
using System.Linq;
using Game;
using TMPro;
using UnityEngine;

namespace GameOver
{
    /// <summary>
    /// Display winner nickname
    /// </summary>
    public class WinnerText : MonoBehaviour
    {
        private TMP_Text _winnerText;

        private void Awake()
        {
            _winnerText = GetComponent<TMP_Text>();
            _winnerText.SetText("Winner: "+PlayerController.AlivePlayers.FirstOrDefault()?.NickName);
        }
    }
}
