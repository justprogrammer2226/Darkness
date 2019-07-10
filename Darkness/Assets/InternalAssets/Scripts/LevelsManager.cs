using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary> Manages all work on the transition between levels. </summary>
/// <remarks>
/// For the script to work correctly,
/// it is necessary that all levels be placed in this order in the “Scenes in build”:
/// 0 - Start menu,
/// 1 - n - All levels.
/// IMPORTANT!!! SCRIPT WORKS ONLY WITH TEXT MESH PRO!!!
/// </remarks>
public class LevelsManager : MonoBehaviour
{
    public static LevelsManager instance;

    [SerializeField] private int indexCurrentLevel;
    [SerializeField] private int numberOfOpenLevels;
    [SerializeField] private int numberOfLevels;

    public Color activeTextColor;
    public Color inactiveTextColor;

    public Button[] buttons;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    private void Start()
    {
        LoadInfo();

        for (int i = 0; i < buttons.Length; i++)
        {
            int levelIndex = i;
            buttons[levelIndex].onClick.AddListener(() => ToLevel(levelIndex + 1));

            if (i < numberOfOpenLevels)
            {
                buttons[levelIndex].interactable = true;
                buttons[levelIndex].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = activeTextColor;
            }
            else
            {
                buttons[levelIndex].interactable = false;
                buttons[levelIndex].gameObject.GetComponentInChildren<TextMeshProUGUI>().color = inactiveTextColor;
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

        // В главное меню, для дебага
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SceneManager.LoadScene(0);
        }

        // Очищаем сохранения, для дебага
        if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerPrefs.DeleteKey("indexCurrentLevel");
            PlayerPrefs.DeleteKey("numberOfOpenLevels");
            PlayerPrefs.DeleteKey("numberOfLevels");
            LoadInfo();
        }
    }

    /// <summary> This method should call when completing any level. </summary>
    public void CompleteLevel()
    {
        LoadInfo();

        if (indexCurrentLevel == numberOfOpenLevels && indexCurrentLevel < numberOfLevels)
        {
            numberOfOpenLevels++;
            indexCurrentLevel++;
        }
        else if (indexCurrentLevel < numberOfOpenLevels)
        {
            indexCurrentLevel++;
        }

        SaveInfo();

        SceneManager.LoadScene(indexCurrentLevel);
    }

    private void ToLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex <= numberOfOpenLevels)
        {
            indexCurrentLevel = levelIndex;
            SaveInfo();
            SceneManager.LoadScene(indexCurrentLevel);
        }
        else
        {
            Debug.Log("Извиняемся, данная игра находиться на стадии разработки, по этому часть уровней ещё не проработана.");
        }
    }

    private void LoadInfo()
    {
        // Set the value to 0, since we are now at the 0 starting scene.
        indexCurrentLevel = PlayerPrefs.GetInt("indexCurrentLevel", 0);
        // Set the value to 0, because only the first level is available to us.
        numberOfOpenLevels = PlayerPrefs.GetInt("numberOfOpenLevels", 1);
        // Set the value to 0 of buttons length.
        numberOfLevels = PlayerPrefs.GetInt("numberOfLevels", buttons != null && buttons.Length != 0 ? buttons.Length : 0);
    }

    private void SaveInfo()
    {
        PlayerPrefs.SetInt("numberOfOpenLevels", numberOfOpenLevels);
        PlayerPrefs.SetInt("indexCurrentLevel", indexCurrentLevel);
        if(buttons != null && buttons.Length != 0) PlayerPrefs.SetInt("numberOfLevels", buttons.Length);
        PlayerPrefs.Save();
    }
}
