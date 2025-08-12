using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioClip music;

    public AudioSource musicSource;

    public Slider volumeSlider;

    private static AudioManager _instance;
    public static AudioManager instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        if (volumeSlider)
        {
            volumeSlider.value = musicSource.volume;
        }        

        musicSource.clip = music;
        musicSource.Play();
        musicSource.loop = true;
    }

    public void UpdateVolume()
    {
        if (volumeSlider)
        {
            musicSource.volume = volumeSlider.value;
        }        
    }
}
