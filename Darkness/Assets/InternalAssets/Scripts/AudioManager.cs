using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource audioSource;
    public AudioClip[] backgroundMusic;

    public Slider musicSlider;
    public Slider sfxSlider;

    public bool start= false;
    public bool stop = false;

    private int _indexOfLastClip;
    private int _indexOfNewClip;

    private float _lastMusicVolume;
    private float _lastSfxVolume;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
            sfxSlider.value = PlayerPrefs.GetFloat("sfxVolume");
        }

        musicSlider.onValueChanged.AddListener((value) => SetMusicVolume(value));
        sfxSlider.onValueChanged.AddListener((value) => SetSfxVolume(value));

        _lastMusicVolume = musicSlider.value;
        _lastSfxVolume = sfxSlider.value;
    }

    private void SetMusicVolume(float volume)
    {
        // This is done for optimization. Thus, we save fewer times.
        // We still will not hear the difference in 0.05. :)
        // So why save once again?
        if (Mathf.Abs(_lastMusicVolume - musicSlider.value) > 0.05)
        {
            audioSource.volume = volume;
            PlayerPrefs.SetFloat("musicVolume", volume);
            PlayerPrefs.Save();
            _lastMusicVolume = musicSlider.value;
        }       
    }

    private void SetSfxVolume(float volume)
    {
        // This is done for optimization. Thus, we save fewer times.
        // We still will not hear the difference in 0.05. :)
        // So why save once again?
        if (Mathf.Abs(_lastSfxVolume - sfxSlider.value) > 0.05)
        {
            PlayerPrefs.SetFloat("sfxVolume", volume);
            PlayerPrefs.Save();
            _lastSfxVolume = sfxSlider.value;
        }
    }

    private void Update()
    {
        if(stop)
        {
            stop = false;
            StopPlayingBackGroundMusic(true);
        }

        if (start)
        {
            start = false;
            StartPlayingBackGroundMusic();
        }
    }

    public void StartPlayingBackGroundMusic()
    {
        if (backgroundMusic.Length > 1)
        {
            for (int i = 0; i < backgroundMusic.Length; i++)
            {
                if (backgroundMusic[i] == null) throw new AudioManagerException("В массиве для фоновой музыки не должно быть пустых элементов.");
            }

            StartCoroutine("StartPlayingRandomMusic");
        }
        else throw new AudioManagerException("Вы должно добавить минимум 2 мелодии для фоновой музыки.");
    }

    public void StopPlayingBackGroundMusic(bool instantly)
    {
        if (instantly) audioSource.Stop();
        StopCoroutine("StartPlayingRandomMusic");
        _indexOfLastClip = _indexOfNewClip;
    }

    private IEnumerator StartPlayingRandomMusic()
    {
        while (_indexOfLastClip == _indexOfNewClip) _indexOfNewClip = Random.Range(0, backgroundMusic.Length);

        audioSource.clip = backgroundMusic[_indexOfNewClip];
        audioSource.Play();

        yield return new WaitForSeconds(audioSource.clip.length);

        _indexOfLastClip = _indexOfNewClip;
        StartCoroutine("StartPlayingRandomMusic");
    }

    public static void PlaySound(string soundName)
    {
        AudioClip sound = Resources.Load<AudioClip>(Path.Combine("Audio/SFX/", soundName));

        if (sound != null)
        {
            AudioSource.PlayClipAtPoint(sound, Vector3.zero, PlayerPrefs.GetFloat("sfxVolume"));
        }
        else
        {
            throw new AudioManagerException($"Звука '{soundName}' не было найдено в папке Audio/SFX.");
        }
    }
}
