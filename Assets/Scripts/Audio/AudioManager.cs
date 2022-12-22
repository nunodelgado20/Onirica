using Helpers;
using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioMixer _mixer;
        public void PlayClipAtPoint(AudioClip clip, Vector3 position, AudioMixerGroup mixerGroup)
        {
            //AudioSource.PlayClipAtPoint(clip, position);

            GameObject gameObject = new GameObject("One shot audio");
            gameObject.transform.position = position;
            AudioSource audioSource = (AudioSource) gameObject.AddComponent(typeof (AudioSource));
            audioSource.clip = clip;
            audioSource.spatialBlend = 1f;
            audioSource.outputAudioMixerGroup = mixerGroup;
            audioSource.Play();
            Destroy(gameObject, clip.length * ((double) Time.timeScale < 0.00999999977648258 ? 0.01f : Time.timeScale));
        }
    }
}
