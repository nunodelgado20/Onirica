using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Characters
{
    [Serializable]
    public class GroundChecker
    {
        [SerializeField] private GroundCheckerType _type;
        [SerializeField] private bool _isActive = true;
        [SerializeField] private bool _activateGizmos = true;
        [Header("Sphere")] [SerializeField] private float _groundedRadius = 0.28f;
        [SerializeField] private Vector3 _offset;
        [Header("Capsule")] [SerializeField] private float _height;
        [SerializeField] private float _capsuleRadius = 0.28f;
        [SerializeField] private Vector3 _capsuleOffset;
        private Collider[] _groundHits = new Collider[1];

        public bool IsGrounded(Transform transform, LayerMask groundLayers)
        {
            if (!_isActive) return false;
            var position = transform.position;
            if (_type == GroundCheckerType.Sphere)
            {
                Vector3 spherePosition =
                    new Vector3(position.x + _offset.x, position.y + _offset.y, position.z + _offset.z);
                return Physics.CheckSphere(spherePosition, _groundedRadius, groundLayers,
                    QueryTriggerInteraction.Ignore);
            }
            else if (_type == GroundCheckerType.OverlapSphere)
            {
                Vector3 spherePosition = new Vector3(position.x + _offset.x, position.y + _offset.y, position.z + _offset.z);
                return Physics.OverlapSphereNonAlloc(spherePosition, _groundedRadius, _groundHits, groundLayers) > 0;
            }
            else if (_type == GroundCheckerType.Capsule)
            {
                Vector3 startPos = new Vector3(position.x + _capsuleOffset.x,
                    position.y + _capsuleOffset.y - _height / 2, position.z + _capsuleOffset.z);
                Vector3 endPos = new Vector3(position.x + _capsuleOffset.x, position.y + _capsuleOffset.y + _height / 2,
                    position.z + _capsuleOffset.z);
                return Physics.CheckCapsule(startPos, endPos, _capsuleRadius, groundLayers,
                    QueryTriggerInteraction.Ignore);
            }
            else if (_type == GroundCheckerType.OverlapCapsule)
            {
                Vector3 startPos = new Vector3(position.x + _capsuleOffset.x,
                    position.y + _capsuleOffset.y - _height / 2, position.z + _capsuleOffset.z);
                Vector3 endPos = new Vector3(position.x + _capsuleOffset.x, position.y + _capsuleOffset.y + _height / 2,
                    position.z + _capsuleOffset.z);
                return Physics.OverlapCapsuleNonAlloc(startPos, endPos, _capsuleRadius, _groundHits, groundLayers) > 0;
            }

            return false;
        }
#if UNITY_EDITOR
        public void OnDrawGizmos(Transform transform, LayerMask groundLayers)
        {
            if (!_activateGizmos) return;
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);
            Color gizmoColor = IsGrounded(transform, groundLayers) ? transparentGreen : transparentRed;

            // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
            var position = transform.position;

            if ((_type == GroundCheckerType.Sphere || _type == GroundCheckerType.OverlapSphere) && _isActive)
            {
                Gizmos.color = gizmoColor;
                Gizmos.DrawSphere(new Vector3(position.x + _offset.x, position.y + _offset.y, position.z + _offset.z),
                    _groundedRadius);
            }
            else if ((_type == GroundCheckerType.Capsule || _type == GroundCheckerType.OverlapCapsule) && _isActive)
            {
                DrawWireCapsule(position + _capsuleOffset, Quaternion.identity, _capsuleRadius, _height, gizmoColor);
            }
        }

        //from: https://github.com/yangruihan/blog/issues/23
        public static void DrawWireCapsule(Vector3 pos, Quaternion rot, float radius, float height,
            Color color = default(Color))
        {
            if (color != default(Color))
                Handles.color = color;

            Matrix4x4 angleMatrix = Matrix4x4.TRS(pos, rot, Handles.matrix.lossyScale);
            using (new Handles.DrawingScope(angleMatrix))
            {
                var pointOffset = (height - (radius * 2)) / 2;

                //draw sideways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.left, Vector3.back, -180, radius);
                Handles.DrawLine(new Vector3(0, pointOffset, -radius), new Vector3(0, -pointOffset, -radius));
                Handles.DrawLine(new Vector3(0, pointOffset, radius), new Vector3(0, -pointOffset, radius));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.left, Vector3.back, 180, radius);
                //draw frontways
                Handles.DrawWireArc(Vector3.up * pointOffset, Vector3.back, Vector3.left, 180, radius);
                Handles.DrawLine(new Vector3(-radius, pointOffset, 0), new Vector3(-radius, -pointOffset, 0));
                Handles.DrawLine(new Vector3(radius, pointOffset, 0), new Vector3(radius, -pointOffset, 0));
                Handles.DrawWireArc(Vector3.down * pointOffset, Vector3.back, Vector3.left, -180, radius);
                //draw center
                Handles.DrawWireDisc(Vector3.up * pointOffset, Vector3.up, radius);
                Handles.DrawWireDisc(Vector3.down * pointOffset, Vector3.up, radius);
            }
        }
        #endif
    }

    [Serializable]
    public enum GroundCheckerType
    {
        Sphere,
        OverlapSphere,
        Capsule,
        OverlapCapsule
    }
}