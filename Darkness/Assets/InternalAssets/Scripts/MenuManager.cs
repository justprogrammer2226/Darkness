using UnityEngine;

class MenuManager : MonoBehaviour
{
    public static MenuManager instance = null;

    public GameObject startMenu;
    public GameObject optionsMenu;

    private void Awake()
    {
        instance = this;
    }

    public void ActivateStartMenu()
    {
        startMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ActivateOptionsMenu()
    {
        startMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }
}
