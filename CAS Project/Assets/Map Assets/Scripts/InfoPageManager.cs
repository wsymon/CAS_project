using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoPageManager : MonoBehaviour
{
    [SerializeField]
    Button LeftButton;

    [SerializeField]
    Button RightButton;

    [SerializeField]
    TextMeshProUGUI TileInfoText;


    //script for the little button guys that make the pages of the education info of tiles change :3
    void Start()
    {
        ResetButtons();
    }

    //ensures that buttons are uninteractable if there is only 1 page
    public void ResetButtons()
    {
        if (TileInfoText.textInfo.pageCount > 1)
        {
            LeftButton.interactable = true;
            RightButton.interactable = true;
        }
    }

    //function for right page button
    public void RightPageTurn()
    {
        if (TileInfoText.pageToDisplay == TileInfoText.textInfo.pageCount)
        {
            TileInfoText.pageToDisplay = 1;
        }
        else
        {
            TileInfoText.pageToDisplay++;
        }
    }

    //function for left page button
    public void LeftPageTurn()
    {
        if (TileInfoText.pageToDisplay == 1)
        {
            TileInfoText.pageToDisplay = TileInfoText.textInfo.pageCount;
        }
        else
        {
            TileInfoText.pageToDisplay--;
        }
    }
}
