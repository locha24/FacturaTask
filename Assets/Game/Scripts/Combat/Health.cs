using System;
using UnityEngine;

namespace Game.Combat
{
    public sealed class Health : MonoBehaviour, IDamageable
    {
        public float Max { get; private set; }
        public float Current { get; private set; }
        public bool IsDead => Current <= 0f;

        public event Action<float, float> Changed;
        
        public event Action Damaged;

        public event Action Died;

        public void Initialize(float max)
        {
            Max = max;
            Current = max;
            Changed?.Invoke(Current, Max);
        }

        public void TakeDamage(float amount)
        {
            if (IsDead)
                return;

            Current = Mathf.Max(0f, Current - amount);
            Changed?.Invoke(Current, Max);
            Damaged?.Invoke();

            if (IsDead)
                Died?.Invoke();
        }
    }
}
