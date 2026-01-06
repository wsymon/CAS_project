using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using Unity.VisualScripting;
using System;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System.Collections;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal;

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
    Tilemap ConstructionTileMap;

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
    Image InfoMenuBacking;

    [SerializeField]
    Image EditMenuBacking;

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
    Button EditConfirmButton;

    [SerializeField]
    GameObject PopupBackingObject;

    [SerializeField]
    GameObject ScroungePopUpObject;

    [SerializeField]
    Button ScroungeTileButton;

    [SerializeField]
    GameObject GlobalUIObject;

    [SerializeField]
    GameObject DevelopmentUIObject;

    [SerializeField]
    GameObject DevelopmentUIBUtton;

    [SerializeField]
    GameObject ExitAndTitleMenuObject;

    [SerializeField]
    GameObject DevelopmentPopUpObject;

    //vectors for past/current selected tile and current/past tile mouse is on 
    public Vector3Int PastSelected = new Vector3Int(0, 0, 0);
    public Vector3Int SelectedGrid = new Vector3Int(0, 0, 0);
    private Vector3Int PastGrid = new Vector3Int(0, 0, 0);
    private Vector3Int CurrentGrid = new Vector3Int(0, 0, 0);
    private List<string> TileEditPossibilities;

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
        else if(TileInformationMenuObject.activeSelf == false && TileEditMenuObject.activeSelf == false && PopupBackingObject.activeSelf == false && DevelopmentUIObject.activeSelf == false)
        {
            //every frame update the highlights based on curretn grid position
            HighlightUpdate(CurrentGrid);
        }
    }

    //click logic with some hightlight stuff mixed in
    private void LeftClick(Vector3Int SelectedGrid)
    {
        if(PopupBackingObject.activeSelf == false && DevelopmentUIObject.activeSelf == false && ExitAndTitleMenuObject.activeSelf == false)
        {   
            if (EventSystem.current.IsPointerOverGameObject() == false)
            {
                //if click was on the same spot 
                if (SelectedGrid == PastSelected)        //
                {
                    //if both menu is open, shut menus
                    if(TileEditMenuObject.activeSelf == true && TileInformationMenuObject.activeSelf == true)
                    {
                        TileEditMenuCloser();
                        TileInfoMenuCloser();
                        HighlightUpdate(SelectedGrid);
                        TileHighlightsTileMap.SetTile(SelectedGrid, Hover);
                    }
                    //if only edit menu open, shut it
                    else if (TileEditMenuObject.activeSelf == true)
                    {
                        TileEditMenuCloser();
                        HighlightUpdate(SelectedGrid);
                        TileHighlightsTileMap.SetTile(SelectedGrid, Hover);
                    }
                    //if only info opne, close it
                    else if(TileInformationMenuObject.activeSelf == true)
                    {
                        TileInfoMenuCloser();
                        HighlightUpdate(SelectedGrid);
                        TileHighlightsTileMap.SetTile(SelectedGrid, Hover);
                    }

                    //click on same spot and no menu open (was therefor shut previously), reopen menu
                    else
                    {
                        //if construction in progress, open info solely with that  (MUST be this order in case of scrounge and player tile map empty but construction not)
                        if (ConstructionTileMap.HasTile(SelectedGrid))
                        {
                            InfoMenuUpdaterandOpener(SelectedGrid, ConstructionTileMap);
                            TileInformationMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
                            TileHighlightsTileMap.SetTile(SelectedGrid, Hover);
                        }
                        //if no construction but existing player tile
                        else if (playerTileMap.HasTile(SelectedGrid))
                        {
                            InfoMenuUpdaterandOpener(SelectedGrid, playerTileMap);
                            EditMenuUpdaterandOpener(SelectedGrid);
                            TileInformationMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
                            TileHighlightsTileMap.SetTile(SelectedGrid, Hover);
                        }
                        //if no player (and no construction), then 
                        else
                        {
                            InfoMenuUpdaterandOpener(SelectedGrid, BaseTileMap);
                            EditMenuUpdaterandOpener(SelectedGrid);
                            TileInformationMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
                            TileHighlightsTileMap.SetTile(SelectedGrid, Hover);
                        }
                    }
                }
                //if clicked on a different tile
                else
                {
                    TileHighlightsTileMap.SetTile(PastSelected, null);
                    TileHighlightsTileMap.SetTile(SelectedGrid, Hover);

                    //if a construction from the same round is still progressing, show that instead of normal/base info
                    if (ConstructionTileMap.HasTile(SelectedGrid))
                    {
                        //if info menu open, just update it
                        if(TileInformationMenuObject.activeSelf == true)
                        {
                            InfoMenuUpdaterandOpener(SelectedGrid, ConstructionTileMap);
                        }
                        //if shut, then opening animation
                        else
                        {
                            TileInformationMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
                            InfoMenuUpdaterandOpener(SelectedGrid, ConstructionTileMap);
                        }

                        //if construction tile and edit menu open for previous tile, close animation the menu
                        if(TileEditMenuObject.activeSelf == true)
                        {
                            TileEditMenuCloser();
                        }
                    }

                    //if player tile empty but applicable option at new selected point, call menu updater for the player tile 
                    else if (playerTileMap.HasTile(SelectedGrid))
                    {
                        InfoMenuUpdaterandOpener(SelectedGrid, playerTileMap);
                        //if shut, update and animate opening of it
                        if(TileInformationMenuObject.activeSelf != true)
                        {
                            TileInformationMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
                        }

                        //edit menu opener covers viability and animation, no concern here
                        EditMenuUpdaterandOpener(SelectedGrid);
                    }

                    //if a base tile is applicable (no player tile nor construciton there), call menu updater for base tile
                    else
                    {
                        InfoMenuUpdaterandOpener(SelectedGrid, BaseTileMap);
                        
                        //if not open, play animation
                        if(TileInformationMenuObject.activeSelf != true)
                        {
                            TileInformationMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
                        }
                        TileHighlightsTileMap.SetTile(PastSelected, null);
                        EditMenuUpdaterandOpener(SelectedGrid);
                    }
                    PastSelected = SelectedGrid;
                }
            }
            else
            {
               // Debug.Log("menu open and clikc inside it");
            }
            
 
        }
        else
        {
           // Debug.Log("clicked while dev/importna menu open");
        }
    }

    //updates the animated tile for tile highlighter
    private void HighlightUpdate(Vector3Int currentGrid)
    {
        //provided menu is not open, do the normal followery thing (delete past and make present new)
        if (TileInformationMenuObject.activeSelf == false && TileEditMenuObject.activeSelf == false)
        {
            if (currentGrid != PastGrid)
            {
                TileHighlightsTileMap.SetTile(currentGrid, Hover);
                TileHighlightsTileMap.SetTile(PastGrid, null);
            }
        }
        //if menu was true just continously make the current SelectedGrid highlighted and remove all others. 
        //probably redundant with the similar logic in the LeftClick() function, but just in case 
        else
        {
            if (TileHighlightsTileMap.GetTile(SelectedGrid) != Hover)
            {
                TileHighlightsTileMap.SetTile(SelectedGrid, Hover);
            }
            TileHighlightsTileMap.SetTile(PastSelected, null);
        }
        //resets old/new status
        PastGrid = currentGrid;
    }

    //closes the info/edit menus respectively
    public void TileInfoMenuCloser()
    {
        if(TileEditMenuObject.activeSelf == false)
        {
            TileHighlightsTileMap.SetTile(PastSelected, null);
        }
    //    StartCoroutine(UIAnimationClosingDelay(TileInformationMenuObject));                   ??
        TileInformationMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
        TileHighlightsTileMap.SetTile(SelectedGrid, null);
    }
    public void TileEditMenuCloser()
    {
        if(TileInformationMenuObject.activeSelf == false)
        {
            TileHighlightsTileMap.SetTile(PastSelected, null);
        }
       // StartCoroutine(UIAnimationClosingDelay(TileEditMenuObject));  ///??                   ??
        TileEditMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
        //DropDownObject.SetActive(false);
        TileHighlightsTileMap.SetTile(SelectedGrid, null);
    }

    IEnumerator UIAnimationClosingDelay(GameObject target)
    {
        float elapsedtime = 0;
        while(elapsedtime < 0.26f)
        {
            elapsedtime += Time.deltaTime;
            yield return null;
        }
        target.SetActive(false);
    }

    //logic and updates ((BUT NOT OPENING DESPITE THE NAME) to info tile menu
    public void InfoMenuUpdaterandOpener(Vector3Int currentGrid, Tilemap specificTileMap)
    {
        //the three universal things that need to happen across all possibilities 
        Image realimage = InfoDisplayofTile.GetComponent<Image>();
        EducationInfo.GetComponent<InfoPageManager>().ResetButtons();

        //if a construction tile from the same round is there
        if (specificTileMap == ConstructionTileMap)
        {
            realimage.sprite = ConstructionTileMap.GetSprite(currentGrid);

            //if there is a player tile under the construction tile (i.e the player made a construction of something)
            if(playerTileMap.GetTile(currentGrid) != null)
            {
                customTile specificTile = (customTile)playerTileMap.GetTile(currentGrid);
                EditCurrentTileName.text = specificTile.StructureType.ToSafeString();
                EducationInfo.text = "(Under construction, completed in " + playerData.GetComponent<TileDataManagement>().TileConstructionTimeData[currentGrid] + " rounds). "  + specificTile.Education;
                TileInforSeqDisplay.text = specificTile.Sequestration.ToString();
                TileInfoOutputDisplay.text = specificTile.Output.ToString();
            }
            //if there is no player tile under the construction tile (i.e the player scrounged the tile there previously)
            else
            {   
                EducationInfo.text = "(Under construction)";
                TileInforSeqDisplay.text = " - ";
                TileInfoOutputDisplay.text = " - ";
                EditCurrentTileName.text = "Deconstruction Site";
            }
        }

        //there is no constructon tile, player clicked on a player tile, this updataes info and edit menu based on base tile possibilities 
        else
        {
            customTile specificTile = (customTile)specificTileMap.GetTile(currentGrid);
            //if there is an error at 350, ignore as no impact on functonality and will not exist once map is entirely composed of customTiles. (caused by empty base tile)
            if(specificTile.GetTileSprite(specificTile) != null)
            {
                Sprite tileInfoDisplay = specificTile.GetTileSprite(specificTile);
                realimage.sprite = tileInfoDisplay;
            }
            
            EducationInfo.text = specificTile.Education;
            TileInforSeqDisplay.text = specificTile.Sequestration.ToString();
            TileInfoOutputDisplay.text = specificTile.Output.ToString();
            EditCurrentTileName.text = specificTile.StructureType.ToSafeString();
      //      if (BaseTileMap.GetTile(currentGrid) is customTile)
       //     {
        //        customTile BaseTile = (customTile)BaseTileMap.GetTile(currentGrid);
         //       if (BaseTile.SuitableTileTypePlacements.Count() != 0)
          //      {
           //         TileEditMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
            //        resetDropDown(currentGrid);
             //       resetEditDisplayInfo();
              //      DropDownObject.SetActive(true);
               // }
            //}

               //theoretically edit menu accounts for this...
        }
    }

    //logic, info and updates to edit menu for tiles 
    public void EditMenuUpdaterandOpener(Vector3Int currentGrid)
    {
        //if base tile at clicked position is customtile, will always be the case once completed but for interim period 
        if (BaseTileMap.GetTile(currentGrid) is customTile)
        {
            //if the tile at that position has editable possibilities then open edit menu
            customTile BaseTile = (customTile)BaseTileMap.GetTile(currentGrid);
            if (BaseTile.SuitableTileTypePlacements != null)
            {
                //all stuff for changing information on edit menu!!
                if(TileEditMenuObject.activeSelf == false)
                {
                    TileEditMenuObject.GetComponent<UIAnimationScript>().UIAnimation();
                }
                resetDropDown(currentGrid);
                resetEditDisplayInfo();     

                //ensures selected options viability matches that presented by the confirm button
                EditMenuConfirmButtonUpdater();  
            }
        }

        //logic for vaibility of scournge button
        if(playerTileMap.HasTile(currentGrid))
        {
            ScroungeTileButton.interactable = true;
            ScroungeTileButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
        }
        else
        {
            ScroungeTileButton.interactable = false;
            ScroungeTileButton.GetComponent<Image>().color = new Color(6f, 185f, 185f, 255f);
        }
    }

    public void EditMenuConfirmButtonUpdater()
    {
        //if credit cost is not payablew
        if(DropDownObject.GetComponent<Dropdown>().options.Count() > 0)
        {
            if(Resources.Load<customTile>(DropDownObject.GetComponent<Dropdown>().options[DropDownObject.GetComponent<Dropdown>().value].text.ToString() + "L1").CreditCost > CurrentPlayerData.RoundCredits || DropDownObject.GetComponent<Dropdown>().options[DropDownObject.GetComponent<Dropdown>().value].text.ToString() == "Not viable")
            {
                //make button inaccessible and make text of it red 
                EditConfirmButton.GetComponent<Image>().color = new Color(6f, 185f, 185f, 255f);
                EditConfirmButton.interactable = false;
            }
            else
            {
                EditConfirmButton.GetComponent<Image>().color = new Color(255, 255, 255, 255);
                EditConfirmButton.interactable = true;
            }
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

        //basically ensures that for times when the player is replacing tiles (at a loss without scrounging) they cannot construct the same tile over itself...
        if (playerTileMap.HasTile(pos))
        {
            string existingOptionToRemove = "";
            customTile existingPlayerTile = (customTile)playerTileMap.GetTile(pos);
            foreach(string option in PossibleOptions)
            {
                if(option == existingPlayerTile.StructureType)
                {
                    existingOptionToRemove = option;
                }
            }
            PossibleOptions.Remove(existingOptionToRemove);
        }

   //     List<string> tempList  = PossibleOptions;
    //    int x = PossibleOptions.Count() - 1;
     //   while(x > -1)
      //  {   
       //     tempList.Remove(PossibleOptions[x]);

            
    //        x--;
     //   }



        //for rare cases where tech options are not developed and sole option (normally windmill) is already on the tile
        if(PossibleOptions.Count() == 0)
        {
            //if rare case, then make dropdown object not visible as no viable otions 
             DropDownObject.GetComponent<Dropdown>().captionText.text = "Not viable";
        }
        else
        {
            //basically when there is a viable option, go ahead and add and keep menu
            foreach(string bit in PossibleOptions)
            {
                Debug.LogWarning(bit);
            }
            DropDownObject.GetComponent<Dropdown>().AddOptions(PossibleOptions);
        }
    }

    //resets display information for tile edit menu
    public void resetEditDisplayInfo()
    {
        //gets the text of the name of the possible tile that is selected in dropdown
        string selectedEditOptionText = DropDownObject.GetComponent<Dropdown>().captionText.text.ToString();
        //provided customTile data in Resources folder exists for that option, get data and update for edit display
        if (File.Exists(Application.dataPath + "\\Resources\\" + selectedEditOptionText + "L1.Asset") == true)
        {
            customTile currentEditSelectedTile = Resources.Load<customTile>(selectedEditOptionText + "L1");
            //updates info on edit display to show info of selected possible tile
            tempEditCost.text = currentEditSelectedTile.CreditCost.ToString();
            tempEditOutput.text = currentEditSelectedTile.Output.ToString();
            tempEditSeq.text = currentEditSelectedTile.Sequestration.ToString();
            tempEditTileImage.GetComponent<Image>().sprite = currentEditSelectedTile.m_AnimatedSprites[0];
        }
    }


    //destroys building for limited return credits (75%)
    public void ScroungeTileChangeConfirm()
    {
        //adds 75% of tile's credit value to the round credit tally
        customTile DeletedTile = (customTile)playerTileMap.GetTile(PastSelected);
        CurrentPlayerData.RoundCredits += (int)Math.Round(DeletedTile.CreditCost * 0.75); //
        playerTileMap.SetTile(PastSelected, null);
        customTile ConstructionTile = Resources.Load<customTile>("ConstructionTile");
        ConstructionTileMap.SetTile(PastSelected, ConstructionTile);

        TileEditMenuCloser();
        InfoMenuUpdaterandOpener(PastSelected, ConstructionTileMap);
        GlobalUIObject.GetComponent<GlobalUI>().UpdateGlobalUI();

    }
    public void ApplyTileChange()
    {
        //gets change and notes credit cost change (don't worry about seq/output, globalui will sort out)
        customTile NewTile = Resources.Load<customTile>(DropDownObject.GetComponent<Dropdown>().captionText.text.ToString() + "L1");
        Debug.Log(NewTile.StructureType);
        playerData.GetComponent<TileDataManagement>().TileConstructionTimeData.Add(PastSelected, NewTile.ConstructionTimeRemaining);
        customTile ConstructionTile = Resources.Load<customTile>("ConstructionTile");
        CurrentPlayerData.RoundCredits -= NewTile.CreditCost;
        playerTileMap.SetTile(PastSelected, NewTile);
        ConstructionTileMap.SetTile(PastSelected, ConstructionTile);

        //sound effect surely?                                                                                                      /
    }
}