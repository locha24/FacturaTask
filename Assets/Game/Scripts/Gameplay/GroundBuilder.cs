using Game.Configs;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public sealed class GroundBuilder : MonoBehaviour
    {
        [SerializeField] private Transform _template;
        [SerializeField] private Transform _car;
        [SerializeField, Min(0f)] private float _visibleDistanceAhead = 70f;

        private LevelConfig _config;

        [Inject]
        private void Construct(LevelConfig config)
        {
            _config = config;
        }

        private void Start()
        {
            Build();
        }

        private void Build()
        {
            Bounds bounds = GetBounds(_template);
            float segmentLength = bounds.size.z;

            float requiredEnd = _car.position.z + _config.Length + _visibleDistanceAhead;
            int segmentCount = Mathf.CeilToInt((requiredEnd - bounds.min.z) / segmentLength);
            
            for (int i = 1; i < segmentCount; i++)
            {
                Vector3 position = _template.position + Vector3.forward * (segmentLength * i);
                Transform segment = Instantiate(_template, position, _template.rotation, _template.parent);
                segment.name = $"{_template.name}_{i:00}";
            }
        }

        private static Bounds GetBounds(Transform root)
        {
            Renderer[] renderers = root.GetComponentsInChildren<Renderer>();
            Bounds bounds = renderers[0].bounds;

            for (int i = 1; i < renderers.Length; i++)
                bounds.Encapsulate(renderers[i].bounds);

            return bounds;
        }
    }
}
