using NTC.Pool;
using UnityEngine;

namespace Game.Gameplay
{
    public sealed class EnemyFeedback : MonoBehaviour
    {
        [SerializeField] private SkinnedMeshRenderer _renderer;
        [SerializeField] private MaterialSwitcher _materials;
        [SerializeField] private ParticleBurst _burstPrefab;
        [SerializeField, Min(0)] private int _hitParticles = 10;
        [SerializeField, Min(0)] private int _deathParticles = 40;

        public void PlayHit()
        {
            Emit(_hitParticles);
            _materials.Flash();
        }

        public void PlayDeath()
        {
            Emit(_deathParticles);
        }

        public void ResetVisuals()
        {
            _materials.Restore();
        }

        private void Emit(int count)
        {
            ParticleBurst burst = NightPool.Spawn(_burstPrefab, transform.position, Quaternion.identity);
            burst.Play(count, _renderer);
        }
    }
}
