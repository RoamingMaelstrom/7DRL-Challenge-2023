using System.Collections.Generic;
using UnityEngine;
using SOEvents;

/*
Todo:
    - Implement Priority in SfxClipInfo
    - Add functionality that stores how many clips are playing.
    - Add limit to number of clips that can play at the same time
    - Choose which clips get to play when many clips are playing based on priority.
*/

public class SfxMain : MonoBehaviour
{
    [SerializeField] StringSOEvent playSfxEvent;
    [SerializeField] ClipInfoContainer clipContainer;
    private void Awake() => playSfxEvent.AddListener(EventHandler);

    // Accepts PlayOrderInstruction string as input.
    public void PlayClip(string clipInstruction) => playSfxEvent.Invoke(clipInstruction);
    
    // Does not accept PlayOrderInstruction string as input.
    public void PlayRandomClip(List<string> clipNames) => PlayClip(clipNames[Random.Range(0, clipNames.Count)]);
    // Does not accept PlayOrderInstruction string as input.
    public void PlayRandomClip(string[] clipNames) => PlayClip(clipNames[Random.Range(0, clipNames.Length)]);


    private void EventHandler(string clipOrder)
    {
        string[] rawClipParameters = clipOrder.Trim().Split(" ");
        SfxClipInfo clipInfo = clipContainer.GetSfxClipInfoObject(rawClipParameters[0]);

        if (clipInfo is null) 
        {
            Debug.Log(string.Format("Could not find clipInfo with name \"{0}\"", rawClipParameters[0]));
            return;
        }

        switch (rawClipParameters.Length)
        {
            case > 1: 
                RunPlayClip(clipInfo, new PlayOrderInstructions(rawClipParameters)); break;
            default: 
                RunPlayClip(clipInfo, null); break;
        }
    }


    private void RunPlayClip(SfxClipInfo clipInfo, PlayOrderInstructions playOrderInstructions)
    {
        if (clipInfo.currentCooldown > 0) return;
        ClipPlayer.PlayClip(clipInfo, playOrderInstructions);
        clipInfo.SetCurrentCooldown(clipInfo.cooldownOnPlay);
    }
}

[System.Serializable]
public class ClipPlayer
{
    public static void PlayClip(SfxClipInfo clipInfo) => AudioSource.PlayClipAtPoint(clipInfo.clip,
     Camera.main.transform.position, clipInfo.volume);   

    public static void PlayClip(SfxClipInfo clipInfo, PlayOrderInstructions playOrderInstruction) 
    {
        if (playOrderInstruction is null) 
        {
            PlayClip(clipInfo);
            return;
        }

        PlayClip(clipInfo, playOrderInstruction.relativeVolume, playOrderInstruction.position,
         playOrderInstruction.positionIsRelativeToCamera);
    }

    public static void PlayClip(SfxClipInfo clipInfo, float relativeVolume, Vector3 position, bool positionIsRelativeToCamera)
    {
        position = (positionIsRelativeToCamera) ? 
         Camera.main.transform.position + position : position;
        position.z = Camera.main.transform.position.z;

        AudioSource.PlayClipAtPoint(clipInfo.clip, position, clipInfo.volume * relativeVolume);
    }
}



[System.Serializable]
public class PlayOrderInstructions
{
    public string clipName;
    public float relativeVolume;
    public Vector3 position;
    public bool positionIsRelativeToCamera;

    public PlayOrderInstructions(string _clipName, float _relativeVolume, Vector3 _position, bool _positionIsRelativeToCamera)
    {
        clipName = _clipName;
        relativeVolume = _relativeVolume;
        position = _position;
        positionIsRelativeToCamera = _positionIsRelativeToCamera;
    }

    public PlayOrderInstructions(string _clipName, float _relativeVolume, Vector2 _position, bool _positionIsRelativeToCamera)
    {
        clipName = _clipName;
        relativeVolume = _relativeVolume;
        position = _position;
        positionIsRelativeToCamera = _positionIsRelativeToCamera;
    }

    public PlayOrderInstructions(string[] rawOrderInstructions)
    {
        clipName = rawOrderInstructions[0];
        relativeVolume = ParseRelativeVolume(rawOrderInstructions); 
        position = ParsePosition(rawOrderInstructions);
        positionIsRelativeToCamera = ParsePositionIsRelativeToCamera(rawOrderInstructions);
    }

    private bool ParsePositionIsRelativeToCamera(string[] rawOrderInstructions)
    {
        for (int i = 1; i < rawOrderInstructions.Length; i++)
        {
            if (rawOrderInstructions[i].Contains("f") || rawOrderInstructions[i].Contains("F")) return false;
        }
        
        return true;
    }

    private Vector3 ParsePosition(string[] rawOrderInstructions)
    {
        string rawVector = "";
        for (int i = 1; i < rawOrderInstructions.Length; i++)
        {
            if (rawOrderInstructions[i].Contains("v")) 
            {
                rawVector = rawOrderInstructions[i];
                break;
            }
        }

        Vector3 output = new Vector3(0, 0, -0);

        if (rawVector == "") return output;

        string[] vectorSegments = rawVector.Split(",");
        vectorSegments[0] = vectorSegments[0].Replace("v", "");

        output.x = float.Parse(vectorSegments[0]);
        if (vectorSegments.Length == 2) output.y = float.Parse(vectorSegments[1]);
        if (vectorSegments.Length >= 3) output.z = float.Parse(vectorSegments[2]);

        return output;
    }

    private float ParseRelativeVolume(string[] rawOrderInstructions)
    {
        for (int i = 1; i < rawOrderInstructions.Length; i++)
        {
            if (rawOrderInstructions[i].Contains(".") && !rawOrderInstructions[i].Contains("v")) 
                return Mathf.Clamp(float.Parse(rawOrderInstructions[i]), 0f, 1f);
        }

        return 1f;
    }
}
