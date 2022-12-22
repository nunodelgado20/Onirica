using UnityEngine;

namespace Helpers
{
    public static class DebugInEditor
    {
        private static bool _debug = false;
        public static void Log(object message)
        {
#if UNITY_EDITOR
            if(_debug) Debug.Log(message); 
#endif
        }

        public static void LogWarning(object message)
        {
#if UNITY_EDITOR
            if(_debug) Debug.LogWarning(message);
#endif
        }
    }
}