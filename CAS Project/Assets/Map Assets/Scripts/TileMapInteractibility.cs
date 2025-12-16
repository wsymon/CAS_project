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
using System.Linq;
using Mono.Cecil;
using UnityEngine.AI;
using UnityEditor;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using UnityEngine.Rendering;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.CompilerServices;

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
    UnityEngine.UI.Image InfoMenuBacking;

    [SerializeField]
    UnityEngine.UI.Image EditMenuBacking;

    [SerializeField]
    GameObject DropDownObject;

    [SerializeField]
    TextMeshProUGUI tempEditSeq;

    [SerializeField]
    TextMeshProUGUI tempEditCost;

    [SerializeField]
    TextMeshProUGUI tempEditOutput;

    [SerializeField]
    GameObject tempEditTileImage;

    [SerializeField]
    GameObject SettingsMenu;

    [SerializeField]
    GameObject SettingsButton;

    [SerializeField]
    UnityEngine.UI.Button EditConfirmButton;

    //vectors for past/current selected tile and current/past tile mouse is on 
    public Vector3Int PastSelected = new Vector3Int(0, 0, 0);
    public Vector3Int SelectedGrid = new Vector3Int(0, 0, 0);
    private Vector3Int PastGrid = new Vector3Int(0, 0, 0);
    private Vector3Int CurrentGrid = new Vector3Int(0, 0, 0);
    private string[] TileEditPossibilities;

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
            SelectedGrid = CurrentGrid;
            LeftClick(SelectedGrid);
        }
        //esc key shuts UI for tile info/editing
        else if (Input.GetButtonDown("Escape"))
        {
            TileInfoMenuCloser();
            TileEditMenuCloser();
        }
        else
        {
            //every frame update the highlights based on curretn grid position
            HighlightUpdate(CurrentGrid);
        }
    }

    //click logic with some hightlight stuff mixed in
    private void LeftClick(Vector3Int SelectedGrid)
    {

        //add later if pause menu is open
        //if(PauseMenuUIObject.ActiveSelf == true)
        //do nothing but call highlight function

        //MAKE AN ELSE IF LATER
        //if tile menus are open
        if (TileInformationMenuObject.activeSelf == true || SettingsMenu.activeSelf == true || SettingsButton.activeSelf == true)
        {
            //if menu open and click outside menu
            //later add global menu!!
            var mousePosReal = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                //if click was on the same spot and menu was open, shut menu
                if (SelectedGrid == PastSelected)
                {
                    TileEditMenuCloser();
                    TileInfoMenuCloser();
                    HighlightUpdate(SelectedGrid);
                    TileHighlightsTileMap.SetTile(SelectedGrid, null);
                    TileHighlightsTileMap.SetTile(SelectedGrid, null);
                }
                //if clicked on a different tile
                else
                {
                    TileHighlightsTileMap.SetTile(PastSelected, null);
                    TileHighlightsTileMap.SetTile(SelectedGrid, Hover);

                    //if a player tile is applicable at new selected point, call menu updater for the player tile
                    if (playerTileMap.HasTile(SelectedGrid))
                    {
                        TileHighlightsTileMap.SetTile(PastSelected, null);
                        EditMenuUpdaterandOpener(SelectedGrid);
                        InfoMenuUpdaterandOpener(SelectedGrid, playerTileMap);
                    }
                    //if a base tile is applicable (no player tile), call menu updater for base tile
                    else
                    {
                        TileHighlightsTileMap.SetTile(PastSelected, null);
                        EditMenuUpdaterandOpener(SelectedGrid);
                        InfoMenuUpdaterandOpener(SelectedGrid, BaseTileMap);

                    }
                    PastSelected = SelectedGrid;
                }
            }
            else
            {
                TileHighlightsTileMap.SetTile(SelectedGrid, null);
                TileHighlightsTileMap.SetTile(CurrentGrid, null);
                //do nothing as click invalid
            }
        }

        //menu was shut
        else
        {
            //if a player tile is applicable at selected point, call menu updater for the player tile
            if (playerTileMap.HasTile(SelectedGrid))
            {
                EditMenuUpdaterandOpener(SelectedGrid);
                InfoMenuUpdaterandOpener(SelectedGrid, playerTileMap);
            }
            //if a base tile is applicable (no player tile), call menu updater for base tile
            else
            {
                EditMenuUpdaterandOpener(SelectedGrid);
                InfoMenuUpdaterandOpener(SelectedGrid, BaseTileMap);
            }

            //set past highlight to null, set current to hover
            TileHighlightsTileMap.SetTile(PastGrid, null);
            TileHighlightsTileMap.SetTile(SelectedGrid, null);
            TileHighlightsTileMap.SetTile(SelectedGrid, Hover);
            PastSelected = SelectedGrid;
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
        DropDownObject.SetActive(false);
        TileHighlightsTileMap.SetTile(PastSelected, null);
        TileHighlightsTileMap.SetTile(SelectedGrid, null);
    }

    //logic, opening and updates to info tile menu
    public void InfoMenuUpdaterandOpener(Vector3Int currentGrid, Tilemap specificTileMap)
    {
        //ensures page turners buttons match properties of educational information
        EducationInfo.GetComponent<InfoPageManager>().ResetButtons();

        //  just updates all the educational/display info
        customTile specificTile = (customTile)specificTileMap.GetTile(currentGrid);
        Sprite tileInfoDisplay = specificTile.GetTileSprite(specificTile);
        SpriteRenderer realimage = InfoDisplayofTile.GetComponent<SpriteRenderer>();
        realimage.sprite = tileInfoDisplay;
        EducationInfo.text = specificTile.Education;
        EditCurrentTileName.text = specificTile.StructureType.ToSafeString();
        TileInformationMenuObject.SetActive(true);
        if (BaseTileMap.GetTile(currentGrid) is customTile)
        {
            customTile BaseTile = (customTile)BaseTileMap.GetTile(currentGrid);
            if (BaseTile.SuitableTileTypePlacements.Count() != 0)
            {
                TileEditMenuObject.SetActive(true);
                resetDropDown(currentGrid);
                resetEditDisplayInfo();
                DropDownObject.SetActive(true);
            }
        }
        TileInforSeqDisplay.text = specificTile.Sequestration.ToString();
        TileInfoOutputDisplay.text = specificTile.Output.ToString();
    }

    //logic, info and updates to edit menu for tiles 
    public void EditMenuUpdaterandOpener(Vector3Int currentGrid)
    {
        //if base tile at clicked position is customtile, will always be the case once completed but for interim period 
        if (BaseTileMap.GetTile(currentGrid) is customTile)
        {
            //if the tile at that position has editable possibilities then open edit menu
            customTile BaseTile = (customTile)BaseTileMap.GetTile(SelectedGrid);
            if (BaseTile.SuitableTileTypePlacements.Count() != 0)
            {
                //all stuff for changing information on edit menu!!
                resetDropDown(currentGrid);
                resetEditDisplayInfo();
                TileEditMenuObject.SetActive(true);
                DropDownObject.SetActive(true);

                //ensures selected options viability matches that presented by the confirm button
                EditMenuConfirmButtonUpdater();            
            }
            else
            {
                //if cannot edit then shut menu
                TileEditMenuCloser();
            }
        }
    }

    public void EditMenuConfirmButtonUpdater()
    {
        //if credit cost is not payable
        string choice = DropDownObject.GetComponent<Dropdown>().options[DropDownObject.GetComponent<Dropdown>().value].text.ToString();
        if(Resources.Load<customTile>(choice + "L1").CreditCost > CurrentPlayerData.RoundCredits)
        {
            //make button inaccessible and make text of it red 
            EditConfirmButton.GetComponent<UnityEngine.UI.Image>().color = new Color(6f, 185f, 185f, 255f);
            EditConfirmButton.interactable = false;
        }
        else
        {
            EditConfirmButton.GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, 255);
            EditConfirmButton.interactable = true;
        }
    }

    //resets the fields for possible options for dropdown menu
    public void resetDropDown(Vector3Int pos)
    {
        customTile BaseTile = (customTile)BaseTileMap.GetTile(pos);
        DropDownObject.GetComponent<Dropdown>().options.Clear();
        List<string> PossibleOptions = new List<string> { };
        foreach (string possibleOption in BaseTile.SuitableTileTypePlacements)
        {
            foreach(string Tech in CurrentPlayerData.DevelopedTechnologies)
            {
                if(possibleOption == Tech)
                {
                    PossibleOptions.Add(possibleOption);
                }
            }
        }
        DropDownObject.GetComponent<Dropdown>().AddOptions(PossibleOptions);
    }

    //resets display information for tile edit menu
    public void resetEditDisplayInfo()
    {
        //gets the text of the name of the possible tile that is selected in dropdown
        //string selectedEditOptionText = DropDownObject.GetComponent<Dropdown>().captionText.ToString();
        int referencerr = DropDownObject.GetComponent<Dropdown>().value;
        List<string> tempList = new List<string> { DropDownObject.GetComponent<Dropdown>().options.ToString() };
        string selectedEditOptionText = DropDownObject.GetComponent<Dropdown>().captionText.text.ToString();

        //provided customTile data in Resources folder exists for that option, get data and update for edit display
        if (File.Exists(Application.dataPath + "\\Resources\\" + selectedEditOptionText + "L1.Asset") == true)
        {
            customTile currentEditSelectedTile = Resources.Load<customTile>(selectedEditOptionText + "L1");
            //updates info on edit display to show info of selected possible tile
            tempEditCost.text = currentEditSelectedTile.CreditCost.ToString();
            tempEditOutput.text = currentEditSelectedTile.Output.ToString();
            tempEditSeq.text = currentEditSelectedTile.Sequestration.ToString();
            tempEditTileImage.GetComponent<SpriteRenderer>().sprite = currentEditSelectedTile.m_AnimatedSprites[0];
        }
        else
        {
            Debug.Log("customTile information for tile in resource folder under name: " + selectedEditOptionText + "L1 could not be found.");
        }
    }

    //if the x is hit, which destroys building for limited return credits (75%)
    public void ScroungeTileChange()
    {
        Debug.Log("Scrounge!!");
        //adds 75% of tile's credit value to the round credit tally
        customTile DeletedTile = (customTile)playerTileMap.GetTile(PastSelected);
        CurrentPlayerData.RoundCredits += (int)Math.Round(DeletedTile.CreditCost * 0.75);
        playerTileMap.SetTile(PastSelected, null);

        //PLAY ANIMATION HERE EVENTUALLY WITH COROUTINE PERHAPS?
    }

    //try past selected then attempt reordering function call to do this first
    public void ApplyTileChange()
    {
        //gets change and notes credit cost change (don't worry about seq/output, globalui will sort out)
        customTile NewTile = Resources.Load<customTile>(DropDownObject.GetComponent<Dropdown>().captionText.text.ToString() + "L1");
        CurrentPlayerData.RoundCredits -= NewTile.CreditCost;
        playerTileMap.SetTile(PastSelected, NewTile);
    }
}