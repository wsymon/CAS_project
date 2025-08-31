using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;


public class open_scne : MonoBehaviour
{
    //serialize field for animation object
    [SerializeField]
    Animator animator;

    //current player name finder to send to function that sets up tilempa
    [SerializeField]
    Object PlayerData;


    public void Open_map_scene()
    {
        StartCoroutine(SceneLoad());
    }

    IEnumerator SceneLoad()
    {
        //open trigger for closing scene animation
        animator.SetTrigger("Closing");

        //pause for animation to close
        yield return new WaitForSeconds(2);

        //load map scene named "1" in BUILD SETTINGS.
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(2);
    }   
}