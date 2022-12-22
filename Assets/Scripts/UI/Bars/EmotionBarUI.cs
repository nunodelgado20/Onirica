using System;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Bars
{
    
    [Serializable]
    public class EmotionBarUI
    {
        [SerializeField] private List<BarFillImageUI> _fillImages = new List<BarFillImageUI>();
        
        public void SetPercentage(float percentage)
        {
            foreach (var image in _fillImages)
            {
                image.SetPercentage(percentage);
            }
        }

        public void UpdateFill(float percentage)
        {
            foreach (var image in _fillImages)
            {
                image.UpdateFill(percentage);
            }
        }

    }
    
}