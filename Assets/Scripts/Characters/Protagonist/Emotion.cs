using System;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Characters.Protagonist
{
    [Serializable]
    public class Emotion
    {
        [Header("Generic parameters")]
        [SerializeField] private EmotionType _emotion;
        [SerializeField] private float _maxValue;
        [SerializeField, Min(0)] private float _initialValue;
        
        [Header("Time-depedent paramaters")]
        [SerializeField, Tooltip("Should the emotion update every second?")] private bool _updateWithTime;
        [SerializeField, Tooltip("% per second")] private float _constantSpeed = 1f;
        [SerializeField] private bool _useVariableSpeed = false;
        [SerializeField, Tooltip("X - Nº of enemies. Y - Update speed (% per scond). Positive value to increase with time. Negative value to decrease with time.")] private AnimationCurve _updateSpeed;

        [Header("Events")]
        [SerializeField] private List<EmotionEvent> _events = new List<EmotionEvent>();
        
        [Min(0)] private float _currentValue;
        private bool _isActive = false;

        public EmotionType EmotionType => _emotion;
        public float GetPercentage => Mathf.Min(_currentValue/_maxValue,1f);
        
        public void Start()
        {
            _currentValue = Mathf.Min(_initialValue, _maxValue);
        }

        public void SetActive(bool result)
        {
            _isActive = result;
        }
        
        public void InvokeEvents()
        {
            foreach (var emotionBarEvent in _events)
            {
                emotionBarEvent.Invoke(_currentValue, _initialValue, _maxValue);
            }
        }

        public void AddToCurrentValue(float amount = 0f)
        {
            if (!_isActive) return;
            
            _currentValue += amount;
            _currentValue = Mathf.Clamp(_currentValue, _currentValue, _maxValue);
        }
        
        public void UpdateWithNearbyEnemies(int numberOfNearbyEnemies)
        {
            if (!_isActive || !_updateWithTime) return;
            DebugInEditor.Log("Speed: "+_updateSpeed.Evaluate(numberOfNearbyEnemies));
            var speed = _useVariableSpeed ? _updateSpeed.Evaluate(numberOfNearbyEnemies) : _constantSpeed;
            _currentValue += Time.deltaTime * speed *_maxValue/100f;
            _currentValue = Mathf.Clamp(_currentValue, _currentValue, _maxValue);
        }
    }
}