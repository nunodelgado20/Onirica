using UnityEngine;

namespace Characters.Protagonist
{
    public class AnimatorData : MonoBehaviour
    {
        [SerializeField] private string _moveAnimParameter = "IsMoving";
        [SerializeField] private string _velocityAnimParameter = "Velocity";
        [SerializeField] private string _jumpAnimParameter = "IsJumping";

        private bool _hasAnimator;
        private Animator _animator;
        public int MoveHash => Animator.StringToHash(_moveAnimParameter);
        public int VelocityHash => Animator.StringToHash(_velocityAnimParameter);
        public int JumpHash => Animator.StringToHash(_jumpAnimParameter);

        private void Awake()
        {
            _hasAnimator = TryGetComponent(out _animator);
        }

        public Animator GetAnimator()
        {
            if (_hasAnimator) return _animator;
            return GetComponent<Animator>();
        }
    }
}
