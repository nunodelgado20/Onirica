using UnityEngine;

namespace Door
{
    public class Key : MonoBehaviour
    {
        [SerializeField] private FinalDoor _finalDoor;
        private void OnTriggerEnter(Collider other)
        {
            //if(other.TryGetComponent(out Emotions))
            _finalDoor.OpenDoor();
            gameObject.SetActive(false);
        }
    }
}