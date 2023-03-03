using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ShipDifficultyDisplay : MonoBehaviour
{
    [SerializeField] Color beginnerColour;
    [SerializeField] Color normalColour;
    [SerializeField] Color hardColour;

    [SerializeField] int difficulty = 0;

    [SerializeField] TextMeshProUGUI difficultyText;


    private void Start() 
    {
        switch (difficulty)
        {
            case 1: 
                ConfigureDifficultyText("Normal", normalColour);
                break;
            case 2: 
                ConfigureDifficultyText("Hard", hardColour);
                break;    
            default: 
                ConfigureDifficultyText("Easy", beginnerColour);
                break;
        }
    }

    void ConfigureDifficultyText(string text, Color textColour)
    {
        difficultyText.color = textColour;
        difficultyText.SetText(text);
    }
}
