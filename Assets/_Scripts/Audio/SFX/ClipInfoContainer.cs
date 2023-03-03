using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClipInfoContainer : MonoBehaviour
{
    [SerializeField] List<SfxClipInfo> clipInfoObjects = new List<SfxClipInfo>();
    Dictionary<string, SfxClipInfo> clipInfosLookupTable = new Dictionary<string, SfxClipInfo>();

    private void Start() 
    {
        foreach (SfxClipInfo clipInfo in clipInfoObjects)
        {
            clipInfosLookupTable.Add(clipInfo.clipName, clipInfo);
        }
    }

    private void FixedUpdate() => UpdateClipInfosCooldown(clipInfoObjects);

    private void UpdateClipInfosCooldown(List<SfxClipInfo> clipInfos)
    {
        foreach (var clipInfo in clipInfos)
        {
            clipInfo.DecrementCurrentCooldown(Time.fixedDeltaTime);
        }
    }

    public void AddClipInfo(SfxClipInfo newClipInfo)
    {           
        if (clipInfosLookupTable.TryGetValue(newClipInfo.clipName, out SfxClipInfo currentInfo)) 
        {
            Debug.Log("ClipInfo Object with same name found in container. Removing old clip from container.");
            clipInfoObjects.Remove(currentInfo);
            clipInfosLookupTable.Remove(newClipInfo.clipName);
        }
        clipInfoObjects.Add(newClipInfo);
        clipInfosLookupTable.Add(newClipInfo.clipName, newClipInfo);
    }

    public SfxClipInfo GetSfxClipInfoObject(string clipName)
    {
        if (clipInfosLookupTable.TryGetValue(clipName, out SfxClipInfo clipInfo)) return clipInfo;
        return null;
    }


}
