using Characters.Protagonist;
using UnityEngine;

namespace UI.Bars
{
    public class EmotionBar : MonoBehaviour
    {
        [SerializeField] private EmotionType _emotionType;
        [SerializeField] private Emotions _emotions;
        [SerializeField] private EmotionBarUI _barUI = new EmotionBarUI();

        private void Start()
        {
            _emotions.EnableEmotion(_emotionType,true);
            _barUI.SetPercentage(_emotions.GetPercentage(_emotionType));
        }
        
        private void Update()
        {
            _barUI.UpdateFill(_emotions.GetPercentage(_emotionType));
        }
        
        public void SetActive(bool isActive)
        {
            bool avoidResetWhileActive = isActive && gameObject.activeSelf;
            if (avoidResetWhileActive) return;
                
            _emotions.EnableEmotion(_emotionType, isActive);
            
            if (isActive)
            {
                _emotions.ResetEmotion(_emotionType);
                _barUI.SetPercentage(_emotions.GetPercentage(_emotionType));
            }
            
            gameObject.SetActive(isActive);
        }

    }
}
