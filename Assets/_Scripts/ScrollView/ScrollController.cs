using System.Collections;
using UnityEngine;

class ScrollController : MonoBehaviour
{
    [SerializeField] private RectTransform _contentView;

    private readonly int _yPos = -2000;

    private void OnEnable()
    {
        StartCoroutine(WaitTwoFrames());
    }

    private IEnumerator WaitTwoFrames()
    {
        yield return null;
        yield return null;
        yield return null;
        _contentView.anchoredPosition = new Vector2(_contentView.anchoredPosition.x, _yPos);
    }
}
