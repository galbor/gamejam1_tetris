using UnityEngine;

namespace Helpers
{
    public class GameData : MonoBehaviour
    {
        private static GameData Instance { get; set; }

        public static bool Started { get; private set; } = false;
        public static bool Lost { get; private set; } = false;

        private void Awake()
        {
            Instance = this;
            Lost = false;
            EventManagerScript.Instance.StartListening(EventManagerScript.StartGame, (_) => Started = true);
            EventManagerScript.Instance.StartListening(EventManagerScript.PlayerDrowned, (_) => Lost = true);
            EventManagerScript.Instance.StartListening(EventManagerScript.PlayerHit, (_) => Lost = true);
        }

        private void OnRestart()
        {
            Started = false;
            Lost = false;
        }
        public static void Restart() => Instance.OnRestart();
    }
}