using UnityEngine;

namespace Mirror
{
    [RequireComponent(typeof(Animator))]
    public class MirrorPiece: MonoBehaviour
    {
        [SerializeField] private string _animParameter = "DidBreak";
        private Animator _animator;

        private int _animHash => Animator.StringToHash(_animParameter);
        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayAnimation()
        {
            _animator.SetBool(_animHash, true);
        }
    }
}