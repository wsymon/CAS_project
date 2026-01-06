using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using System.Linq;
using System.IO.Compression;
using System.Numerics;
public class TileDataManagement : MonoBehaviour
{
    //reference for the player's tilemap (including all edits and all structures that contribute to Seq/output)
    [SerializeField]
    Tilemap playerTileMap;

    [SerializeField]
    Tilemap ConstructionTileMap;

    //player data object, important
    [SerializeField]
    GameObject playerData;

    [SerializeField]
    GameObject MapGrid;

    [SerializeField]
    customTile constructionTile;


    //a custom structure/type of information for tile information
    public struct TileInformation
    {
        //sets types
        public string StructureType;
        public int Level;
        public int ConstructionTimeRemaining;

        public TileInformation(string structureType, int level, int constructionTimeRemaining)
        {
            StructureType = structureType;
            Level = level;
            ConstructionTimeRemaining = constructionTimeRemaining;

        }

        //VERY IMPORTANT, overrides normal type of TileInformation as TileDataMangagement+TileInformation to be string...
        public override string ToString()
        {
            return $"StructureType: {StructureType}, Level: {Level}, {ConstructionTimeRemaining}";
        }
    }


    //dictionary of vector3int for grid reference adn TileInformation for other tile info for standard tile data for new files
    public Dictionary<Vector3Int, TileInformation> StandardtileData = new Dictionary<Vector3Int, TileInformation>();

    //current dictionary for player 
    public Dictionary<Vector3Int, TileInformation> CurrentTileData = new Dictionary<Vector3Int, TileInformation>();

    //dictionary for the construction time left on tiles
    public Dictionary<Vector3Int, int> TileConstructionTimeData = new Dictionary<Vector3Int, int>();

    public string player_file_name;
    public string tile_file_reference;

    public int CurrentTotalSeq;
    public int CurrentTotalOutput;
    public int CurrentTotalCreditCost;
    public int PastCreditCost;
    public int CurrentTotalCarbonCost;

    public int Round;

    //solely for updating player (not tile) file at changes/end of scene
    private string tech;
    private string data;


    private void Awake()
    {
        if (SceneManager.GetActiveScene().name == "Map")
        {

            //reads and sets all currently listed tile types to a list for reference
            //gets player information to set player name to reference apply selected tile map
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
        }
    }

    //function that pastes tile data in file to tilemap
    public void ApplySelectedTileMap()
    {
        //silly var solely to ensure map tilemap exists when called, previously would occur prior to initialisation
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
                        customTile tempTile = Resources.Load<customTile>(tempTileType + "L" + tempLevel);;
                        playerTileMap.SetTile(tempTilePosition, tempTile);

                        if(int.Parse(SeperatedTileData[5]) > 0)
                        {
                            ConstructionTileMap.SetTile(tempTilePosition, constructionTile);
                            TileConstructionTimeData.Add(tempTilePosition, int.Parse(SeperatedTileData[5]));
                        }

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
        customTile specificTile = (customTile)Specifiedtilemap.GetTile(gridReference);
        Debug.Log(gridReference + " level " + specificTile.Level + " seq " + specificTile.Sequestration + " type " + specificTile.StructureType + " output " + specificTile.Output + "education" + specificTile.Education + specificTile.ConstructionTimeRemaining);
    }

    //Creates new file in name of new player with standard tile data 
    public void NewFileTileDataSetup(string player_name)
    {
        CurrentTileData.Clear();
        //gets a long var[] of all lines in the standard tile data
        var DataList = File.ReadLines(Application.dataPath + "\\Tile Saves\\StandardTileData.txt");

        //loops through each line
        foreach (string line in DataList)
        {
            string[] linenumbers;
            linenumbers = Regex.Split(line, @"\D+");

            int a = int.Parse(linenumbers[1]);
            int b = int.Parse(linenumbers[2]);
            Vector3Int s = new Vector3Int(a, b, 0);

            //the 0-x-1th letters of the 4th substring of the line divided by spaces is the name
            string[] lp = line.ToString().Split(' ');
            string c = lp[4].Substring(0, lp[4].Length - 1);

            //5 = level
            int d = int.Parse(linenumbers[4]);
            int e = int.Parse(linenumbers[5]);  //this is construction time...

            CurrentTileData[s] = new TileInformation(c.ToSafeString(), d, e);
            CurrentTileData[s] = new TileInformation(c, d, e);
        }
        foreach (KeyValuePair<Vector3Int, TileInformation> Tile in CurrentTileData)
        {
            using (StreamWriter newFile = File.AppendText(Application.dataPath + "\\Tile Saves\\TileData" + player_name + ".txt"))
            {
                Debug.Log(Tile);
                newFile.WriteLine(Tile);
            }
        }
        ApplySelectedTileMap();
    }

    //updates values for totals of tilemap to be accessed by playerdata for end screen calculations
    public void TileInfoCollection()
    {
        CurrentTotalSeq = 0;
        CurrentTotalOutput = 0;
        CurrentTotalCarbonCost = 0;

        int x1 = 0;
        int y1 = 0;
        while (x1 < 100)
        {
            while (y1 < 100)
            {
                if (playerTileMap.HasTile(new Vector3Int(x1, y1, 0)) && ConstructionTileMap.HasTile(new Vector3Int(x1, y1, 0)) == false)
                {
                    customTile retrievedTile = (customTile)playerTileMap.GetTile(new Vector3Int(x1, y1, 0));
                    CurrentTotalSeq = CurrentTotalSeq + retrievedTile.Sequestration;
                    CurrentTotalOutput = CurrentTotalOutput + retrievedTile.Output;
                    CurrentTotalCreditCost = CurrentTotalCreditCost + retrievedTile.CreditCost;
                    CurrentTotalCarbonCost = CurrentTotalCarbonCost + retrievedTile.CarbonCost;
                }
                y1++;
            }
            x1++;
            y1 = 0;
        }
    }


    //now defunct, but collects all data from the tilemap and CurrentPlayerData and writes to both tile, player and current files.
    //as no more saving during Rounds (only at round end), a modified version of this now occurs within GlobalUIScript...
    public void UpdateTileAndPlayerFileData()
    {
        //alter bounds later to match that of the real map
        int x = 0;
        int y = 0;
        Vector3Int Pos;

        //clears current tile data/player to refill it
        CurrentTileData.Clear();
        tech = "";

        //loops through entire map and adds tiles within playertile to an array
        //ADD SIZE OF MAP HERE LATER
        while(x < 100)
        {
            while(y < 100)
            {
                Pos =  new Vector3Int(x, y, 0);
                if (playerTileMap.HasTile(Pos))
                {
                    customTile ATile = playerTileMap.GetTile<customTile>(Pos);
                    if (ConstructionTileMap.HasTile(Pos))
                    {
                        CurrentTileData[Pos] = new TileInformation(ATile.StructureType, ATile.Level, TileConstructionTimeData[Pos]);
                    }
                    else
                    {
                        CurrentTileData[Pos] = new TileInformation(ATile.StructureType, ATile.Level, 0);
                    }
                }
                y++;
            }
            y = 100;
            x++;
        }

        //clears player tile file...
        File.WriteAllText(Application.dataPath + "\\Tile Saves\\TileData" + CurrentPlayerData.Name + ".txt", "");

        //writes array to player tile file
        foreach (KeyValuePair<Vector3Int, TileInformation> Tile in CurrentTileData)
        {
            using (StreamWriter newFile = File.AppendText(Application.dataPath + "\\Tile Saves\\TileData" + CurrentPlayerData.Name + ".txt"))
            {
                newFile.WriteLine(Tile);
            }
        }

        foreach(string developedTech in CurrentPlayerData.DevelopedTechnologies)
        {
            tech += " " + developedTech;
        }
        data = CurrentPlayerData.Name + "\n" + CurrentPlayerData.CityName + "\n" + CurrentPlayerData.Round + "\n0\n" + CurrentPlayerData.RoundCredits.ToString() + "\n0, 0, 0\n" + tech;

        x = 0;
        while(x < 20)
        {
            if (File.Exists(Application.dataPath + "\\Saves\\Save" + x + ".txt"))
            {
                string nametest = File.ReadLines(Application.dataPath + "\\Saves\\Save" + x + ".txt").FirstOrDefault();
                if(nametest == CurrentPlayerData.Name)
                {
                    File.WriteAllText(Application.dataPath + "\\Saves\\Save" + x + ".txt", data);
                    File.WriteAllText(Application.dataPath + "\\Saves\\Current_File.txt", data);
                    x = 21;
                }
            }   
            x++;
        }
        data = "";
    }
}