using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToMainMenuLogic : MonoBehaviour
{
    public void ToMainMenuScene()
    {
        SceneTransitionManager transitionManager = FindObjectOfType<SceneTransitionManager>();
        transitionManager.LoadScene("Start Scene");
    }
}
