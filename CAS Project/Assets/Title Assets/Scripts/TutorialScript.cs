using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialScript : MonoBehaviour
{
    [SerializeField]
    Button ForwardButton;

    [SerializeField]
    Button BackwardButton;

    [SerializeField]
    GameObject TutorialObject;

    [SerializeField]
    GameObject TopPage;

    [SerializeField]
    GameObject FileCompletionObject;
    
    [SerializeField]
    GameObject BottomPage;

    [SerializeField]
    Button FileCompletedbutton;

    public Sprite Page1;
    public Sprite Page2;
    public Sprite Page3;

    private bool forwardAnimation = false;
    private bool backwardanimation = false;
    public float ElapsedTime = 0;
    public float PercentComplete = 0;
    public int EndTime = 1;

    public void Update()
    {
        if(TutorialObject.activeSelf == true)
        {
            FileCompletedbutton.interactable = false;
        }
        else
        {
            FileCompletedbutton.GetComponent<Button>().interactable = true;
        }

        if(forwardAnimation == true)
        {
            PageAnimationForward();
        }
        if(backwardanimation == true)
        {
            PageAnimationBackward();
        }
    }

    //CHANGE LAYER POSITION SO NEXT ONE IS UNDER CURRENT ONE...INVERSE FOR BACKWARD 

    public void PageForward()
    {
        ForwardButton.interactable = false;
        BackwardButton.interactable = false;
        TopPage.SetActive(true);
        if(BottomPage.GetComponent<Image>().sprite == Page1)
        {
            TopPage.GetComponent<Image>().sprite = Page1;
            BottomPage.GetComponent<Image>().sprite = Page2;
            forwardAnimation = true;
        }
        else if (BottomPage.GetComponent<Image>().sprite == Page2)
        {
            TopPage.GetComponent<Image>().sprite = Page2;
            BottomPage.GetComponent<Image>().sprite = Page3;
            forwardAnimation = true;
        }
        else
        {
            TopPage.GetComponent<Image>().sprite = Page3;
            BottomPage.GetComponent<Image>().sprite = Page1;
        }
        forwardAnimation = true;
    }
    

    public void PageBackward()
    {
        ForwardButton.interactable = false;
        BackwardButton.interactable = false;
        TopPage.SetActive(true);
        if(BottomPage.GetComponent<Image>().sprite == Page1)
        {
            TopPage.GetComponent<Image>().sprite = Page1;
            BottomPage.GetComponent<Image>().sprite = Page3;
        }
        else if (BottomPage.GetComponent<Image>().sprite == Page2)
        {
            TopPage.GetComponent<Image>().sprite = Page2;
            BottomPage.GetComponent<Image>().sprite = Page1;
        }
        else
        {
            TopPage.GetComponent<Image>().sprite = Page3;
            BottomPage.GetComponent<Image>().sprite = Page2;
        }
        backwardanimation = true;
    }

    public void PageAnimationForward()
    {
        ElapsedTime += Time.deltaTime;
        PercentComplete = 1 - (ElapsedTime / EndTime);
        TopPage.transform.localPosition = new Vector2(Mathf.Lerp(1, 0, PercentComplete)*1000, 0);
        TopPage.GetComponent<Image>().color = new Color(255, 255, 255, Mathf.Lerp(0, 1, PercentComplete));

        if(ElapsedTime > EndTime)
        {
            forwardAnimation = false;
            ElapsedTime = 0;
            PercentComplete = 0;
            TopPage.SetActive(false);
            TopPage.transform.position = new Vector2(0, 0);
            ForwardButton.interactable = true;
            BackwardButton.interactable = true;
        }
    }

    public void PageAnimationBackward()
    {
        ElapsedTime += Time.deltaTime;
        PercentComplete = 1 - (ElapsedTime / EndTime);
        TopPage.transform.localPosition = new Vector2(Mathf.Lerp(1, 0, PercentComplete)*-1000, 0);
        TopPage.GetComponent<Image>().color = new Color(255, 255, 255, Mathf.Lerp(0, 1, PercentComplete));

        if(ElapsedTime > EndTime)
        {
            backwardanimation = false;
            ElapsedTime = 0;
            PercentComplete = 0;
            TopPage.SetActive(false);
            TopPage.transform.position = new Vector2(0, 0);
            ForwardButton.interactable = true;
            BackwardButton.interactable = true;
        }
    }
}