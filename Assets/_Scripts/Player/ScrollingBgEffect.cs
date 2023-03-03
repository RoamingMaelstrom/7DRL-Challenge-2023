using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBgEffect : MonoBehaviour
{
    Vector2 panelSize, startPos;
    public float parallaxStrength;
    [SerializeField] Camera mainCam;

    private void Start() {
        startPos = mainCam.transform.position;
        transform.position = startPos;
        panelSize = transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size;  
    }

    private void LateUpdate() 
    {
        // Creates the parallax effect.
        Vector2 distance = mainCam.transform.position * parallaxStrength;
        transform.position = startPos + distance;

        // Looping effect so the camera cannot leave the background.
        Vector2 distanceFromCam = mainCam.transform.position * (1 - parallaxStrength);
        if (distanceFromCam.x > startPos.x + panelSize.x / 2) startPos = new Vector2(startPos.x + panelSize.x, startPos.y);
        else if (distanceFromCam.x < startPos.x - panelSize.x / 2) startPos = new Vector2(startPos.x - panelSize.x, startPos.y);

        if (distanceFromCam.y > startPos.y + panelSize.y / 2) startPos = new Vector2(startPos.x, startPos.y + panelSize.y);
        else if (distanceFromCam.y < startPos.y - panelSize.y / 2) startPos = new Vector2(startPos.x, startPos.y - panelSize.y);

    }
}
