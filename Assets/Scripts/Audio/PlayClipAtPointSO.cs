using UnityEngine;
using UnityEngine.Audio;

namespace Audio
{
    [CreateAssetMenu(fileName = "PlayClipAtPoint", menuName = "Assets/Audio/Play Clip At Point")]
    public class PlayClipAtPointSO : ScriptableObject
    {
        [SerializeField] private AudioClip _clip;
        [SerializeField] private AudioMixerGroup _mixerGroup;

        public void Play(Transform transform)
        {
            AudioManager.Instance.PlayClipAtPoint(_clip, transform.position, _mixerGroup);
        }
    }
}