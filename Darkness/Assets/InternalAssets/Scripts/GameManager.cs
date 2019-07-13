using UnityEngine;

class GameManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.Instance.SetBackgroundClips("Backrub", "Beach_House", "Beam_Me_Up", "Bit_Coin");
        AudioManager.Instance.StartPlayingBackgroundMusic();
    }
}
