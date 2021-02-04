using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    /// <summary>
    /// Quit app on back button
    /// </summary>
    public class ExitGameOnBackButton : MonoBehaviour
    {
        private void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
}
