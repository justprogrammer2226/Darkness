using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AsyncSceneLoader : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Index of the scene that will be loaded when you first start the game")]
    public int startSceneIndex;
    [Tooltip("Slider on which the scene loading information will be displayed")]
    public Slider loadingSlider;
    [Tooltip("The text that will display information about loading the scene as a percentage")]
    public TextMeshProUGUI progressText;

    private int _sceneIndex;

    private void Start()
    {
        // If there is no scene index for asynchronous loading, then the start scene is loaded.
        _sceneIndex = PlayerPrefs.GetInt("sceneID_AsyncLoad", startSceneIndex);
        // We delete the key, because if we do not remove it, the initial launch of the game may not load the start menu, but some level.
        PlayerPrefs.DeleteKey("sceneID_AsyncLoad");
        // We start asynchronous loading of level.
        StartCoroutine(AsyncLoad());
    }

    /// <summary> Asynchronously load the scene with the index "_sceneIndex". </summary>
    private IEnumerator AsyncLoad()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(_sceneIndex);

        while(!asyncOperation.isDone)
        {
            loadingSlider.value = asyncOperation.progress / 0.9f;
            progressText.text = string.Format("{0:0}%", loadingSlider.value * 100);
            yield return null;
        }
    }

    /// <summary> Asynchronously load the scene with the index "buildIndex". </summary>
    public static void LoadScene(int buildIndex)
    {
        // Save the index value for asynchronous loading level.
        PlayerPrefs.SetInt("sceneID_AsyncLoad", buildIndex);
        PlayerPrefs.Save();
        // Load the loading scene.
        SceneManager.LoadScene(0);
    }
}
