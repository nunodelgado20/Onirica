using System.Collections.Generic;
using Helpers;
using UnityEngine;

namespace Characters.Protagonist
{
    [RequireComponent(typeof(NearbyEnemies))]
    public class Emotions : MonoBehaviour
    {

        [SerializeField] List<Emotion> _emotions = new List<Emotion>();
        private NearbyEnemies _nearbyEnemies;

        private void Awake()
        {
            _nearbyEnemies=GetComponent<NearbyEnemies>();
        }

        private void Update()
        {
            int nearbyEnemies = _nearbyEnemies.Amount();
            foreach (var emotion in _emotions)
            {
                emotion.UpdateWithNearbyEnemies(nearbyEnemies);
            }

            TryInvokeEvents();
        }

        public void Start()
        {
            foreach (var emotion in _emotions)
            {
                emotion.Start();
            }
        }

        public float GetPercentage(EmotionType emotionType)
        {
            foreach (var emotion in _emotions)
            {
                if (emotion.EmotionType == emotionType)
                    return emotion.GetPercentage;
            }

            DebugInEditor.LogWarning("No current value for emotion " + emotionType + " was found.");
            return 0;
        }

        public void AddToCurrentValue(EmotionType emotionType, float valueIncrement)
        {
            foreach (var emotion in _emotions)
            {
                if (emotion.EmotionType == emotionType)
                {
                    emotion.AddToCurrentValue(valueIncrement);
                    return;
                }
            }

            DebugInEditor.LogWarning("No current value for emotion " + emotionType + " was set.");
        }

        public void AddToCurrentValue(EmotionChangeSO emotionChange)
        {
            foreach (var emotion in _emotions)
            {
                if (emotion.EmotionType == emotionChange.EmotionToChange)
                {
                    emotion.AddToCurrentValue(emotionChange.ValueChange);
                    return;
                }
            }
        }
        
        public void AddToCurrentValue(IEmotionChange emotionChange)
        {
            foreach (var emotion in _emotions)
            {
                if (emotion.EmotionType == emotionChange.EmotionToChange)
                {
                    emotion.AddToCurrentValue(emotionChange.ValueChange);
                    return;
                }
            }
        }

        public void ResetEmotion(EmotionType emotionType)
        {
            foreach (var emotion in _emotions)
            {
                if (emotion.EmotionType == emotionType)
                {
                    emotion.Start();
                    return;
                }
            }

            DebugInEditor.LogWarning("No current value for emotion " + emotionType + " was set.");
        }

        public void EnableEmotion(EmotionType emotionType, bool result)
        {
            foreach (var emotion in _emotions)
            {
                if (emotion.EmotionType == emotionType)
                {
                    emotion.SetActive(result);
                    return;
                }
            }
        }

        private void TryInvokeEvents()
        {
            foreach (var emotion in _emotions)
            {
                emotion.InvokeEvents();
            }
        }



        
    }
}