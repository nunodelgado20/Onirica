using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Helpers;
using UnityEngine;

namespace Core
{
    
    public class Collapsable : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float _goalTransparency = 0.5f;
        [SerializeField] private float _minAngle = 0f;
        [SerializeField] private float _maxAngle = 60f;
        [SerializeField] private float _uncollapseTimer = 1f;
        private MeshRenderer _mesh;
        private Material[] _materialsInstances;
        private bool _isCollapsed;
        private WaitForSeconds _uncollapseWaitTimer;
        private Camera _camera;
        private bool _isBetweenCameraAndPlayer = false;
        private bool _shouldUncollapse = false;
        private readonly Dictionary<Material, Color> _initialColors = new Dictionary<Material, Color>();
        private readonly Dictionary<Material, Color> _transparentColors = new Dictionary<Material, Color>();

        private void Awake()
        {
            _mesh = GetComponent<MeshRenderer>();
            _uncollapseWaitTimer = new WaitForSeconds(_uncollapseTimer);
            _camera = Camera.main;
        }

        private void Start()
        {
            _materialsInstances = _mesh.materials;
            foreach (var material in _materialsInstances)
            {
                _initialColors.Add(material, material.color);
            }

            foreach (var material in _materialsInstances)
            {
                var transparentColor = material.color;
                transparentColor.a = _goalTransparency;
                _transparentColors.Add(material, transparentColor);
            }

            var angle = Vector3.Angle(_camera.transform.forward, transform.right);
            _isBetweenCameraAndPlayer = (angle >= _minAngle && angle <= _maxAngle);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_isBetweenCameraAndPlayer && other.TryGetComponent(out ITriggerCollapsables collapsablesTrigger))
            {
                _shouldUncollapse = false;
                StopAllCoroutines();
                Collapse();
                _shouldUncollapse = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!_isBetweenCameraAndPlayer) return;
            StartCoroutine(UncollapseRoutine());
        }

        public void Collapse()
        {
/*            if (_isCollapsed) return;
            _mesh.enabled = false;
            _isCollapsed = true;*/
            DebugInEditor.Log("Colapse " + name);
            if (_isCollapsed) return;
            SetMaterialsTransparency(_transparentColors, true);
            _isCollapsed = true;
        }

        public void UnCollapse()
        {
/*            if (!_isCollapsed) return;
            _mesh.enabled = true;
            _isCollapsed = false;*/
            DebugInEditor.Log("UnColapse " + name);
            if (!_isCollapsed) return;
            SetMaterialsTransparency(_initialColors, false);
            _isCollapsed = false;
        }

        private IEnumerator UncollapseRoutine()
        {
            yield return _uncollapseWaitTimer;
            if (_shouldUncollapse) UnCollapse();
        }

        private void SetMaterialsTransparency(Dictionary<Material, Color> value, bool enableTransparency)
        {
            foreach (var materialInstance in _materialsInstances)
            {
                value.TryGetValue(materialInstance, out Color endColor);
                if (enableTransparency)
                    materialInstance.DOColor(endColor, 0.5f).OnStart(() =>
                        ShaderTransparencyEnabler.EnableTransparency(materialInstance, true));
                else
                    materialInstance.DOColor(endColor, 0.5f).OnComplete(() =>
                        ShaderTransparencyEnabler.EnableTransparency(materialInstance, false));
            }
        }
    }
}