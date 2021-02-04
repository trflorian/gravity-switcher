using System;
using TMPro;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// Display countdown timer
    /// </summary>
    public class CountdownTimerUI : MonoBehaviour
    {
        private TMP_Text _countdownTimerText;
        
        private void Awake()
        {
            _countdownTimerText = GetComponent<TMP_Text>();
            GameManager.CountdownChanged += UpdateCountdown;
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            GameManager.CountdownChanged -= UpdateCountdown;
        }

        private void UpdateCountdown(int countdownDigit)
        {
            _countdownTimerText.SetText($"{countdownDigit}");
            _countdownTimerText.gameObject.SetActive(countdownDigit > 0);
        }
    }
}
