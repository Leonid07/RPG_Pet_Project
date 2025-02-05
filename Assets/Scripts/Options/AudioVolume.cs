using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    [SerializeField] AudioMixer _mixerGroup = null;
    [SerializeField] string _exposedParameterSound = "SoundVolume";
    [SerializeField] string _exposedParameterMusic = "MusicVolume";
    public Slider SliderSound;
    public Slider SliderMusic;

    private void Start()
    {
        float defaultValue;
        _mixerGroup.GetFloat(_exposedParameterSound, out defaultValue);
        _mixerGroup.GetFloat(_exposedParameterMusic, out defaultValue);

        float volumeMusic = PlayerPrefs.GetFloat(_exposedParameterMusic, Mathf.Pow(10, defaultValue / 20));
        float volumeSound = PlayerPrefs.GetFloat(_exposedParameterSound, Mathf.Pow(10, defaultValue / 20));
        SliderSound.value = volumeSound;
        SliderMusic.value = volumeMusic;
        SetVolumeMusic(volumeMusic);
        SetVolumeSound(volumeSound);
        SliderSound.onValueChanged.AddListener(SetVolumeSound);
        SliderMusic.onValueChanged.AddListener(SetVolumeMusic);
    }
    public void SetVolumeMusic(float volume)
    {
        _mixerGroup.SetFloat(_exposedParameterMusic, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(_exposedParameterMusic, volume);
    }
    public void SetVolumeSound(float volume)
    {
        _mixerGroup.SetFloat(_exposedParameterSound, Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat(_exposedParameterSound, volume);
    }
}