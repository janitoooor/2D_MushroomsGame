using System.Collections;
using UnityEngine;

namespace Assets
{
    public sealed class Coroutines : MonoBehaviour
    {
        private static Coroutines s_instance
        {
            get
            {
                if (m_instance == null)
                {
                    var go = new GameObject("[Coroutines]");
                    m_instance = go.AddComponent<Coroutines>();
                    DontDestroyOnLoad(go);
                }
                return m_instance;
            }
        }

        private static Coroutines m_instance;

        public static Coroutine StartRoutine(IEnumerator enumerator)
        {
            return s_instance.StartCoroutine(enumerator);
        }

        public static void StopRoutine(Coroutine routine)
        {
            s_instance.StopCoroutine(routine);
        }
    }
}
