using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Messenger : MonoBehaviour
{
    public static Messenger MS;

    [HideInInspector]
    public bool resetting = false;

    private float? masterVolume = 1;
    private float? musicVolume = 1;
    private float? soundVolume = 1;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (MS == null)
        {
            MS = this;
        }
        else
        {
            if (MS != this)
            {
                Destroy(this);
            }
        }
    }

    public float? GetMasterVolume()
    {
        return masterVolume;
    }

    public float? GetMusicVolume()
    {
        return musicVolume;
    }

    public float? GetSoundVolume()
    {
        return soundVolume;
    }

    public void SetMasterVolume(float masterVolume)
    {
        this.masterVolume = masterVolume;
    }

    public void SetMusicVolume(float musicVolume)
    {
        this.musicVolume = musicVolume;
    }

    public void SetSoundVolume(float soundVolume)
    {
        this.soundVolume = soundVolume;
    }
}
