using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start() {
        if(PlayerPrefs.HasKey("musicVolume")){
            LoadMusicVolume();
        }else{
            SetMusicVolume();  
        }
        if(PlayerPrefs.HasKey("masterVolume")){
            LoadMasterVolume();
        }else{
            SetMasterVolume();  
        }
        if(PlayerPrefs.HasKey("sfxVolume")){
            LoadSfxVolume();
        }else{
            SetSoundFXVolume();  
        }
        
    }
    public void SetMasterVolume(){
        float volume = masterSlider.value;
        myMixer.SetFloat("master", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }
    public void SetMusicVolume(){
        float volume = musicSlider.value;
        myMixer.SetFloat("music", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("musicVolume", volume);
    }
    public void SetSoundFXVolume(){
        float volume = sfxSlider.value;
        myMixer.SetFloat("sfx", Mathf.Log10(volume)*20);
        PlayerPrefs.SetFloat("sfxVolume", volume);
    }
    private void LoadMusicVolume(){
        musicSlider.value = PlayerPrefs.GetFloat("musicVolume");

        SetMusicVolume();
    }
    private void LoadSfxVolume(){
        sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");

        SetSoundFXVolume();
    }
    private void LoadMasterVolume(){
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");

        SetSoundFXVolume();
    }
}
