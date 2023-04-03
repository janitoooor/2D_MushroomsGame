using System.Collections;
using UnityEngine;

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

    public static void StartRoutine(IEnumerator enumerator)
    {
        s_instance.StartCoroutine(enumerator);
    }

    public static void StopRoutine(IEnumerator routine)
    {
        s_instance.StopCoroutine(routine);
    }
}
