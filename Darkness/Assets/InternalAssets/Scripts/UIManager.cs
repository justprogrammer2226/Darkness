using UnityEngine;
using UnityEngine.UI;

class UIManager : MonoBehaviour
{
    public Slider musicSlider;
    public Slider sfxSlider;

    private float _lastMusicVolume;
    private float _lastSfxVolume;

    private void Start()
    {
        if (PlayerPrefs.HasKey("musicVolume") && PlayerPrefs.HasKey("sfxVolume"))
        {
            musicSlider.value = AudioManager.Instance.GetMusicVolume();
            sfxSlider.value = AudioManager.Instance.GetSfxVolume();
        }

        musicSlider.onValueChanged.AddListener((value) => SetMusicVolume(value));
        sfxSlider.onValueChanged.AddListener((value) => SetSfxVolume(value));

        _lastMusicVolume = musicSlider.value;
        _lastSfxVolume = sfxSlider.value;
    }

    public void SetMusicVolume(float volume)
    {
        // This is done for optimization. Thus, we save fewer times.
        // We still will not hear the difference in 0.05. :)
        // So why save once again?
        if (Mathf.Abs(_lastMusicVolume - musicSlider.value) > 0.05)
        {
            AudioManager.Instance.SetMusicVolume(volume);
        }
    }

    public void SetSfxVolume(float volume)
    {
        // This is done for optimization. Thus, we save fewer times.
        // We still will not hear the difference in 0.05. :)
        // So why save once again?
        if (Mathf.Abs(_lastSfxVolume - sfxSlider.value) > 0.05)
        {
            AudioManager.Instance.SetSfxVolume(volume);
        }
    }

    public void PlayClickSound()
    {
        AudioManager.PlaySound("ui_button_click");
    }
}
