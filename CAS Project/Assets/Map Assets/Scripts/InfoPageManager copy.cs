using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPageManager2 : MonoBehaviour
{
    [SerializeField]
    Button LeftButton2;

    [SerializeField]
    Button RightButton2;

    [SerializeField]
    TextMeshProUGUI TileInfoText2;


    //script for the little button guys that make the pages of the education info of tiles change :3


    void Start()
    {
        ResetButtons2();
    }

    //ensures that buttons are uninteractable if there is only 1 page
    public void ResetButtons2()
    {
        if (TileInfoText2.textInfo.pageCount > 1)
        {
            LeftButton2.interactable = true;
            RightButton2.interactable = true;
        }
        else
        {
       //     LeftButton.interactable = false;
         //   RightButton.interactable = false;
        }
    }

    //function for right page button
    public void RightPageTurn2()
    {
        Debug.Log(TileInfoText2.pageToDisplay + " " +  TileInfoText2.textInfo.pageCount);
        if (TileInfoText2.pageToDisplay == TileInfoText2.textInfo.pageCount)
        {
            TileInfoText2.pageToDisplay = 1;
        }
        else
        {
            TileInfoText2.pageToDisplay++;
        }
    }

    //function for left page button
    public void LeftPageTurn2()
    {
        Debug.Log(TileInfoText2.pageToDisplay + " " +  TileInfoText2.textInfo.pageCount);
        if (TileInfoText2.pageToDisplay == 1)
        {
            TileInfoText2.pageToDisplay = TileInfoText2.textInfo.pageCount;
        }
        else
        {
            TileInfoText2.pageToDisplay--;
        }
    }
}
