using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager AM;
    public Sound[] sounds;
    public bool mainMenu = true;

    [HideInInspector]
    bool alreadyInBattle = false;

    private void Start()
    {

    }

    void Awake()
    {
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        DontDestroyOnLoad(this);
        if (AM == null)
        {
            AM = this;
        }
        else
        {
            if (AM != this)
            {
                Destroy(this);
            }
        }
    }


    public void Play(string name)
    {
        Play(name, 1);
    }

    public void Play(string name, float volume)
    {
        Sound s = Array.Find(sounds, sound => name == sound.name);

        s.source.volume = volume * s.volume;
        if (s.isMusic)
            s.source.volume *= (float)Messenger.MS.GetMusicVolume();
        else
            s.source.volume *= (float)Messenger.MS.GetSoundVolume();

        if (s.source.loop == false)
        {
            s.source.PlayOneShot(s.source.clip);
        }
        else
        {
            s.source.Play();
        }

        //mainMenu = (name == "MainMenuTheme" || (name == "ShotKill" && mainMenu == true)) ? true : false;

    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => name == sound.name);

        s.source.Stop();

    }

    public void StopAll()
    {

        AudioSource[] audioallAudioSources = FindObjectsOfType<AudioSource>() as AudioSource[];
        foreach (AudioSource audioS in audioallAudioSources)
        {
            audioS.Stop();
        }

    }

    public void ChangeVolume(bool isMusic, float volume)
    {
        foreach (Sound s in sounds)
        {
            if (s.isMusic == isMusic)
            {
                s.source.volume = s.volume * volume;
            }

        }

        //mainMenu = (name == "MainMenuTheme" || (name == "ShotKill" && mainMenu == true)) ? true : false;

    }

    public bool GetIsPlaying(string name)
    {
        Sound s = Array.Find(sounds, sound => name == sound.name);
        return s.source.isPlaying;
    }

    public void CardSound()
    {
        int r = UnityEngine.Random.Range(0, 5);
        string[] soundList = { "Card 1", "Card 2", "Card 3", "Card 4", "Card 5" };

        Play(soundList[r]);
    }
}
