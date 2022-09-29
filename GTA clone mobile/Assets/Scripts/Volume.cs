using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    [SerializeField] AudioMixer masterMixer;
    [SerializeField] Slider musicVolumeSlider;
    [SerializeField] float musicLvl;
    [SerializeField] Slider VFXVolumeSlider;
    [SerializeField] float VFXLvl;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MainMusic"))
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MainMusic");
            masterMixer.SetFloat("Main", PlayerPrefs.GetFloat("MainMusic"));
        }
        else
        {
            musicVolumeSlider.value = 0;
            masterMixer.SetFloat("Main", 0);
        }

        if (PlayerPrefs.HasKey("Effects"))
        {
            VFXVolumeSlider.value = PlayerPrefs.GetFloat("Effects");
            masterMixer.SetFloat("VFX", PlayerPrefs.GetFloat("Effects"));
        }
        else
        {
            VFXVolumeSlider.value = 0;
            masterMixer.SetFloat("VFX", 0);
        }
    }

    public void SetMusicVolume()
    {
        musicLvl = musicVolumeSlider.value;
        masterMixer.SetFloat("Main", musicLvl);
        PlayerPrefs.SetFloat("MainMusic", musicLvl);
    }

    public void SetVFXVolume()
    {
        VFXLvl = VFXVolumeSlider.value;
        masterMixer.SetFloat("VFX", VFXLvl);
        PlayerPrefs.SetFloat("Effects", VFXLvl);
    }

}