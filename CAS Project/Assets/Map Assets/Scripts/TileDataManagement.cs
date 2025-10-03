using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEditor.Timeline.Actions;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.Tilemaps;
using UnityEditor;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine.SceneManagement;
using UnityEngine.Windows.Speech;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal.Internal;
using Unity.Collections;



public class TileDataManagement : MonoBehaviour
{
    //reference for the player's tilemap (including all edits and all structures that contribute to Seq/output)
    [SerializeField]
    Tilemap playerTileMap;

    [SerializeField]
    GameObject playerData;

    [SerializeField]
    GameObject MapGrid;

    //a custom structure/type of information for tile information
    public struct TileInformation
    {
        //sets types
        public string StructureType;
        public int Level;

        public TileInformation(string structureType, int level)
        {
            StructureType = structureType;
            Level = level;
        }

        //VERY IMPORTANT, overrides normal type of TileInformation as TileDataMangagement+TileInformation to be string...
        public override string ToString()
        {
            return $"StructureType: {StructureType}, Level: {Level}";
        }
    }


    //dictionary of vector3int for grid reference adn TileInformation for other tile info for standard tile data for new files
    public Dictionary<Vector3Int, TileInformation> StandardtileData = new Dictionary<Vector3Int, TileInformation>();

    //current dictionary for player 
    public Dictionary<Vector3Int, TileInformation> CurrentTileData = new Dictionary<Vector3Int, TileInformation>();

    //dictionary for the types of tiles 
    //    public Dictionary<int, string> TileTypes = new Dictionary<int, string>();

    //for setup function and for coordinates in tile pasting
    int x = 0;
    int y = 0;

    public string player_file_name;
    public string tile_file_reference;

    public int CurrentTotalSeq;
    public int CurrentTotalOutput;
    public int CurrentTotalCreditCost;
    public int PastCreditCost;

    private int Round;



    //0 is player name, 1 is player city, 2 is round, 3 is credits, 4 is output, 5 is seq
    private string[] CurrentPlayerData = new string[5];



    private void Start()
    {
        ResetStandardTileMap();
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "Map")
        {
            //reads and sets all currently lited tile types to a list for reference
            //gets player information to set player name to reference applay selected tile map
            string[] player_information = new string[10];
            var raw = File.ReadLines(Application.dataPath + "\\saves\\Current_File.txt");
            int l = 0;
            foreach (string line in raw)
            {
                player_information[l] = line;
                l++;
            }
            player_file_name = player_information[0];
            tile_file_reference = Application.dataPath + "\\Tile Saves\\TileData" + player_file_name + ".txt";
            Debug.Log(player_file_name + "   " + tile_file_reference);
            Round = int.Parse(player_information[2]);
            //if tile file exists (was an old file) then apply those tile edits, if not make a new one
            if (File.Exists(tile_file_reference) == true)
            {
                ApplySelectedTileMap();
            }
            else
            {
                NewFileTileDataSetup(player_file_name);
            }
            UpdateGlobalInfo();
        }
    }

    //flunction that pastes tile data in file to tilemap
    public void ApplySelectedTileMap()
    {
        var imbadatthis = false;
        while (imbadatthis == false)
        {
            if (MapGrid.activeSelf == true)
            {
                imbadatthis = true;

                var currentTileData = File.ReadLines(tile_file_reference);
                foreach (string tile in currentTileData)
                {
                    string[] SeperatedTileData;
                    SeperatedTileData = Regex.Split(tile, @"\D+");
                    int x = int.Parse(SeperatedTileData[1]);
                    int y = int.Parse(SeperatedTileData[2]);

                    //sets values for grid reference, tile type, level, output and seq based on tile info in player data
                    Vector3Int tempTilePosition = new Vector3Int(x, y, 0);
                    int tempLevel = int.Parse(SeperatedTileData[4]);

                    //funny way to find tile type string
                    string[] lp = tile.ToString().Split(' ');
                    string tempTileType = lp[4].Substring(0, lp[4].Length - 1);
                    if (File.Exists(Application.dataPath + "\\Resources\\" + tempTileType + "L" + tempLevel + ".Asset") == true)
                    {
                        //loads the tile data from the file and pastes it at the temp file position. 
                        var tempTileData = Resources.Load<customTile>(tempTileType + "L" + tempLevel);
                        //  Debug.Log(tempTileData);
                        customTile tempTile = tempTileData;
                        //                           Debug.Log(tempTile.StructureType);
                        playerTileMap.SetTile(tempTilePosition, tempTile);
                        //ExistingTileDataCollection(playerTileMap, tempTilePosition);
                    }
                    else
                    {
                        Debug.Log("Could not find tile file with tag " + SeperatedTileData[4] + " and name " + SeperatedTileData[4]);
                    }
                }

            }
            else
            {
                Debug.Log("map not real");
            }
        }
    }

    //is capable of reading the data from existing custom files at some reference and in some map
    //not needed, is in TileMapInteractivity in an more developed form just here for reference
    public void ExistingTileDataCollection(Tilemap Specifiedtilemap, Vector3Int gridReference)
    {
        Debug.Log("inside data collection");
        customTile specificTile = (customTile)Specifiedtilemap.GetTile(gridReference);
        Debug.Log(gridReference + " level " + specificTile.Level + " seq " + specificTile.Sequestration + " type " + specificTile.StructureType + " output " + specificTile.Output + "education" + specificTile.Education);
    }

    //practice function that sets up the standard tile map (can adjust for size of map, have to manually include the desired tile information and delete unwanted ones)
    private void ResetStandardTileMap()
    {
        //adjust the x/y maxes to desired map size
        while (x < 11)
        {
            while (y < 11)
            {
                //manually change this information in the file itself, cannot here
                StandardtileData[new Vector3Int(x, y, 0)] = new TileInformation("Windmill", 1);
                y++;
            }
            x++;
            y = 0;
        }
        //writes StandardTileData to the file that has been cleared
        File.WriteAllText(Application.dataPath + "\\Tile Saves\\StandardTileData.txt", string.Empty);
        using (StreamWriter standardTileData = File.AppendText(Application.dataPath + "\\Tile Saves\\StandardTileData.txt"))
        {
            foreach (KeyValuePair<Vector3Int, TileInformation> Tile in StandardtileData)
            {
                standardTileData.WriteLine(Tile);
            }
        }

    }

    //Creates new file in name of new player with standard tile data 
    //IMPORTANT use parts of this function as the basis for reading tile data and pasting to grid map in scene load...
    public void NewFileTileDataSetup(string player_name)
    {
        CurrentTileData.Clear();
        //gets a long var[] of all lines in the standard tile data
        var DataList = File.ReadLines(Application.dataPath + "\\Tile Saves\\StandardTileData.txt");
        //loops through each line
        foreach (string line in DataList)
        {
            //opens
            string[] linenumbers;
            linenumbers = Regex.Split(line, @"\D+");

            int a = int.Parse(linenumbers[1]);
            int b = int.Parse(linenumbers[2]);
            Vector3Int s = new Vector3Int(a, b, 0);

            //the 0-x-1th letters of the 4th substring of the line divided by spaces is the name
            string[] lp = line.ToString().Split(' ');
            string c = lp[4].Substring(0, lp[4].Length - 1);

            //5 = level
            Debug.Log(linenumbers[4]);
            int d = int.Parse(linenumbers[4]);
            Debug.Log(s + " " + c + " " + d);
            CurrentTileData[s] = new TileInformation(c.ToSafeString(), d);
            CurrentTileData[s] = new TileInformation(c, d);
            //Array.Clear(linenumbers, 0, linenumbers.Length);
        }
        foreach (KeyValuePair<Vector3Int, TileInformation> Tile in CurrentTileData)
        {
            using (StreamWriter newFile = File.AppendText(Application.dataPath + "\\Tile Saves\\TileData" + player_name + ".txt"))
            {
                newFile.WriteLine(Tile);
            }
        }
        ApplySelectedTileMap();
    }

    public void UpdateGlobalInfo()
    {
        //string reference = Application.dataPath + "\\Tile Saves\\TileData" + player_Name + ".txt";
        //var rawPastGlobalData = File.ReadLines(Application.dataPath + "\\saves\\Current_File.txt");
        CurrentPlayerData[2] = Round.ToString();
        CurrentTotalCreditCost = 0;
        CurrentTotalSeq = 0;
        CurrentTotalOutput = 0;

        int x1 = 0;
        int y1 = 0;
        while (x1 < 100)
        {
            while (y1 < 100)
            {
                if (playerTileMap.HasTile(new Vector3Int(x1, y1, 0)) && playerTileMap.GetTile(new Vector3Int(x1, y1, 0)).GetType() == typeof(customTile))
                {
                    customTile retrievedTile = (customTile)playerTileMap.GetTile(new Vector3Int(x1, y1, 0));
                    CurrentTotalSeq = CurrentTotalSeq + retrievedTile.Sequestration;
                    CurrentTotalOutput = CurrentTotalOutput + retrievedTile.Output;
                    CurrentTotalCreditCost = CurrentTotalCreditCost + retrievedTile.CreditCost;
                }
                y1++;
            }
            x1++;
            y1 = 0;
        }
        //0 is player name, 1 is player city, 2 is round, 3 is credits, 4 is output, 5 is seq
        CurrentPlayerData[3] = (100 + 1.5 * CurrentTotalOutput).ToSafeString();
        Debug.Log("Sums (seq, out, credit): " + CurrentTotalSeq + " " + CurrentTotalOutput + " " + CurrentTotalCreditCost);
        Debug.Log(CurrentTotalCreditCost - PastCreditCost);
        PastCreditCost = CurrentTotalCreditCost;
    }
}