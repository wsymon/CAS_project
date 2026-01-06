using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitAndTitleScript : MonoBehaviour
{
    [SerializeField]
    GameObject MenuGameobject;

    public void ExitToTitle()
    {
        StartCoroutine(Exit("Title"));
    }

    public void ExitGame()
    {
        StartCoroutine(Exit("Exit"));
    }

    IEnumerator Exit(string destination)
    {

        yield return new WaitForSeconds(2.1f);
        if(destination == "Exit")
        {
            Application.Quit();
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
