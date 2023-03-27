using UnityEngine;

namespace Assets.Scripts
{
    class CallMethodCloseObject : MonoBehaviour
    {
        public void CloseObject()
        {
            gameObject.SetActive(false);
        }
    }
}
