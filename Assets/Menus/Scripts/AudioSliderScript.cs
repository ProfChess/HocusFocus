using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSliderScript : MonoBehaviour
{
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    private void Start()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.25f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 0.25f);
        SetMusicVolume();
        SetSFXVolume();
    }

    public void SetMusicVolume()
    {
        float volume = musicSlider.value;
        Mixer.SetFloat("MusicVol", Mathf.Log10(volume) * 20);
    }

    public void SetSFXVolume()
    {
        float volume = sfxSlider.value;
        Mixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
    }

    public float getMusicVolume()
    {
        return musicSlider.value;
    }
    public float getSFXVolume()
    {
        return sfxSlider.value;
    }
}
