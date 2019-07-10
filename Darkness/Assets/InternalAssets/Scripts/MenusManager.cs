using UnityEngine;

class MenusManager : MonoBehaviour
{
    public static MenusManager instance = null;

    public GameObject startMenu;
    public GameObject optionsMenu;
    public GameObject levelsMenu;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
        instance = this;
    }

    public void ActivateStartMenu()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
        levelsMenu.SetActive(false);
    }

    public void ActivateOptionsMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
        levelsMenu.SetActive(false);
    }

    public void ActivateLevelsMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(false);
        levelsMenu.SetActive(true);
    }
}
