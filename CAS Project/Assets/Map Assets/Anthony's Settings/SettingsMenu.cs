using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] GameObject settingsMenu;

    //hey Anthony :3 I just made it so pressing the button at any time will shut/open the menu 
    //based on whether or not it is already open, just so that since the button will be visible 
    //all of the time at the top of the screen it can, double clicking it opens+shuts the menu
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
