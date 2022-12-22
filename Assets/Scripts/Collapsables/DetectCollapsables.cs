using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.U2D;
using UnityEngineInternal;

namespace Core
{
    public class DetectCollapsables : MonoBehaviour
    {
        [SerializeField] private bool _active = false;
        [SerializeField] private LayerMask _layerMask;
        [SerializeField] private Transform _target;
        [SerializeField] private float _maxAngle = 60f;

        [SerializeField]
        private List<DetectCollapsablesDestination> _destinations = new List<DetectCollapsablesDestination>();

        private List<Collapsable> _previousCollapsedObjects = new List<Collapsable>();
        private List<Collapsable> _newCollapsedObjects = new List<Collapsable>();

        // Update is called once per frame
        void Update()
        {
            if (!_active) return;
            _newCollapsedObjects.Clear();
            foreach (var destination in _destinations)
            {
                if (!destination.IsActive) continue;
                CollapseAllInPathTo(destination);
            }
        }


        private void CollapseAllInPathTo(DetectCollapsablesDestination destination)
        {
            RaycastHit[] hits = new RaycastHit[10];
            var destinationPosition = destination.PositionWithOffset(_target.position);
            var direction = MathHelper.Direction(destinationPosition, transform.position);
            //Or linecast
            var hitCount = Physics.RaycastNonAlloc(transform.position, direction, hits, 1000, _layerMask);

            if (hitCount <= 0)
            {
                UncollapseAllObjects();
                return;
            }

            for (int i = 0; i < hitCount; i++)
            {
                var didHitCollapsable = hits[i].transform.TryGetComponent(out Collapsable collapsable);
                if (!didHitCollapsable) continue;

                CollapseObject(destination, collapsable);
                _newCollapsedObjects.Add(collapsable);
                if (!_previousCollapsedObjects.Contains(collapsable)) _previousCollapsedObjects.Add(collapsable);
            }

            UncollapsePreviousObjects();
        }

        private void UncollapsePreviousObjects()
        {
            for (int i = 0; i < _previousCollapsedObjects.Count; i++)
            {
                bool shouldRemainCollapsed = false;
                for (int j = 0; j < _newCollapsedObjects.Count; j++)
                {
                    shouldRemainCollapsed = _previousCollapsedObjects[i] == _newCollapsedObjects[j];
                    if (shouldRemainCollapsed) break;
                }

                if (shouldRemainCollapsed) continue;

                _previousCollapsedObjects[i].UnCollapse();
                _previousCollapsedObjects.Remove(_previousCollapsedObjects[i]);
            }
        }

        private void UncollapseAllObjects()
        {
            for (int i = 0; i < _previousCollapsedObjects.Count; i++)
            {
                _previousCollapsedObjects[i].UnCollapse();
            }

            _previousCollapsedObjects.Clear();
        }

        private void CollapseObject(DetectCollapsablesDestination destination, Collapsable collapsable)
        {
            if (destination.ShouldCollapseOnHit) collapsable.Collapse();
            else collapsable.UnCollapse();
        }

        public bool IsInBetweenCameraAndPlayer(Collider collider)
        {
            foreach (var destination in _destinations)
            {
                if (!destination.IsActive) continue;
                var destinationPosition = destination.PositionWithOffset(_target.position);
                if (Physics.Linecast(transform.position, destinationPosition, out RaycastHit hit, _layerMask))
                {
                    if (hit.transform == collider.transform) return true;
                }
            }

            return false;
        }

        public bool IsInBetweenCameraAndPlayer(Vector3 forward)
        {
            var angle = Vector3.Angle(transform.forward, forward);
            return angle <= _maxAngle;
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            foreach (var destination in _destinations)
            {
                if (!destination.IsActive) continue;
                Gizmos.color = destination.ShouldCollapseOnHit ? Color.green : Color.red;
                Gizmos.DrawLine(transform.position, destination.PositionWithOffset(_target.position));
            }
        }
#endif
    }
}