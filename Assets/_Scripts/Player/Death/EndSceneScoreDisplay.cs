using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndSceneScoreDisplay : MonoBehaviour
{
    [SerializeField] ScoreObject scoreObject;
    [SerializeField] TextMeshProUGUI scoreText;

    private void FixedUpdate() {
        scoreText.SetText("Final Score: {0}", (int)scoreObject.score);
    }
}
