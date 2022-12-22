using System.Collections.Generic;
using UnityEngine;

namespace Characters
{
    public class GroundCheckers : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayers;
        [SerializeField] private List<GroundChecker> _groundCheckers = new List<GroundChecker>();
        public bool IsGrounded()
        {
            foreach (var groundChecker in _groundCheckers)
            {
                if (groundChecker.IsGrounded(transform, _groundLayers))
                    return true;
            }

            return false;
        }

#if UNITY_EDITOR
        private void Update()
        {
            IsGrounded();
        }
        
        private void OnDrawGizmos()
        {
            foreach (var groundChecker in _groundCheckers)
            {
                groundChecker.OnDrawGizmos(transform, _groundLayers);
            }
        }
#endif
    }

}