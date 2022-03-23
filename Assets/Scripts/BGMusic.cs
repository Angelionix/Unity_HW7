using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour
{
    private AudioSource audio;
    public AudioClip sound;
    private void Start()
    {
        audio = this.GetComponent<AudioSource>();
        audio.clip = sound;
    }
    public void PlaySound()
    {
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
