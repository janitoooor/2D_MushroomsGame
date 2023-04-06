using UnityEngine;

public class BoostersLayerTutorial : MonoBehaviour
{
    [SerializeField] private TutorialPanel _tutorialPanel;

    private void OnEnable()
    {
        _tutorialPanel.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        _tutorialPanel.Show();
    }
}