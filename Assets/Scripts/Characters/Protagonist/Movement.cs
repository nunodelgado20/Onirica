using System;
using Core;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Characters.Protagonist
{
    [RequireComponent(typeof(AnimatorData))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(GroundCheckers))]
    public class Movement : MonoBehaviour, ITriggerCollapsables
    {
        [Header("Movement")] [SerializeField] private bool _faceMoveDirection = false;
        [SerializeField] private float _moveForwardSpeed = 5f;
        [SerializeField] private bool _moveBackwards;
        [SerializeField] private float _moveBackwardsSpeed = 1f;
        [SerializeField] private bool _sprint;
        [SerializeField] private float _sprintSpeed = 10f;
        [SerializeField] private bool _aim;
        [SerializeField] private float _aimAndMoveSpeed = 2.5f;

        [Header("Rotation")] [SerializeField] private float _rotationSmoothTime = 0.12f;

        [Header("Jump")] [SerializeField] private float _jumpHeight = 1.2f;
        [SerializeField] private float _gravityIntensity = 1f;
        [SerializeField] private float _groundedVerticalVelocity = -2f;
        [SerializeField] private float _maxFallSpeed = 50f;
        [SerializeField] private float _maxJumpSpeed = 100f;
        [SerializeField] private float _jumpTimer = 0.15f;

        [Header("Audio")] [SerializeField] private AudioClip _runClip;
        [SerializeField] private AudioClip _jumpClip;

        private GroundCheckers _groundChecker;
        private AnimatorData _animations;
        private CharacterController _controller;
        private Animator _animator;
        private Camera _mainCamera;
        private InputController _input;
        private AudioSource _audioSource;

        private Vector3 _horizontalVelocity;
        private Vector3 _verticalVelocity;
        private Vector3 _localDirection;
        private float _rotationSpeed;
        private float _targetRotationAngle;
        private float _timeSinceLastJump;
        private bool _jumped;

        private Vector3 _smoothVelocity;

        public void SetFaceMoveDirection(bool result) => _faceMoveDirection = result;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _groundChecker = GetComponent<GroundCheckers>();
            _animations = GetComponent<AnimatorData>();
            _audioSource = GetComponent<AudioSource>();
            _mainCamera = FindObjectOfType<Camera>();
        }

        private void Start()
        {
            _input = InputController.Instance;
            _horizontalVelocity = Vector3.zero;
            _animator = _animations.GetAnimator();

            _audioSource.clip = _runClip;
            _audioSource.Stop();
        }

        void Update()
        {
            ApplyMotions();
        }

        public void ApplyMotions()
        {
            ApplyMovement();
            Jump();
            ApplyGravity();
            _controller.Move(Time.deltaTime * (_horizontalVelocity + _verticalVelocity));

            UpdateAnimator();
            UpdateAudio();
        }

        private void ApplyMovement()
        {
            if (_faceMoveDirection)
            {
                MoveFacingDirection(_input.MoveInput());
            }
            else
            {
                MoveFacingForward(_input.MoveInput());
            }
        }

        private void MoveFacingDirection(Vector3 inputDirection)
        {
            float moveSpeed = _input.IsAiming() && _aim ? _aimAndMoveSpeed : _moveForwardSpeed;
            if (_input.IsSprinting() && _sprint) moveSpeed *= _sprintSpeed;
            //var moveInput = context.ReadValue<Vector2>();
            if (inputDirection == Vector3.zero) moveSpeed = 0;

            RotateToFaceDirection(inputDirection);

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotationAngle, 0.0f) * Vector3.forward;
            Vector3 motion = moveSpeed * targetDirection;
            _horizontalVelocity = new Vector3(motion.x, 0, motion.z);
        }

        private void RotateToFaceDirection(Vector3 inputDirection)
        {
            if (inputDirection != Vector3.zero)
            {
                _targetRotationAngle = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                       _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotationAngle,
                    ref _rotationSpeed,
                    _rotationSmoothTime);
                // rotate to face input direction relative to camera position
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }
        }

        private void MoveFacingForward(Vector3 inputDirection)
        {
            bool isWalkingBackwards = inputDirection.z < 0f;

            float moveSpeed = isWalkingBackwards && _moveBackwards ? _moveBackwardsSpeed : _moveForwardSpeed;
            moveSpeed = _input.IsAiming() && _aim ? _aimAndMoveSpeed : moveSpeed;
            if (_input.IsSprinting() && _sprint) moveSpeed *= _sprintSpeed;

            _localDirection =
                transform.TransformDirection(
                    inputDirection); // = Vector3.Lerp(_localDirection, transform.TransformDirection(inputDirection), 0.1f*Time.deltaTime);
            Vector3 motion = moveSpeed * _localDirection;
            _horizontalVelocity = new Vector3(motion.x, 0, motion.z);

            RotateUsingCamera();
        }

        private void RotateUsingCamera()
        {
            //var rotationAngle = _mainCamera.transform.eulerAngles.y;
            var rotationAngle = _mainCamera.transform.eulerAngles.y; // _cameraRig.xAxis.Value;
            transform.localRotation = Quaternion.Euler(0, rotationAngle, 0);
        }

        private void Jump()
        {
            if (!_groundChecker.IsGrounded()) return;

            //if (_controller.velocity.y < _groundedVerticalVelocity)
            if (_verticalVelocity.y < _groundedVerticalVelocity)
            {
                _verticalVelocity.y = _groundedVerticalVelocity;
            }

            _timeSinceLastJump += Time.deltaTime;
            if (_input.Jumped() && _timeSinceLastJump > _jumpTimer)
            {
                var verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * Physics.gravity.y * _gravityIntensity);
                _verticalVelocity = new Vector3(0, verticalVelocity, 0);
                _timeSinceLastJump = 0f;

                _animator.SetBool(_animations.JumpHash, true);
                if(_jumpClip != null) _audioSource.PlayOneShot(_jumpClip);
            }

            if (_groundChecker.IsGrounded() && _verticalVelocity.y < 0f && _animator.GetBool(_animations.JumpHash))
            {
               _animator.SetBool(_animations.JumpHash, false);
            }
        }

        private void ApplyGravity()
        {
            float verticalVelocity = _verticalVelocity.y;
            verticalVelocity += Physics.gravity.y * _gravityIntensity * Time.deltaTime;
            verticalVelocity = Mathf.Clamp(verticalVelocity, -_maxFallSpeed, _maxJumpSpeed);
            _verticalVelocity = new Vector3(0f, verticalVelocity, 0f);
        }

        private void UpdateAnimator()
        {
            var velocity = _controller.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);
            var horizontalVel = Mathf.Abs(localVelocity.z);
            //Vector3 localVelocity = transform.InverseTransformDirection(velocity);

            if (horizontalVel > Mathf.Epsilon)
            {
                _animator.SetBool(_animations.MoveHash, true);
                _animator.SetFloat(_animations.VelocityHash, horizontalVel);
            }
            else
            {
                if (_animator.GetBool(_animations.MoveHash))
                    _animator.SetBool(_animations.MoveHash, false);
                _animator.SetFloat(_animations.VelocityHash, 0);
            }
        }

        private void UpdateAudio()
        {
            bool isMoving = _horizontalVelocity.sqrMagnitude >= Mathf.Epsilon;

            if (isMoving && !_audioSource.isPlaying)
            {
                _audioSource.Play();
                return;
            }

            if (!isMoving && _audioSource.isPlaying)
            {
                _audioSource.Stop();
                return;
            }
        }
    }
}