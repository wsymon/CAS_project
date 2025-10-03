using System;
using System.Collections;
using System.Threading;
using Microsoft.Unity.VisualStudio.Editor;
using Unity.VisualScripting;
using Unity.VisualScripting.AssemblyQualifiedNameParser;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    [SerializeField]
    GameObject TutorialObject;

    [SerializeField]
    GameObject CreateObject;

    [SerializeField]
    GameObject SelectObject;

    [SerializeField]
    GameObject TutorialButton;

    [SerializeField]
    GameObject CreditObject;

    [SerializeField]
    GameObject QuitObject;


    //GIVE PROPERTY TO BLOCK OUT ALL OTHER INPUT INCLUDING CLICKING EITHER SIDE OF IT EXCEPT FOR THE LITTLE "X" BUTTON AT THE TOP RIGHT. 
    void Update()
    {
        if (Input.GetButtonDown("Jump"))
        {
            TutorialClose();
        }
    }

    //open function with time counter function below it
    public void TutorialOpen()
    {
        //TutorialObject.GetComponent<UnityEngine.UI.Image>().raycastTarget = true;
        //StartCoroutine(CloseAnimation(CreditObject));
        //StartCoroutine(CloseAnimation(QuitObject));
        //StartCoroutine(CloseAnimation(TutorialButton));
        //  StartCoroutine(CloseAnimation(SelectObject));
        // StartCoroutine(CloseAnimation(CreateObject));

        StartCoroutine(OpenAnimation(TutorialObject));
    }
    IEnumerator OpenAnimation(GameObject gameObject)
    {
        for (float alpha = 0; alpha >= 1; alpha += 0.00000001f)
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, alpha);
            Debug.Log(gameObject.GetComponent<UnityEngine.UI.Image>().color);
            yield return new WaitForSecondsRealtime(1f);
        }
        gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 1);
    }

    //close function withe time counter below it
    public void TutorialClose()
    {
        //TutorialObject.GetComponent<UnityEngine.UI.Image>().raycastTarget = false;
        TutorialButton.SetActive(true);
    //    StartCoroutine(OpenAnimation(CreditObject));
     //   StartCoroutine(OpenAnimation(QuitObject));
      //  StartCoroutine(OpenAnimation(TutorialButton));
       // StartCoroutine(OpenAnimation(SelectObject));
       // StartCoroutine(OpenAnimation(CreateObject));


        StartCoroutine(CloseAnimation(TutorialObject));
    }
    IEnumerator CloseAnimation(GameObject gameObject)
    {
        for (float alpha = 1; alpha <= 0; alpha -= 0.00000001f)
        {
            gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, alpha);
            Debug.Log(gameObject.GetComponent<UnityEngine.UI.Image>().color);
            yield return new WaitForSecondsRealtime(1f);
        }
        gameObject.GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
    }
}