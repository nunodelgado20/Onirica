using System;
using Helpers;
using UnityEngine;
using UnityEngine.AI;
using Plane = Helpers.Plane;
using Random = UnityEngine.Random;

namespace Characters.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AudioSource))]
    public class OldGregorController : MonoBehaviour
    {
        [Serializable]
        private enum StopFollowCriteria
        {
            Distance,
            Collider
        }

        [Header("Generic parameters")] [SerializeField, Range(0, 100)]
        private float _probabilityToFollowProtagonist = 50f;
        [SerializeField, Min(0f)] private float _stoppingDistance = 1f;

        [Header("Slow OldGregor - Always follows (global)")] [SerializeField]
        private float _lowSpeed = 3f;

        [SerializeField] private Material _slowMaterial;

        [Header("Fast OldGregor - Stops following (local)")] [SerializeField]
        private float _highSpeed = 5f;
        [SerializeField, Range(1, 20)] private float _aggroRange = 10f;
        [SerializeField] private float _waitTime = 2f;
        [SerializeField] private StopFollowCriteria _stopFollowCriteria;
        [SerializeField, Min(1)] private float _maxDistanceFromSpawn = 20f;
        [SerializeField] private Material _fastMaterial;
        [Header("Animator parameters")] [SerializeField]
        private string _isMovingParameter = "IsMoving";


        private NavMeshAgent _agent;
        private AudioSource _audioSource;
        private Transform _target;
        private Animator _animator;
        private SkinnedMeshRenderer _mesh;

        private Vector3 _spawnPosition;
        private float _speed;
        private float _elapsedTime;
        private bool _hasTarget = false;
        private bool _isReturningToSpawn;
        private bool _shouldReturnToSpawn;
        private bool _alwaysFollowProtagonist;

        private int IsMovingHash => Animator.StringToHash(_isMovingParameter);
        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _audioSource = GetComponent<AudioSource>();
            _animator = GetComponent<Animator>();
            _mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        }

        public void SetTarget(Transform target)
        {
            _target = target;
            _hasTarget = true;
        }

        void Start()
        {
            _elapsedTime = 0f;
            _alwaysFollowProtagonist = ShouldAlwaysFollowProtagonist();
            
            _speed = _alwaysFollowProtagonist ? _lowSpeed : _highSpeed;
            ReplaceMaterials(_alwaysFollowProtagonist ? _slowMaterial : _fastMaterial);

            _agent.speed = _speed;
            _agent.stoppingDistance = _stoppingDistance;
            _spawnPosition = transform.position;
            
            OnMovementStop();
        }

        // Update is called once per frame
        void Update()
        {
            if (!_hasTarget) return;
            if (ShouldStopNearTarget()) return;
            if (ShouldReturnToSpawn()) return;
            FollowTarget();
        }

        private void FollowTarget()
        {
            if (_alwaysFollowProtagonist)
            {
                _agent.SetDestination(_target.position);
                OnMovementStart();
                DebugInEditor.Log("Always follow target");
                return;
            }

            var isTargetInRange = MathHelper.Distance(_target.position, transform.position, Plane.XZ) <= _aggroRange;
            if (isTargetInRange)
            {
                _agent.SetDestination(_target.position);
                OnMovementStart();
                DebugInEditor.Log("Target in range");
                return;
            }

            //TODO Moving Animations? Sound?
        }

        private bool ShouldReturnToSpawn()
        {
            if (_alwaysFollowProtagonist || _stopFollowCriteria != StopFollowCriteria.Distance) return false;
            
            var isFarFromSpawn = MathHelper.Distance(_spawnPosition, transform.position, Plane.XZ) >=
                                 _maxDistanceFromSpawn;

            var canReturnToSpawn = (isFarFromSpawn || _shouldReturnToSpawn) && !_isReturningToSpawn;

            if (canReturnToSpawn)
            {
                OnMovementStop();
                _elapsedTime += Time.deltaTime;
                DebugInEditor.Log("Waiting to return to spawn");
                if (_elapsedTime >= _waitTime)
                {
                    DebugInEditor.Log("Returning to spawn");
                    _agent.SetDestination(_spawnPosition);
                    _isReturningToSpawn = true;
                    _elapsedTime = 0f;
                }

                return true;
            }

            
            var hasReachedSpawn = MathHelper.Distance(_spawnPosition, transform.position, Plane.XZ) <=
                                  _agent.stoppingDistance;
            if (hasReachedSpawn)
            {
                _shouldReturnToSpawn = false;
                _isReturningToSpawn = false;
            }
            
            return _isReturningToSpawn;
        }

        private bool ShouldStopNearTarget()
        {
            var isNearTarget = MathHelper.Distance(_target.position, transform.position, Plane.XZ) <=
                               _agent.stoppingDistance;
            if (isNearTarget)
            {
                OnMovementStop();
                _agent.SetDestination(transform.position);
                DebugInEditor.Log("Stopping near target");
                return true;
            }

            return false;
        }

        //TODO? implementation of returning back with colliders.
        private void OnTriggerEnter(Collider other)
        {
            if (_alwaysFollowProtagonist || _shouldReturnToSpawn || _stopFollowCriteria != StopFollowCriteria.Collider ) return;

            /*if (other.TryGetComponent(out XXX xxx))
            {
                _shouldReturnToSpawn = true;
            }*/
        }

        private bool ShouldAlwaysFollowProtagonist()
        {
            float randomValue = Random.Range(0, 100f);
            return randomValue <= _probabilityToFollowProtagonist;
        }

        private void OnMovementStop()
        {
            transform.LookAt(_target);

            if(_audioSource.isPlaying) _audioSource.Stop();
            _animator.SetBool(IsMovingHash,false);

        }

        private void OnMovementStart()
        {
            if(!_audioSource.isPlaying) _audioSource.Play();
            _animator.SetBool(IsMovingHash,true);
        }

        private void ReplaceMaterials(Material newMaterial)
        {
            var materials = _mesh.materials;
            for (int i = 0; i < materials.Length; i++)
            {
                materials[i] = newMaterial;
            }

            _mesh.materials = materials;
        }
    }
}