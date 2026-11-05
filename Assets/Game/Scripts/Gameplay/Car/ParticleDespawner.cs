using NTC.Pool;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(ParticleSystem))]
    public sealed class ParticleDespawner : MonoBehaviour
    {
        private ParticleSystem _particles;

        private void Awake()
        {
            _particles = GetComponent<ParticleSystem>();
        }

        private void Update()
        {
            if (!_particles.IsAlive(true))
                NightPool.Despawn(gameObject);
        }
    }
}
