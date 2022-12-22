using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Audio
{
    public class VolumeControl : MonoBehaviour
    {
        [SerializeField] private AudioMixerGroup _mixerGroup;
        [SerializeField] private Slider _slider;
        [SerializeField] private float _multiplier = 20f;
        private float _defaultSliderValue = 1; //0 dB
        private void OnEnable()
        {
            _slider.value = PlayerPrefs.GetFloat(_mixerGroup.name, _defaultSliderValue);
            _slider.onValueChanged.AddListener(HandleSliderValueChanged);
        }

        private void OnDisable()
        {
            PlayerPrefs.SetFloat(_mixerGroup.name, _slider.value);
            _slider.onValueChanged.RemoveListener(HandleSliderValueChanged);
        }
    
        private void HandleSliderValueChanged(float sliderValue)
        {
            _mixerGroup.audioMixer.SetFloat(_mixerGroup.name, Mathf.Log10(sliderValue)*_multiplier);
        }

    }
}
