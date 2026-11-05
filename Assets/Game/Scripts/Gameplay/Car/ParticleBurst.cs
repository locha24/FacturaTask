using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(ParticleSystem), typeof(ParticleDespawner))]
    public sealed class ParticleBurst : MonoBehaviour
    {
        private ParticleSystem _particles;

        private void Awake()
        {
            _particles = GetComponent<ParticleSystem>();
        }

        public void Play(int count, SkinnedMeshRenderer source)
        {
            ParticleSystem.ShapeModule shape = _particles.shape;
            shape.skinnedMeshRenderer = source;

            _particles.Play();
            _particles.Emit(count);
        }
    }
}
