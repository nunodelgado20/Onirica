using UnityEngine;

namespace Animations
{
    public class OnStateExitBool : StateMachineBehaviour
    {
        [SerializeField] private string _boolParameter = "Jump";
        [SerializeField] private bool _exitState = false;

        // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
        public override void OnStateExit(UnityEngine.Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(_boolParameter, _exitState);
        }
    }
}