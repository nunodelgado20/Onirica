using UI;
using UnityEngine;

public class TriggerVictory : MonoBehaviour
{
    private LoadScene _loadScene;

    private void Awake()
    {
        _loadScene = GetComponent<LoadScene>();
    }

    private void OnTriggerEnter(Collider other)
    {
        _loadScene.Load();
    }
}
