using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class open_scne : MonoBehaviour
{

    public void Start()
    {
        
    }
    //serialize field for animation object
    [SerializeField]
    Animator animator;

    public void Open_map_scene()
    {
        Debug.Log("le button est hit");
        StartCoroutine(SceneLoad());
    }

     
        
    
    IEnumerator SceneLoad()
    {
        Debug.Log("inside enumerabator");
        //open trigger for closing scene animation
        animator.SetTrigger("Transition");

        //pause for animation to close
        yield return new WaitForSeconds(2);

        //load map scene named "1" in BUILD SETTINGS.
        SceneManager.LoadScene(1);
    }

    
}
