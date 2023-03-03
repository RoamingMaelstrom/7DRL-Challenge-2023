using System.Collections;
using UnityEngine;
using SOEvents;
using System;

// Todo: Might have broken this in a fluey haze. 
// Good class/System to refactor.
public class MainMusicLogic : MonoBehaviour
{
    [SerializeField] StringSOEvent musicPlayerEventHandler;
    // AudioSources that play the music. Usually, only one source is playing at a time. 
    // The exception is when there is a transition between the two sources.
    [SerializeField] AudioSource musicSource1;
    [SerializeField] AudioSource musicSource2;

    [SerializeField] TrackContainer trackContainer;

    public float fadeLength = 1f;

    // Specifies whether Clip Swapping takes place when a Clip nears it completion.
    public bool AUTO_PLAY_NEXT_TRACK = true;

    // Volume when music is playing normally in the game.
    [SerializeField] [Range(0.0f, 1.0f)] public float normalVolume = 0.6f;
    // Volume when the game is paused.
    [SerializeField] [Range(0.0f, 1.0f)] public float pausedVolume = 0.2f;

    // AudioSource that is currently playing.
    AudioSource playingSource;

    VolumeTransitioner volumeTransitioner;

    private void Awake() 
    {
        volumeTransitioner = new VolumeTransitioner(normalVolume, pausedVolume, fadeLength);
        musicPlayerEventHandler.AddListener(EventHandler);
    }

    public void InvokeEvent(string eventName)
    {
        musicPlayerEventHandler.Invoke(eventName);
    }


    // Todo: Could extend to add optional args, rather than relying on preset values.
    private void EventHandler(string eventName)
    {
        switch (eventName)
        {
            case "fadeOut": RunEnterMutedState(); break;
            case "fadeIn": RunExitMutedState(); break;
            case "nextTrack": RunNextTrack(); break;
            default: Debug.Log(string.Format("Unrecognised Event Name ({0})", eventName)); break;
        }
    }

    private void RunNextTrack()
    {
        AudioSourceSwapper.SwapSourceInPlace(ref playingSource, ref musicSource1, ref musicSource2, true);
        playingSource.clip = trackContainer.GetNextTrack();
        if (!playingSource.isPlaying) playingSource.Play();
    }

    private void Start() 
    {
        SetupMusicSystem();
        StartCoroutine(RunMusicSystem());
    }

    // Generates a random order of tracks, sets music players to normalVolume, sets playingSource to musicSource1
    void SetupMusicSystem()
    {
        SetupPlayersVolume();
        playingSource = musicSource1;
        playingSource.clip = trackContainer.GetNextTrack();
    }

    IEnumerator RunMusicSystem()
    {
        if (!playingSource.isPlaying) playingSource.Play();
        if (playingSource.clip is null) 
        {
            AUTO_PLAY_NEXT_TRACK = false;
            Debug.Log("No AudioClips found in TrackContainer. Music System Cannot run.");
        }
        while (AUTO_PLAY_NEXT_TRACK)
        {
            while (playingSource.clip.length - playingSource.time > 5) yield return new WaitForSecondsRealtime(1);
            EventHandler("nextTrack");
        }
        yield return null;
    }

    // Todo: Implement playerprefs loading/saving.
    // Called in SetupMusicSystem to make sure the music sources start at normalVolume.
    void SetupPlayersVolume()
    {
        musicSource1.volume = normalVolume;
        musicSource2.volume = normalVolume;
    }

    void RunEnterMutedState() => volumeTransitioner.EnterMutedStateOnEvent(this, playingSource);

    void RunExitMutedState() => volumeTransitioner.ExitMutedStateOnEvent(this, playingSource);

}



public class VolumeTransitioner
{
    public VolumeTransitioner(float normalVolume = 0.5f, float quietVolume = 0.15f, float transitionLength = 1f)
    {
        this.normalVolume = normalVolume;
        this.quietVolume = quietVolume;
        this.transitionLength = transitionLength;
    }

    public float normalVolume;
    public float quietVolume;
    public float transitionLength;

    Coroutine currentVolumeTransitionRoutine;

    // Stops currentVolumeTransitionRoutine running if it exists, then Starts EnterMutedState. Ensures compatability with enterPauseEvent.
    public void EnterMutedStateOnEvent(MonoBehaviour monoBehaviourClass, AudioSource audioSource)
    {
        if (currentVolumeTransitionRoutine != null) monoBehaviourClass.StopCoroutine(currentVolumeTransitionRoutine);
        currentVolumeTransitionRoutine = monoBehaviourClass.StartCoroutine(EnterMutedState(audioSource));
    }

    // Stops currentVolumeTransitionRoutine running if it exists, then Starts ExitMutedState. Ensures compatability with exitPauseEvent.
    public void ExitMutedStateOnEvent(MonoBehaviour monoBehaviourClass, AudioSource audioSource)
    {
        if (currentVolumeTransitionRoutine != null) monoBehaviourClass.StopCoroutine(currentVolumeTransitionRoutine);
        currentVolumeTransitionRoutine = monoBehaviourClass.StartCoroutine(ExitMutedState(audioSource));
    }

    // Gradually changes the volume of the playingSource AudioSource to quietVolume based on transitionLength.
    IEnumerator EnterMutedState(AudioSource playingSource)
    {
        float rateOfChange =  (playingSource.volume - quietVolume) * Time.fixedDeltaTime / transitionLength;

        while (playingSource.volume > quietVolume)
        {
            playingSource.volume -= rateOfChange;
            yield return new WaitForSecondsRealtime(Time.fixedDeltaTime); 
        }
        yield return null;
    }

    IEnumerator ExitMutedState(AudioSource playingSource)
    {
        float rateOfChange =  (playingSource.volume - normalVolume) * Time.fixedDeltaTime / transitionLength * 1.01f;

        while (playingSource.volume < normalVolume)
        {
            playingSource.volume -= rateOfChange;
            yield return new WaitForFixedUpdate(); 
        }
        yield return null;
    }
}



public class AudioSourceSwapper
{
    public static void SwapSourceInPlace(ref AudioSource playingSource, ref AudioSource audioSource1, ref AudioSource audioSource2, bool copyVolume = true)
    {
        if (copyVolume) 
        {
            audioSource1.volume = playingSource.volume;
            audioSource2.volume = playingSource.volume;
        }
        if (playingSource.GetInstanceID() == audioSource1.GetInstanceID()) playingSource = audioSource2;
        else playingSource = audioSource1;

    }
}
