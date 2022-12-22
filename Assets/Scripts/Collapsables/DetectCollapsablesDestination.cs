using System;
using UnityEngine;

namespace Core
{
    [Serializable]
    public class DetectCollapsablesDestination
    {
        [SerializeField] private bool _active = true;
        [SerializeField] private Vector3 _targetPositionsOffset;
        [SerializeField] private bool _shouldCollapseOnHit;


        public Vector3 PositionWithOffset(Vector3 origin)
        {
            Vector3 destination;
            destination.x = origin.x + _targetPositionsOffset.x;
            destination.y = origin.y + _targetPositionsOffset.y;
            destination.z = origin.z + _targetPositionsOffset.z;
            return destination;
        }

        public bool ShouldCollapseOnHit => _shouldCollapseOnHit;
        public bool IsActive => _active;
    }
}