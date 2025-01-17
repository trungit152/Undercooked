using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SoundController : MonoBehaviour
{
    [Header("Audio Source")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;
    [Header("Audio Clip")]
    public AudioClip background_menu;
    public AudioClip background_1;
    public AudioClip background_2;
    public AudioClip cut;
    public AudioClip put_lift;
    public AudioClip fry;

    [Header("Control")]
    [SerializeField] private Image musicImg;
    [SerializeField] private Image sfxImg;
    [SerializeField] private Sprite music_on;
    [SerializeField] private Sprite music_off;
    [SerializeField] private Sprite sfx_on;
    [SerializeField] private Sprite sfx_off;

    private float oriVolume;
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "Menu" || SceneManager.GetActiveScene().name == "SelectLevel")
        {
            musicSource.clip = background_menu;
            musicSource.Play();
            musicSource.volume = 0.8f;
            oriVolume = 0.8f;
        }
        else if (SceneManager.GetActiveScene().name == "level1")
        {
            musicSource.clip = background_1;
            musicSource.Play();
            musicSource.volume = 0.6f;
            oriVolume = 0.6f;
        }
        else if (SceneManager.GetActiveScene().name == "level2")
        {
            musicSource.clip = background_2;
            musicSource.Play();
            musicSource.volume = 0.6f;
            oriVolume = 0.6f;
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
    public void PlayFrySFX()
    {
        if (SFXSource.clip != fry)
        {
            SFXSource.PlayOneShot(fry);
        }
    }
    public void StopSFX()
    {
        SFXSource.Stop();
    }
    public void MusicButton()
    {
        if(musicSource.volume != 0)
        {
            musicSource.volume = 0;
            musicImg.sprite = music_off;
        }
        else if (musicSource.volume == 0)
        {
            musicSource.volume = oriVolume;
            musicImg.sprite = music_on;
        }
    }
    public void SFXButton()
    {
        if (SFXSource.volume != 0)
        {
            SFXSource.volume = 0;
            sfxImg.sprite = sfx_off;
        }
        else if (SFXSource.volume == 0)
        {
            musicSource.volume = 1;
            sfxImg.sprite = sfx_on;
        }
    }
}
