using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceLines : MonoBehaviour
{
    public AudioSource _audioSource;
    bool hasPlayed = false;
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasPlayed)
        {
            hasPlayed = false;
        } else
        {
            _audioSource.Play();
            hasPlayed = true;
        }
    }
}
