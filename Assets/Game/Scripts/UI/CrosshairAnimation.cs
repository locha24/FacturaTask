using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.UI
{
    public class CrosshairAnimation : MonoBehaviour
    {
        [SerializeField] private RectTransform crosshair;
        [SerializeField] private float distance = 40f;
        [SerializeField] private float duration = 0.6f;
        [SerializeField] private Ease ease = Ease.InOutSine;

        private float _baseX;
        private Tween _swayTween;

        private void Awake()
        {
            _baseX = crosshair.anchoredPosition.x;
        }

        private void OnEnable()
        {
            crosshair.anchoredPosition = new Vector2(_baseX - distance, crosshair.anchoredPosition.y);

            _swayTween = crosshair
                .DOAnchorPosX(_baseX + distance, duration)
                .SetEase(ease)
                .SetLoops(-1, LoopType.Yoyo)
                .SetUpdate(true)
                .SetLink(gameObject);
        }

        private void OnDisable()
        {
            _swayTween?.Kill();
            crosshair.anchoredPosition = new Vector2(_baseX, crosshair.anchoredPosition.y);
        }
    }
}