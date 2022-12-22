using Audio;
using UnityEngine;
using UnityEngine.Audio;

namespace Teleports
{
    public class Teleport : MonoBehaviour
    {
        [SerializeField] private AudioClip _teleportAudio;
        [SerializeField] private AudioMixerGroup _mixerGroup;
        [SerializeField] private Transform _destination;
   

        private void OnTriggerEnter(Collider other)
        {
            if (_destination == null) return;
            if (other.TryGetComponent(out CharacterController protagonist))
            {
                AudioManager.Instance.PlayClipAtPoint(_teleportAudio, transform.position, _mixerGroup);
                protagonist.enabled = false;
                protagonist.transform.position = _destination.position;
                protagonist.enabled = true;
            }
        }
    }
}
