using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;
using Unity.Mathematics;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileDataManagement : MonoBehaviour
{
    //reference for the player's tilemap (including all edits and all structures that contribute to Seq/output)
    [SerializeField]
    Tilemap playerTileMap;

    [SerializeField]
    GameObject playerData;


    //a custom structure/type of information for tile information
    public struct TileInformation
    {
        //sets types
        public int StructureType;
        //public Tile TileSource;
        public int Level;
        public int Output;
        public int Sequestration;

        //sets name and reference
        public TileInformation(int structureType, int level, int output, int sequestration)
        {
            StructureType = structureType;
            Level = level;
            Output = output;
            Sequestration = sequestration;
        }

        //VERY IMPORTANT, overrides normal type of TileInformation as TileDataMangagement+TileInformation to be string...
        public override string ToString()
        {
            return $"StructureType: {StructureType}, Level: {Level}, Output: {Output}, Sequestration: {Sequestration}";
        }
    }


    //dictionary of vector3int for grid reference adn TileInformation for other tile info for standard tile data for new files
    public Dictionary<Vector3Int, TileInformation> StandardtileData = new Dictionary<Vector3Int, TileInformation>();

    //current dictionary for player 
    public Dictionary<Vector3Int, TileInformation> CurrentTileData = new Dictionary<Vector3Int, TileInformation>();

    //dictionary for the types of tiles 
    public Dictionary<int, string> TileTypes = new Dictionary<int, string>();

    //for setup function and for coordinates in tile pasting
    int x = 0;
    int y = 0;

    public string player_file_name;
    public string tile_file_reference;



    private void Start()
    {
        var types = File.ReadLines(Application.dataPath + "\\Tile Saves\\TileTypes.txt");
        foreach (string type in types)
        {
            string[] TileTypeTempList;
            TileTypeTempList = type.Split(",");
            TileTypes[int.Parse(TileTypeTempList[0])] = TileTypeTempList[1];
        }


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

        //if tile file exists (was an old file) then apply those tile edits, if not make a new one
        if (File.Exists(tile_file_reference) == true)
        {
            ApplySelectedTileMap(player_file_name);
        }
        else
        {
            NewFileTileDataSetup(player_file_name);
        }
    }

    //flunction that pastes tile data in file to tilemap
    public void ApplySelectedTileMap(string player_file_name)
    {
        var currentTileData = File.ReadLines(tile_file_reference);
        foreach (string tile in currentTileData)
        {
            string[] SeperatedTileData;
            SeperatedTileData = Regex.Split(tile, @"\D+");
            int x = int.Parse(SeperatedTileData[1]);
            int y = int.Parse(SeperatedTileData[2]);

            //sets values for grid reference, tile type, level, output and seq based on tile info in player data
            Vector3Int tempTilePosition = new Vector3Int(x, y, 0);
            string tempTileType = TileTypes[int.Parse(SeperatedTileData[4])];
            int tempLevel = int.Parse(SeperatedTileData[5]);
            int tempOutput = int.Parse(SeperatedTileData[6]);
            int tempSeq = int.Parse(SeperatedTileData[7]);

            //playerTileMap.SetTile(tempTilePosition, );
            //setTile(null) = destroy a tile 
        }
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
                StandardtileData[new Vector3Int(x, y, 0)] = new TileInformation(1, 1, 100, 200);
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

            int c = int.Parse(linenumbers[4]);
            int d = int.Parse(linenumbers[5]);
            int e = int.Parse(linenumbers[6]);
            int f = int.Parse(linenumbers[7]);

            CurrentTileData[s] = new TileInformation(c, d, e, f);
            //Array.Clear(linenumbers, 0, linenumbers.Length);
        }
        foreach (KeyValuePair<Vector3Int, TileInformation> Tile in CurrentTileData)
        {
            using (StreamWriter newFile = File.AppendText(Application.dataPath + "\\Tile Saves\\TileData" + player_name + ".txt"))
            {
                newFile.WriteLine(Tile);
            }
        }
        ApplySelectedTileMap(player_name);
    }
}
//Assets/Map/PC Computer - Omori - Snowglobe Mountain_XXX.asset 

//if (tilemapData.TryGetValue(position, out TileData tileData))