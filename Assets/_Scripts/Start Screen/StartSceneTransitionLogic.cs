using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneTransitionLogic : MonoBehaviour
{
    [SerializeField] GameObject startPanel;
    [SerializeField] GameObject howToPlayPanel;
    [SerializeField] GameObject howToPlayObjects;
    SceneTransitionManager sceneTransitionManager;

    private void Start() 
    {
        sceneTransitionManager = FindObjectOfType<SceneTransitionManager>();
    }

    public void LoadGameScene()
    {
        sceneTransitionManager.LoadScene("Game Scene");
    }

    public void ToStartPanel()
    {
        startPanel.SetActive(true);
        howToPlayPanel.SetActive(false);
        howToPlayObjects.SetActive(false);
    }

    public void ToHowToPlayPanelAndObjects()
    {
        startPanel.SetActive(false);
        howToPlayPanel.SetActive(true);
        howToPlayObjects.SetActive(true);
    }
}
