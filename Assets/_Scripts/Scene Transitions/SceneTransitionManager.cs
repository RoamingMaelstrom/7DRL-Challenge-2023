using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Place GameObject containing this Script in Start Scene.
public class SceneTransitionManager : MonoBehaviour
{

    public List<SceneTransitionInfo> sceneTransitionObjects = new List<SceneTransitionInfo>();

    static SceneTransitionManager currentSceneTransitionManager;

    private void Start() 
    {
        if (currentSceneTransitionManager)
        {
            Destroy(this);
            return;
        }
        currentSceneTransitionManager = this;
    }

    public void LoadScene(string sceneName)
    {
        foreach (var sceneTransitionInfo in sceneTransitionObjects)
        {
            if (sceneTransitionInfo.sceneName == sceneName) 
            {
                StartCoroutine(SwitchScenes(sceneTransitionInfo));
                return;
            }
        }

        Debug.Log(string.Format("Could not find SceneTransitionInfo Object for Scene with name \"{0}\"", sceneName));

    }

    public IEnumerator SwitchScenes(SceneTransitionInfo transitionInfo)
    {
        if (transitionInfo.sceneExitAnimator != null && transitionInfo.exitAnimationLength > 0)
        {
           transitionInfo.sceneExitAnimator.SetTrigger(transitionInfo.exitTriggerName); 
           yield return new WaitForSecondsRealtime(transitionInfo.exitAnimationLength);
        } 
        SceneManager.LoadScene(transitionInfo.sceneName);
        if (transitionInfo.sceneEnterAnimator != null && transitionInfo.enterAnimationLength > 0)
        {
           transitionInfo.sceneEnterAnimator.SetTrigger(transitionInfo.enterTriggerName); 
           yield return new WaitForSecondsRealtime(transitionInfo.enterAnimationLength);
        } 
    }

}


// Todo: Replace this with ScriptableObject with Custom Editor window.
[System.Serializable]
public class SceneTransitionInfo
{   
    public string sceneName;
    
    public Animator sceneExitAnimator;
    public string exitTriggerName;
    public float exitAnimationLength;

    public Animator sceneEnterAnimator;
    public string enterTriggerName;
    public float enterAnimationLength;

}
