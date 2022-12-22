using UnityEngine;

namespace Door
{
    public class FinalDoor : MonoBehaviour
    {
        [SerializeField] private GameObject _closedDoor;
        [SerializeField] private GameObject _openDoor;

        public void OpenDoor()
        {
            _closedDoor.SetActive(false);
            _openDoor.SetActive(true);
        }
    }
}

