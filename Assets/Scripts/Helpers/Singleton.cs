using UnityEngine;

namespace Helpers
{
    public class Singleton <T> : MonoBehaviour where T : MonoBehaviour
    {
        //From : https://www.youtube.com/watch?v=ptkxRn0HCJc&t=369s&ab_channel=JasonWeimann

        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    
                    if(_instance == null)
                        _instance = new GameObject("Instance of " + typeof(T)).AddComponent<T>();
                }

                return _instance;
            }
        }
        
        //TODO to avoid this, the player must also be presistant. Dont destrou on load.
//if i activate the code below, inputcontroller will stop working because it is assigned to unity events and it will be destroyed and a new instance will be created.
        /*private void Awake()
        {
            if(_instance != null)
                Destroy(gameObject);
        }*/
    }
}