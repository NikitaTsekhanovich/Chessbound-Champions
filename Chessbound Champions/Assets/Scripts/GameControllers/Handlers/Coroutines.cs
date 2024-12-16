using System.Collections;
using UnityEngine;

namespace GameControllers.Handlers
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines _instance;

        public void Initialize()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(_instance);
        }

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return _instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(IEnumerator enumerator)
        {
            _instance.StartCoroutine(enumerator);
        }
    }
}
