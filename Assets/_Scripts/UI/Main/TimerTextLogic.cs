using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TimerTextLogic : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TimerLogic timerLogic;

    private void FixedUpdate() {
        timerText.SetText(string.Format("{0}:{1:00}", (int)(timerLogic.timePlayed / 60), (int)(timerLogic.timePlayed % 60)));
    }
}
