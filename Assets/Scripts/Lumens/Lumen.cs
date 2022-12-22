using Audio;
using Characters.Protagonist;
using UnityEngine;
using UnityEngine.Events;

namespace Lumens
{
    [RequireComponent(typeof(AudioSource))]
    public class Lumen : MonoBehaviour, IEmotionChange
    {
        [Header("Emotions data")]
        [SerializeField] private EmotionType _emotionToChange;
        [SerializeField] private float _emotionValueChange;

        [Header("Events")]
        [SerializeField] private UnityEvent _onTriggerEnter;
        
        [Header("Audio data")]
        [SerializeField] private AudioClip _standardSound;
        [SerializeField] private AudioClip _catchSound;
        private AudioSource _audioSource;
        private AudioManager _audioManager;
        public float ValueChange => _emotionValueChange;
        public EmotionType EmotionToChange => _emotionToChange;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            _audioSource.Stop();
            _audioSource.clip = _standardSound;
            _audioSource.Play();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Emotions emotions))
            {
                emotions.AddToCurrentValue(this);
                
                AudioManager.Instance.PlayClipAtPoint(_catchSound, transform.position, _audioSource.outputAudioMixerGroup);
                _onTriggerEnter?.Invoke();
                gameObject.SetActive(false);
            }
            
        }

    }
}
