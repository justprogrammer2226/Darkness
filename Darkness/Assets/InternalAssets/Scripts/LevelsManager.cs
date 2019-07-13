using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Manages all work on the transition between levels. </summary>
/// <remarks> IMPORTANT!!! The level scenes must be in order. </remarks>
public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance;

    [Header("Colors for text")]
    public Color activeTextColor;
    public Color inactiveTextColor;

    [Header("Indexes from 'Scenes in build'")]
    public int startSceneIndex;
    public int firstLevelIndex;

    [Header("Level buttons")]
    public Button[] levelButtons;

    [Header("Debug")]
    // The index of the current level betweeb 0 and levelButtons.Length - 1.
    // This is not an index in 'Scenes in build'.
    [SerializeField] private int currentLevelIndex;
    // Just number of open levels.
    [SerializeField] private int numberOfOpenLevels;
    // We have to memorize the number of open levels.
    // Because when we switch to another scene, the array of buttons will be reset.
    [SerializeField] private int numberOfLevels;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        LoadInfo();

        if (levelButtons == null) return;

        for (int i = 0; i < levelButtons.Length; i++)
        {
            int temp = i;

            levelButtons[temp].onClick.AddListener(() => ToLevel(temp));

            if (temp < numberOfOpenLevels)
            {
                levelButtons[temp].interactable = true;
                levelButtons[temp].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = activeTextColor;
            }
            else
            {
                levelButtons[temp].interactable = false;
                levelButtons[temp].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = inactiveTextColor;
            }
        }
    }

    private void Update()
    {
        // Происходит какое - то действие(прохождение уровня)
        if (Input.GetKeyDown(KeyCode.W))
        {
            CompleteLevel();
        } 

        // Очищаем сохранения, для дебага
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerPrefs.DeleteKey("currentLevelIndex");
            PlayerPrefs.DeleteKey("numberOfOpenLevels");
            PlayerPrefs.DeleteKey("numberOfLevels");
            PlayerPrefs.DeleteKey("startSceneIndex");
            PlayerPrefs.DeleteKey("firstLevelIndex");
            LoadInfo();
        }
    }

    /// <summary> This method should call when completing any level. </summary>
    public void CompleteLevel()
    {
        if (currentLevelIndex + 1 == numberOfOpenLevels && currentLevelIndex + 1 < numberOfLevels)
        {
            currentLevelIndex++;
            numberOfOpenLevels++;
            SaveInfo();
            AsyncSceneLoader.LoadScene(firstLevelIndex + currentLevelIndex);
        }
        else if (currentLevelIndex + 1 < numberOfOpenLevels)
        {
            currentLevelIndex++;
            SaveInfo();
            AsyncSceneLoader.LoadScene(firstLevelIndex + currentLevelIndex);
        }
        else if (numberOfLevels == numberOfOpenLevels)
        {
            LoadStartMenu();
        }
    }

    /// <summary> Moves to level with 'levelID' index. Indexing starts at 0. </summary>
    private void ToLevel(int levelID)
    {
        if (levelID >= 0 && levelID < numberOfOpenLevels)
        {
            currentLevelIndex = levelID;
            SaveInfo();
            AsyncSceneLoader.LoadScene(firstLevelIndex + levelID);
        }
    }

    private void LoadInfo()
    {
        // Set default values if they are not already set.
        if (!PlayerPrefs.HasKey("startSceneIndex")) PlayerPrefs.SetInt("startSceneIndex", startSceneIndex);
        if (!PlayerPrefs.HasKey("firstLevelIndex")) PlayerPrefs.SetInt("firstLevelIndex", firstLevelIndex);
        if (!PlayerPrefs.HasKey("currentLevelIndex")) PlayerPrefs.SetInt("currentLevelIndex", 0);
        if (!PlayerPrefs.HasKey("numberOfOpenLevels")) PlayerPrefs.SetInt("numberOfOpenLevels", 1);
        if (!PlayerPrefs.HasKey("numberOfLevels")) PlayerPrefs.SetInt("numberOfLevels", levelButtons != null && levelButtons.Length != 0 ? levelButtons.Length : 0);

        // Load saved values.
        startSceneIndex = PlayerPrefs.GetInt("startSceneIndex");
        firstLevelIndex = PlayerPrefs.GetInt("firstLevelIndex");
        currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex");
        numberOfOpenLevels = PlayerPrefs.GetInt("numberOfOpenLevels");
        numberOfLevels = PlayerPrefs.GetInt("numberOfLevels");
    }

    private void SaveInfo()
    {
        PlayerPrefs.SetInt("startSceneIndex", startSceneIndex);
        PlayerPrefs.SetInt("firstLevelIndex", firstLevelIndex);
        PlayerPrefs.SetInt("currentLevelIndex", currentLevelIndex);
        PlayerPrefs.SetInt("numberOfOpenLevels", numberOfOpenLevels);
        if(levelButtons != null && levelButtons.Length != 0) PlayerPrefs.SetInt("numberOfLevels", levelButtons.Length);
        PlayerPrefs.Save();
    }

    public void LoadStartMenu()
    {
        AsyncSceneLoader.LoadScene(startSceneIndex);
    }
}
