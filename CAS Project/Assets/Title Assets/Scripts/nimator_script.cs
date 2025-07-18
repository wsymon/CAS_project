using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class nimator_script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    [SerializeField] Animator transition;
    public void Begin()
    {
        //functionality for final begin buttons (for selecting a file and creating new file/game, loads Map scene)
        Debug.Log("Button hit");
        transition.SetTrigger("Start");

        StartCoroutine(SceneLoad());
    }

    IEnumerator SceneLoad()
    {
        Debug.Log("inside enumerabator");
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1);

        SceneManager.LoadScene(1);
    }
}
