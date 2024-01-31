using DG.Tweening;
using UnityEngine;

namespace Effects
{
    public class ScreenEffects : MonoBehaviour
    {
    
        private static ScreenEffects Instance { get; set; }

        private void Awake() => Instance = this;

        private void OnShake(float duration, float strength)
        {
            transform.DOShakePosition(duration, strength);
        }
    
        public static void Shake(float duration, float strength) => Instance.OnShake(duration, strength);
    }
}

