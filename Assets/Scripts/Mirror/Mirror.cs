using Audio;
using Characters.Enemy;
using UnityEngine;
using UnityEngine.Audio;

namespace Mirror
{
    public class Mirror : MonoBehaviour
    {

        [SerializeField] private AudioClip _breakSound;
        [SerializeField] private AudioMixerGroup _mixerGroup;
        [SerializeField] private Transform _spawnTransform;
        [SerializeField] private GameObject _oldGregorPrefab;
        private MirrorBreak _mirrorBreak;
        private Collider _collider;

        private void Awake()
        {
            _mirrorBreak = GetComponent<MirrorBreak>();
            _collider = GetComponent<Collider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out CharacterController protagonist))
            {
                _mirrorBreak.Break();
                AudioManager.Instance.PlayClipAtPoint(_breakSound, transform.position, _mixerGroup);
                var instance = Instantiate(_oldGregorPrefab, _spawnTransform.position, transform.rotation).GetComponent<OldGregorController>();
                instance.SetTarget(protagonist.transform);

                _collider.enabled = false;
            }
        
        }
    }
}
