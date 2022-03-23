using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private AudioSource audio;

    public AudioClip[] sounds;
    private void Start()
    {
        audio = this.GetComponent<AudioSource>();
    }

    public void PlaySound()
    {
        audio.clip = sounds[Random.Range(0, sounds.Length - 1)];
        audio.Play();
    }
    public void MuteSound()
    {
        if (audio.mute)
        {
            audio.mute = false;
        }
        else
        {
            audio.mute = true;
        }     
    }
}
