using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SfxClipInfo", menuName = "SfxClipInfoObject", order = 10)]
public class SfxClipInfo : ScriptableObject
{
    public string clipName;
    public AudioClip clip;
    [Range(0f, 1f)] public float volume = 1f;
    public float cooldownOnPlay;
    public float currentCooldown {get; private set;} = 0f;


    public SfxClipInfo(string _name, AudioClip _clip, float _volume = 0.5f, float _cooldownOnPlay = 0.25f)
    {
        clipName = _name;
        clip = _clip;
        volume = _volume;
        cooldownOnPlay = _cooldownOnPlay;
    }

    public void DecrementCurrentCooldown(float value) => currentCooldown -= value;
    public void SetCurrentCooldown(float value) => currentCooldown = cooldownOnPlay;
}
