using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SOEvents;

public class SetupEndPanel : MonoBehaviour
{
    [SerializeField] GameObjectFloatSOEvent playerDeathEvent;
    [SerializeField] GameObject endPanel;
    public float delay = 3f;

    private void Awake() 
    {
        playerDeathEvent.AddListener(MakePanelVisibleAfterDelay);
    }

    public void MakePanelVisibleAfterDelay(GameObject playerParent, float arg1)
    {
        StartCoroutine(MakePanelVisible());
    }

    IEnumerator MakePanelVisible()
    {
        yield return new WaitForSecondsRealtime(delay);
        endPanel.SetActive(true);
        yield return null;
    }
}
