using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;

    public void SettingsPopUpChange()
    {
        if (settingsMenu.activeSelf == true)
        {
            settingsMenu.SetActive(false);
        }
        else if (settingsMenu.activeSelf == false)
        {
            settingsMenu.SetActive(true);
        }
    }
    public void MainMenu()
    {
        settingsMenu.SetActive(false);
        SceneManager.LoadScene("Title");
    }

}
