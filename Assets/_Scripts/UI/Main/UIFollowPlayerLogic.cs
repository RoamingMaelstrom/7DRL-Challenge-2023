using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFollowPlayerLogic : MonoBehaviour
{
    [SerializeField] RectTransform uiFollowingPlayer;
    [SerializeField] Vector2 playerPositionOffset;
    [SerializeField] GameObject playerBody;
    [SerializeField] [Range(0f, 1f)] float followLerpValue;

    Vector2 playerScreenPos;

    private void FixedUpdate() {
        playerScreenPos = Camera.main.WorldToScreenPoint(playerBody.transform.position);
        uiFollowingPlayer.transform.position = Vector3.Lerp(uiFollowingPlayer.transform.position, playerScreenPos + playerPositionOffset, 0.25f);
    }
}
