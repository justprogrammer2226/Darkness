using UnityEngine;

class MenusManager : MonoBehaviour
{
    public static MenusManager instance = null;

    public GameObject startMenu;
    public GameObject optionsMenu;

    private void Awake()
    {
        if (instance != null) Destroy(gameObject);
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
