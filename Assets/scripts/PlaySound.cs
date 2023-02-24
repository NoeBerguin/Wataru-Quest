using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip soundEffectInvocation;
    public AudioClip soundEffectExplosion;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // jouer le son lors du démarrage
        audioSource.PlayOneShot(soundEffectInvocation);
    }


    void ExplodeSoundEvent()
    {
        audioSource.PlayOneShot(soundEffectExplosion);
    }
}
