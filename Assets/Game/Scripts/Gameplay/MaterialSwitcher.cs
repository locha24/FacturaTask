using UnityEngine;

namespace Game.Gameplay
{
    public sealed class MaterialSwitcher : MonoBehaviour
    {
        [SerializeField] private Renderer[] _renderers;
        [SerializeField] private Material _flashMaterial;
        [SerializeField, Min(0f)] private float _flashDuration = 0.08f;

        private Material[][] _originals;
        private Material _override;
        private float _flashEndTime;

        private void Awake()
        {
            _originals = new Material[_renderers.Length][];
            for (int i = 0; i < _renderers.Length; i++)
                _originals[i] = _renderers[i].sharedMaterials;
            
            enabled = false;
        }

        public void Flash()
        {
            SetAll(_flashMaterial);
            _flashEndTime = Time.time + _flashDuration;
            enabled = true;
        }

        public void Override(Material material)
        {
            _override = material;
            ShowBase();
        }

        public void Restore()
        {
            _override = null;
            ShowBase();
            enabled = false;
        }

        private void Update()
        {
            if (Time.time < _flashEndTime)
                return;

            ShowBase();
            enabled = false;
        }

        private void ShowBase()
        {
            if (_override != null)
            {
                SetAll(_override);
                return;
            }

            for (int i = 0; i < _renderers.Length; i++)
                _renderers[i].sharedMaterials = _originals[i];
        }

        private void SetAll(Material material)
        {
            foreach (Renderer renderer in _renderers)
            {
                Material[] slots = renderer.sharedMaterials;
                for (int i = 0; i < slots.Length; i++)
                    slots[i] = material;

                renderer.sharedMaterials = slots;
            }
        }
    }
}
