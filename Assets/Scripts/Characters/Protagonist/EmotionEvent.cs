using System;
using UnityEngine;
using UnityEngine.Events;

namespace Characters.Protagonist
{
    [Serializable]
    public class EmotionEvent
    {
        private enum EventMoment
        {
            Start,
            End,
            Value
        }

        [SerializeField] private EventMoment _moment;
        [SerializeField] private float _atValue;
        [SerializeField] private UnityEvent _event;
        private bool _hasBeenInvoked = false;
        private float _tinyValue = 0.001f;
        public void Invoke(float currentValue, float initialValue, float maxValue)
        {
            switch (_moment)
            {
                case EventMoment.Value when currentValue*_atValue >= _atValue*_atValue && !_hasBeenInvoked:
                    InvokeEvents();
                    _hasBeenInvoked = true;
                    break;
                case EventMoment.Start when currentValue <= initialValue && !_hasBeenInvoked:
                    InvokeEvents();
                    _hasBeenInvoked = true;
                    break;
                case EventMoment.End when currentValue >= (maxValue-_tinyValue) && !_hasBeenInvoked:
                    InvokeEvents();
                    _hasBeenInvoked = true;
                    break;
                case EventMoment.Value when _hasBeenInvoked && currentValue*_atValue < _atValue*_atValue:
                    _hasBeenInvoked = false;
                    break;
                case EventMoment.Start when _hasBeenInvoked && currentValue > initialValue:
                    _hasBeenInvoked = false;
                    break;
                case EventMoment.End when _hasBeenInvoked && currentValue < (maxValue-_tinyValue):
                    _hasBeenInvoked = false;
                    break;
            }
        }

        private void InvokeEvents() => _event.Invoke();
    }
}