using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI.Bars
{
    [Serializable]
    public class BarFillImageUI
    {
        [SerializeField] private FillSmoothMethod _fillSmoothMethod;
        [SerializeField, Tooltip("Only for MoveTowards")] private float _smoothSpeed;
        [SerializeField, Tooltip("Only for DOTween")] private float _smoothTime;
        [SerializeField] private Ease _tweenEase;
        [FormerlySerializedAs("_fillImage")] [SerializeField] private Image _image;
        
        public void SetPercentage(float percentage)
        {
            _image.fillAmount = percentage;
        }
        
        public void UpdateFill(float percentage)
        {
            switch (_fillSmoothMethod)
            {
                case FillSmoothMethod.None:
                    _image.fillAmount = percentage;
                    break;
                case FillSmoothMethod.MoveTowards:
                    _image.fillAmount = Mathf.MoveTowards(_image.fillAmount, percentage, _smoothSpeed*Time.deltaTime);
                    break;
                
                case FillSmoothMethod.DOTween:
                    _image.DOFillAmount(percentage, _smoothTime).SetEase(_tweenEase);
                    break;
            }
        }

    }
}