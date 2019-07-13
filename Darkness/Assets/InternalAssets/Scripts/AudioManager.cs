using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance != null)
            {
                return _instance;
            }

            return new GameObject("SoundManager").AddComponent<AudioSource>().gameObject.AddComponent<AudioManager>();
        }
    }

    private List<AudioClip> _backgroundClips = new List<AudioClip>();

    private AudioSource _audioSource;

    private int _indexOfLastClip;
    private int _indexOfNewClip;

    void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        _audioSource = GetComponent<AudioSource>();
        DontDestroyOnLoad(gameObject);
    }

    public void SetMusicVolume(float volume)
    {
        _audioSource.volume = volume;
        PlayerPrefs.SetFloat("musicVolume", volume);
        PlayerPrefs.Save();
    }

    public void SetSfxVolume(float volume)
    {
        PlayerPrefs.SetFloat("sfxVolume", volume);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume()
    {
        return PlayerPrefs.GetFloat("musicVolume", 1);
    }

    public float GetSfxVolume()
    {
        return PlayerPrefs.GetFloat("sfxVolume", 1);
    }

    public void SetBackgroundClips(params string[] clipNames)
    {
        _backgroundClips.Clear();
        foreach(string clipName in clipNames)
        {
            AudioClip sound = Resources.Load<AudioClip>(Path.Combine("AudioManager/Music/", clipName));

            if (sound != null)
            {
                _backgroundClips.Add(sound);
            }
            else
            {
                throw new AudioManagerException($"Клипа '{clipName}' не было найдено в папке AudioManager/Music.");
            }
        }
    }

    public void StartPlayingBackgroundMusic()
    {
        if (_backgroundClips.Count > 1)
        {
            StartCoroutine("StartPlayingRandomMusic");
        }
        else
        {
            throw new AudioManagerException("Вы должно добавить минимум 2 мелодии для фоновой музыки.");
        }
    }

    public void StopPlayingBackgroundMusic(bool instantly)
    {
        if (instantly) _audioSource.Stop();
        StopCoroutine("StartPlayingRandomMusic");
        _indexOfLastClip = _indexOfNewClip;
    }

    private IEnumerator StartPlayingRandomMusic()
    {
        while (_indexOfLastClip == _indexOfNewClip) _indexOfNewClip = Random.Range(0, _backgroundClips.Count);

        _audioSource.clip = _backgroundClips[_indexOfNewClip];
        _audioSource.Play();

        yield return new WaitForSeconds(_audioSource.clip.length);

        _indexOfLastClip = _indexOfNewClip;
        StartCoroutine("StartPlayingRandomMusic");
    }

    public static void PlaySound(string soundName)
    {
        AudioClip sound = Resources.Load<AudioClip>(Path.Combine("AudioManager/Sounds/", soundName));

        if (sound != null)
        {
            AudioSource.PlayClipAtPoint(sound, Vector3.zero, _instance.GetSfxVolume());
        }
        else
        {
            throw new AudioManagerException($"Звука '{soundName}' не было найдено в папке AudioManager/Sounds.");
        }
    }
}
