using System.Collections;
using System.Collections.Generic;
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
        _tutorialPanel.gameObject.SetActive(true);
    }
}
