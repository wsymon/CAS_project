
using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using Unity.Collections;
using Unity.Burst.Intrinsics;
using System;

public class TileMapInteractibility : MonoBehaviour
{
    //necessary serialisations
    [SerializeField]
    Camera mainCamera;

    [SerializeField]
    AnimatedTile Hover;

    [SerializeField]
    Grid MapGrid;

    [SerializeField]
    Tilemap BaseTileMap;

    [SerializeField]
    Tilemap TileHighlightsTileMap;

    [SerializeField]
    GameObject playerData;

    [SerializeField]
    Tilemap playerTileMap;

    [SerializeField]
    GameObject TileInformationMenuObject;

    [SerializeField]
    GameObject TileEditMenuObject;

    [SerializeField]
    TextMeshProUGUI EducationInfo;

    [SerializeField]
    TextMeshProUGUI EditCurrentTileName;

    [SerializeField]
    GameObject InfoDisplayofTile;

    [SerializeField]
    TextMeshProUGUI TileInforSeqDisplay;

    [SerializeField]
    TextMeshProUGUI TileInfoOutputDisplay;

    [SerializeField]
    SpriteRenderer InfoMenuBacking;

    [SerializeField]
    SpriteRenderer EditMenuBacking;

    //vectors for past/current selected tile and current/past tile mouse is on 
    public Vector3Int PastSelected = new Vector3Int(0, 0, 0);
    public Vector3Int SelectedGrid = new Vector3Int(0, 0, 0);
    private Vector3Int PastGrid = new Vector3Int(0, 0, 0);
    private Vector3Int CurrentGrid = new Vector3Int(0, 0, 0);


    //small start look to ensure first hover animation plays wherever mouse is initially (not just at 0,0)
    void Start()
    {
        var mousePosReal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var posee = Input.mousePosition;

        CurrentGrid = MapGrid.WorldToCell(mousePosReal);
        PastGrid = CurrentGrid;
        HighlightUpdate(CurrentGrid);

        BaseTileMap.SetTile(new Vector3Int(-7, -1, 0), null);
        playerTileMap.SetTile(new Vector3Int(-7, -1, 0), null);

    }

    void Update()
    {
        //getting the current grid position for mouse 
        var mousePosReal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CurrentGrid = MapGrid.WorldToCell(mousePosReal);
        //if left click...
        //later add to check if pause is open
        if (Input.GetMouseButtonDown(0))
        {
            LeftClick(CurrentGrid);
        }

        //esc key shuts UI for tile info/editing
        if (Input.GetButtonDown("Escape"))
        {
            TileInfoMenuCloser();
            TileEditMenuCloser();
        }

        //every frame update the highlights based on curretn grid position
        HighlightUpdate(CurrentGrid);
    }

    //all logic for left clicks in terms of tile edit/info UI and links to open/close functions
    private void LeftClick(Vector3Int selectedGrid)
    {
        //prelim stuff
        var mousePosReal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 Colider_position = mousePosReal;

        //later add other top UI to here. 
        //provided the click wasn't on the UI
        if ((Math.Abs(InfoMenuBacking.bounds.center.y) - Math.Abs(InfoMenuBacking.bounds.extents.y)) > Math.Abs(mousePosReal.y) || Math.Abs(mousePosReal.y) > (Math.Abs(InfoMenuBacking.bounds.center.y) + Math.Abs(InfoMenuBacking.bounds.extents.y)) || (Math.Abs(InfoMenuBacking.bounds.center.x) - Math.Abs(InfoMenuBacking.bounds.extents.x)) > Math.Abs(mousePosReal.x) || Math.Abs(mousePosReal.x) > (Math.Abs(InfoMenuBacking.bounds.center.x) + Math.Abs(InfoMenuBacking.bounds.extents.x)))
        {
            //if player clicks on the same tile twice, then UI will open/close depending on whether it is closed/opened
            if (selectedGrid == PastSelected)
            {
                if (TileInformationMenuObject.activeSelf == true)
                {
                    TileEditMenuCloser();
                    TileInfoMenuCloser();
                }
                else
                {
                    TileInformationMenuObject.SetActive(true);
                    TileEditMenuObject.SetActive(true);
                }
            }
            //if click was on a new tile in player tilemap and not in the UI, check if tile is customTile, if so update menu with that new tile info
            if (selectedGrid != PastSelected && playerTileMap.HasTile(selectedGrid) == true)
            {
                if (playerTileMap.GetTile(selectedGrid) is customTile)
                {
                    MenuUpdater(selectedGrid, playerTileMap);
                    TileHighlightsTileMap.SetTile(PastSelected, null);
                    TileHighlightsTileMap.SetTile(PastGrid, null);
                }
                else
                {
                    Debug.Log("Tile at that point is not a custom tile (Have you forgotten to place one here?)");
                    TileHighlightsTileMap.SetTile(PastSelected, null);
                }
            }
            //but if click was in base tilemap and not in the UI, do the same as above and update menu with the new info
            else if (selectedGrid != PastSelected && BaseTileMap.HasTile(selectedGrid))
            {
                if (BaseTileMap.GetTile(selectedGrid) is customTile)
                {
                    MenuUpdater(selectedGrid, BaseTileMap);
                    TileHighlightsTileMap.SetTile(PastSelected, null);
                }
                else
                {
                    Debug.Log("Tile at that point is not a custom tile (Have you forgotten to place one here?)");
                    TileHighlightsTileMap.SetTile(PastSelected, null);
                    Temp(selectedGrid, BaseTileMap);
                }
            }
            //changes global selected to what just occured (redundant but don't touch in case it breaks idk) and same for past selected
            SelectedGrid = selectedGrid;
            PastSelected = selectedGrid;
        }
    }
    //updates the animated tile for tile highlighter
    private void HighlightUpdate(Vector3Int currentGrid)
    {
        //provided menu is not open, do the normal followery thing (delete past and make present new)
        if (TileInformationMenuObject.activeSelf == false)
        {
            if (currentGrid != PastGrid)
            {
                TileHighlightsTileMap.SetTile(currentGrid, Hover);
                TileHighlightsTileMap.SetTile(PastGrid, null);
            }
        }
        //if menu was true just continously make the current SelectedGrid highlighted and remove all others. 
        //probably redundant with the similar logic in the LeftClick() function, but just in case 
        else if (TileInformationMenuObject.activeSelf == true)
        {
            if (TileHighlightsTileMap.GetTile(SelectedGrid) != Hover)
            {
                TileHighlightsTileMap.SetTile(PastSelected, Hover);
            }
        }
        //resets old/new status
        PastGrid = currentGrid;
    }

    //closes the info/edit menus respectively
    public void TileInfoMenuCloser()
    {
        TileInformationMenuObject.SetActive(false);
        TileHighlightsTileMap.SetTile(PastSelected, null);
        TileHighlightsTileMap.SetTile(SelectedGrid, null);
    }
    public void TileEditMenuCloser()
    {
        TileEditMenuObject.SetActive(false);
        TileHighlightsTileMap.SetTile(PastSelected, null);
        TileHighlightsTileMap.SetTile(SelectedGrid, null);
    }

    //updates the menus. logic needed for applying to text boxes changes and whether to open edit menu based on bool yet to be added in customTile info set
    //also change structure type from int? I think save file can be shortened to just lvl, strucutre and coord but recheck logic for finding file...
    public void MenuUpdater(Vector3Int currentGrid, Tilemap specificTileMap)
    {
        Debug.Log("MenuUpdater");
        customTile specificTile = (customTile)specificTileMap.GetTile(currentGrid);
        Sprite tileInfoDisplay = specificTile.GetTileSprite(specificTile);
        SpriteRenderer realimage = InfoDisplayofTile.GetComponent<SpriteRenderer>();
        realimage.sprite = tileInfoDisplay;
        EducationInfo.text = specificTile.Education;
        EditCurrentTileName.text = specificTile.StructureType.ToSafeString();
        TileInformationMenuObject.SetActive(true);
        TileEditMenuObject.SetActive(true);  
        TileInforSeqDisplay.text = specificTile.Sequestration.ToString();
        TileInfoOutputDisplay.text = specificTile.Output.ToString();
    }

    //temp function that just grabs sprite from normal tile locaiton if clicked as it is not custom tile
    private void Temp(Vector3Int location, Tilemap map)
    {
        if (map.GetTile(location).GetType() == typeof(AnimatedTile))
        {
            AnimatedTile spspspsp = (AnimatedTile)map.GetTile(location);
            Sprite spspspsrpite = spspspsp.m_AnimatedSprites[0];
            SpriteRenderer im = InfoDisplayofTile.GetComponent<SpriteRenderer>();
            im.sprite = spspspsrpite;
            EducationInfo.text = "NOT A CUSTOM TILE and no info :3 (but animated!)";
            EditCurrentTileName.text = "NOT A CUSTOM TILE and no name :3 (but animated!)";
            TileInformationMenuObject.SetActive(true);
            TileEditMenuObject.SetActive(true);
        }
        else if (map.GetTile(location).GetType() == typeof(Tile))
        {
            Tile spspspsp = (Tile)map.GetTile(location);
            Sprite spspspsrpite = spspspsp.sprite;
            SpriteRenderer im = InfoDisplayofTile.GetComponent<SpriteRenderer>();
            im.sprite = spspspsrpite;
            EducationInfo.text = "NOT A CUSTOM TILE and no info :3";
            EditCurrentTileName.text = "NOT A CUSTOM TILE and no name :3";
            TileInformationMenuObject.SetActive(true);
            TileEditMenuObject.SetActive(true);
        }
        else
        {
            SpriteRenderer im = InfoDisplayofTile.GetComponent<SpriteRenderer>();
            im.sprite = null;
            EducationInfo.text = "Nothing info there D:";
            EditCurrentTileName.text = "Nothing there D:";
            TileInformationMenuObject.SetActive(true);
            TileEditMenuObject.SetActive(true);
        }
        PastSelected = location;
    }   
}