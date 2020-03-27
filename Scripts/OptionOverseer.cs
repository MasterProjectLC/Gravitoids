using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionOverseer : MonoBehaviour
{
    [SerializeField]
    private MenuOverseer menu;

    [SerializeField]
    private SliderOption[] sliders;

    // Start is called before the first frame update
    void Start()
    {
        foreach (SliderOption slider in sliders)
        {
            SetupSlider(slider, "Master", Messenger.MS.GetMasterVolume());
            SetupSlider(slider, "Music", Messenger.MS.GetMusicVolume());
            SetupSlider(slider, "Sound", Messenger.MS.GetSoundVolume());
        }
    }

    void SetupSlider(SliderOption slider, string name, float? value)
    {
        if (slider.GetOption() == name && value != null)
        {
            float newX = (float)(value * (slider.GetRadius() * 2)) - slider.GetRadius() + slider.GetCenter();
            slider.GetComponent<RectTransform>().position = new Vector3(newX, slider.GetComponent<RectTransform>().position.y);
        }
    }

    void GetSliderValue(SliderOption slider)
    {
        switch (slider.GetOption())
        {
            case "Master":
                Messenger.MS.SetMasterVolume(slider.GetValue());
                AudioListener.volume = (float)Messenger.MS.GetMasterVolume();
                break;

            case "Music":
                Messenger.MS.SetMusicVolume(slider.GetValue());
                AudioManager.AM.ChangeVolume(true, slider.GetValue());
                break;

            case "Sound":
                Messenger.MS.SetSoundVolume(slider.GetValue());
                AudioManager.AM.ChangeVolume(false, slider.GetValue());
                break;
        }
    }

}
