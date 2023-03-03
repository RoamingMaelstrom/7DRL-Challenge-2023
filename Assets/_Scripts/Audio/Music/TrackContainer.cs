using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TrackContainer : MonoBehaviour
{
    // Tracks that will be selected
    public List<AudioClip> tracks = new List<AudioClip>();
    // A list containing indexes of tracks.
    public List<int> trackPath = new List<int>();

    // Current trackPath index.
    public int currentTrackPathIndex {get; private set;} = 0;

    public AudioClip GetNextTrack()
    {
        if (tracks.Count == 0) return null;

        // Returns clip if currentTrackPathIndex is a valid index in trackPath.
        currentTrackPathIndex ++;
        if (currentTrackPathIndex < trackPath.Count) return tracks[trackPath[currentTrackPathIndex]];

        // Runs when reach end of trackPath or trackPath is not populated, but tracks is (e.g. first time GetNextTrack is called).
        currentTrackPathIndex = 0;
        
        if (trackPath.Count == 0) GenerateNextTrackPath();
        else GenerateNextTrackPath(trackPath[trackPath.Count - 1]);

        return tracks[trackPath[0]];
    }

    void GenerateNextTrackPath(int indexToBanFromStart = - 1)
    {
        trackPath = GenerateTrackPath();

        if (trackPath.Count <= 1) return;
        if (indexToBanFromStart < 0) return;

        if (trackPath[0] == indexToBanFromStart)
        {
            SwapFirstIndexWithRandomIndex(trackPath);
        }
    }

    List<int> GenerateTrackPath()
    {
        // Indexes remaining that can be selected as the next trackPath index.
        List<int> trackIndexesRemaining = new List<int>();
        for (int i = 0; i < tracks.Count; i++)
        {
            trackIndexesRemaining.Add(i);
        }

        List<int> newTrackPath = new List<int>();

        for (int i = 0; i < tracks.Count; i++)
        {
            int randomTrackIndex = Random.Range(0, trackIndexesRemaining.Count);
            newTrackPath.Add(trackIndexesRemaining[randomTrackIndex]);
            trackIndexesRemaining.RemoveAt(randomTrackIndex);
        }
        return newTrackPath;
    }

    void SwapFirstIndexWithRandomIndex(List<int> items)
    {
        int newRandomIndex = Random.Range(1, items.Count);
        int tempValue = items[newRandomIndex];
        items[newRandomIndex] = items[0];
        items[0] = tempValue;
    }

}