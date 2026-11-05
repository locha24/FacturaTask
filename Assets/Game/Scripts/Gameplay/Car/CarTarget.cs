using Game.Combat;
using UnityEngine;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Health), typeof(BoxCollider))]
    public sealed class CarTarget : MonoBehaviour
    {
        private Health _health;
        private BoxCollider _body;

        public Vector3 Position => transform.position;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _body = GetComponent<BoxCollider>();
        }

        public void TakeDamage(float amount) => _health.TakeDamage(amount);

        public float SqrDistanceToBody(Vector3 point)
        {
            Vector3 local = transform.InverseTransformPoint(point) - _body.center;
            Vector3 half = _body.size * 0.5f;

            float dx = Mathf.Max(Mathf.Abs(local.x) - half.x, 0f);
            float dz = Mathf.Max(Mathf.Abs(local.z) - half.z, 0f);

            return dx * dx + dz * dz;
        }
    }
}
