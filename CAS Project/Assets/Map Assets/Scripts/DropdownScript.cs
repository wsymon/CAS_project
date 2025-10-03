using UnityEngine;
using UnityEngine.Tilemaps;

public class DropdownScript : MonoBehaviour
{
    [SerializeField]
    GameObject Options;

    [SerializeField]
    GameObject PlayerData;

    [SerializeField]
    Tilemap BaseTileMap;

    public string currentSelectedEditPossibility;

    //button click for the down arrow for drop down menu
    public void DropdownButtonClick()
    {
        if (Options.activeSelf == true)
        {
            Options.SetActive(false);
        }
        else
        {
            Options.SetActive(true);
        }
    }

    //
    public void ConfirmEditPossibility()
    {
        //collapse options gameobject, change text of top text, ensure this is linked/accessible to the function in tilemapinteractivity for confirm change
    }

    public void UpdateDropDownMenu()
    {
        Debug.Log("clicked");
        //delete extra child objects
        Vector3Int currentBaseTileGrid = PlayerData.GetComponent<TileMapInteractibility>().SelectedGrid;
        customTile BaseTile = (customTile)BaseTileMap.GetTile(currentBaseTileGrid);
        foreach (string tileEditPossibility in BaseTile.SuitableTileTypePlacements)
        {
            Debug.Log(tileEditPossibility);
            //copy standard game object 

            //change text

            //move position -y 30 some amount
        }
    }
}
