using Game.Combat;
using Game.Configs;
using Game.Core;
using NTC.Pool;
using UnityEngine;

namespace Game.Gameplay
{

    [RequireComponent(typeof(Health), typeof(CapsuleCollider), typeof(EnemyFeedback))]
    public sealed class Enemy : MonoBehaviour
    {
        private enum State
        {
            Idle,
            Chase
        }

        private static readonly int IdleHash = Animator.StringToHash("Idle");
        private static readonly int RunHash = Animator.StringToHash("Run");

        private const float CrossFadeTime = 0.15f;

        private Health _health;
        private Animator _animator;
        private EnemyFeedback _feedback;

        private EnemyConfig _config;
        private GameFlow _flow;
        private Health _target;
        private BoxCollider _targetBody;

        private State _state;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _animator = GetComponentInChildren<Animator>();
            _feedback = GetComponent<EnemyFeedback>();

            _health.Damaged += OnDamaged;
            _health.Died += OnDied;
        }

        private void OnDestroy()
        {
            if (_health == null)
                return;

            _health.Damaged -= OnDamaged;
            _health.Died -= OnDied;
        }

        public void Init(EnemyConfig config, GameFlow flow, Health target)
        {
            _config = config;
            _flow = flow;
            _target = target;
            _targetBody = target.GetComponent<BoxCollider>();

            _health.Initialize(config.MaxHealth);
            _feedback.ResetVisuals();

            _state = State.Idle;
            _animator.Play(IdleHash, 0, Random.value);
        }

        private void Update()
        {
            if (_flow.State != GameState.Playing)
            {
                if (_state != State.Idle)
                    SetState(State.Idle);
                return;
            }

            if (_state == State.Idle)
                TickIdle();
            else
                TickChase();
        }

        private void TickIdle()
        {
            if (FlatDistance(transform.position, _target.transform.position) <= _config.AggroRadius)
                SetState(State.Chase);
        }

        private void TickChase()
        {
            Vector3 targetPosition = _target.transform.position;
            targetPosition.y = transform.position.y;

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _config.MoveSpeed * Time.deltaTime);
            FaceTowards(targetPosition - transform.position);

            if (TouchesCar())
                HitCar();
        }
        
        private bool TouchesCar()
        {
            Vector3 local = _target.transform.InverseTransformPoint(transform.position) - _targetBody.center;
            Vector3 half = _targetBody.size * 0.5f;

            float dx = Mathf.Max(Mathf.Abs(local.x) - half.x, 0f);
            float dz = Mathf.Max(Mathf.Abs(local.z) - half.z, 0f);

            return dx * dx + dz * dz <= _config.TouchDistance * _config.TouchDistance;
        }

        private void HitCar()
        {
            _target.TakeDamage(_config.ContactDamage);
            _health.TakeDamage(_health.Current);
        }

        private void FaceTowards(Vector3 direction)
        {
            direction.y = 0f;
            if (direction.sqrMagnitude < 0.0001f)
                return;

            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                Quaternion.LookRotation(direction),
                _config.TurnSpeed * Time.deltaTime);
        }

        private void OnDamaged()
        {
            if (!_health.IsDead)
                _feedback.PlayHit();
        }

        private void OnDied()
        {
            _feedback.PlayDeath();
            NightPool.Despawn(gameObject);
        }

        private void SetState(State state)
        {
            _state = state;
            _animator.CrossFadeInFixedTime(state == State.Chase ? RunHash : IdleHash, CrossFadeTime);
        }

        private static float FlatDistance(Vector3 a, Vector3 b)
        {
            a.y = 0f;
            b.y = 0f;
            return Vector3.Distance(a, b);
        }
    }
}
