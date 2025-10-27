using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager I;

    [Header("Music")]
    public AudioSource music_source;
    public AudioClip music_sound;

    [Header("Sound")]
    public AudioSource[] sound_sources;
    private Queue<AudioSource> queue_sources;

    [Header("SOUNDS")]
    public AudioClip Win;
    public AudioClip Lose;
    public AudioClip Click;
    public AudioClip ClickButton;

    private void Awake()
    {
        I = this;
    }

    private void Start()
    {
        queue_sources = new Queue<AudioSource>(sound_sources);
        UpdateVolume();
        PlayMusic();
    }

    public void UpdateVolume()
    {
        UpdateVolumeSounds();
        UpdateVolumeMusic();
    }

    public void UpdateVolumeSounds()
    {
        foreach (var sound in sound_sources)
        {
            //sound.volume = PrefData.Sound;
        }
    }

    public void UpdateVolumeMusic()
    {
        //music_source.volume = PrefData.Music;
    }


    public void PlaySoundByType(TypeSound type)
    {
        switch (type)
        {
            case TypeSound.WIN:
                PlaySound(Win);
                break;
            case TypeSound.LOSE:
                PlaySound(Lose);
                break;
            case TypeSound.CLICK:
                PlaySound(Click);
                break;
            case TypeSound.CLICKBUTTON:
                PlaySound(ClickButton);
                break;
        }
    }



    public void PlaySound(AudioClip clip)
    {
        var source = queue_sources.Dequeue();
        if (source == null)
            return;
        source.PlayOneShot(clip);
        queue_sources.Enqueue(source);
    }

    public void PlayMusic(float volume = 0.7f)
    {
        music_source.clip = music_sound;
        music_source.volume = volume;   
        music_source.Play();
    }

    public void StopMusic()
    {
        music_source.Stop();
    }
}
public enum TypeSound
{
    MUSIC,
    WIN,
    LOSE,
    CLICK,
    CLICKBUTTON,
}
