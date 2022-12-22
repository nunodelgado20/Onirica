using DG.Tweening;
using UnityEngine;

namespace Utils
{
    public class DOTweenStartup : MonoBehaviour
    {
        void Start()
        {
            DOTween.SetTweensCapacity(500,50);
        }
    }
}
