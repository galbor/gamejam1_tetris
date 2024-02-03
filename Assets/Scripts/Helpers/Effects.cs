using DG.Tweening;
using UnityEngine;

namespace Effects
{
    public class ScreenEffects : MonoBehaviour
    {
        private Tweener _shakeTween;


        private static ScreenEffects Instance { get; set; }

        private void Awake() => Instance = this;

        private void OnShake(float duration, float strength)
        {
            _shakeTween = transform.DOShakePosition(duration, strength);
        }
    
        public static void Shake(float duration, float strength) => Instance.OnShake(duration, strength);

        public static void StopShake()
        {
            if (Instance._shakeTween != null && Instance._shakeTween.IsActive())
                Instance._shakeTween.Kill();
        }

    }
}

