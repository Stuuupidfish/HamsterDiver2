using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip [] musicClips;

    private bool musicStop = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource.clip = musicClips[0];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if (!musicStop && !audioSource.isPlaying)
        {
            int index = Random.Range(0, musicClips.Length);
            audioSource.clip = musicClips[index];
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
        musicStop = true;
    }
}
