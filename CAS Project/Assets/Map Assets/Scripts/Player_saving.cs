using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;

[System.Serializable]
public class Player_saving  : MonoBehaviour
{
    //defines for data associated with player (generalised to be fetched into "current player" from player save specified when transitioning to Map scene
    //which are Round/Subround (3-5 subs per round) (round number (different file for associated carbon level and energy production), Research (dictionary of development of different technologies), 
    //Name (to be used in credits/title screen identification) and CityName (for name of city for funsies), Credits (current spending capacity essentially, max/gain specified by round function)
    public int Round;
    public int SubRound;
    public string[] Research;
    public string Name;
    public string CityName; 
    public int Credits;
    public Dictionary<Vector3Int, string[]> tilemapPlayerEditsLoaded;
    public Vector3Int Time;
    //dictionary of player edits in tilemap, dictionary with 2 parts to it; the vector3Int of the tile's coordinates in the map, and a string dictionary of related details of the tile (solar_panel, lvl1, ect)

    //Dictionary<int, string[]> tilemapPlayerEdits = new Dictionary<int, string[]>(); defining the parts of the dictionary, find way to set to values in specified player file

    //public PlayerDataFetch(Player player){  }
    
}
