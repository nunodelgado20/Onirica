using UnityEngine;

namespace Characters.Protagonist
{
    [CreateAssetMenu(fileName = "EmotionChange", menuName = "Assets/Emotions/Emotion Change")]
    public class EmotionChangeSO :  ScriptableObject, IEmotionChange
    {
        [SerializeField] private EmotionType _emotionToChange;
        [SerializeField] private float _valueChange;

        public float ValueChange => _valueChange;

        public EmotionType EmotionToChange => _emotionToChange;
    }
}