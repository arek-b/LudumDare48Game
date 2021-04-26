using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundtrackPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] tracks;
    private AudioSource audioSource;
    private System.Random random;
    int startingTrack;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (tracks.Length == 0)
            return;

        audioSource = GetComponent<AudioSource>();
        random = new System.Random();
        startingTrack = random.Next(0, tracks.Length - 1);
        audioSource.clip = tracks[startingTrack];
        audioSource.Play();
        StartCoroutine(PlayNextTrack(audioSource.clip.length, startingTrack));
    }

    private IEnumerator PlayNextTrack(float waitTime, int currentTrack)
    {
        yield return new WaitForSeconds(waitTime);
        int newTrack;
        do
        {
            newTrack = random.Next(0, tracks.Length - 1);
        }
        while (newTrack == currentTrack);
        audioSource.clip = tracks[newTrack];
        audioSource.Play();
        StartCoroutine(PlayNextTrack(audioSource.clip.length, newTrack));
    }
}
