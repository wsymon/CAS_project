using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEditorInternal;
using UnityEngine;

[System.Serializable]
public class CurrentPlayerData  : MonoBehaviour
{
    //defines for data associated with player (generalised to be fetched into "current player" from player save specified when transitioning to Map scene
    //which are Round/Subround (3-5 subs per round) (round number (different file for associated carbon level and energy production), Research (dictionary of development of different technologies), 
    //Name (to be used in credits/title screen identification) and CityName (for name of city for funsies), Credits (current spending capacity essentially, max/gain specified by round function)

    //static here indicates the variables will presist between rounds and the round summary scenes
    public static int Round;
    public static string Name;
    public static string CityName; 
    public static int ChangeInCredits;
    public static int RoundCredits;
    public static int TotalSequestration;
    public static int SequestrationGoal;
    public static int CarbonCost;
    public static int TotalOutput;
    public static int OutputGoal;
    public static string[] DevelopedTechnologies;

    //dictionary of player edits in tilemap, dictionary with 2 parts to it; the vector3Int of the tile's coordinates in the map, and a string dictionary of related details of the tile (solar_panel, lvl1, ect)

    //Dictionary<int, string[]> tilemapPlayerEdits = new Dictionary<int, string[]>(); defining the parts of the dictionary, find way to set to values in specified player file

    //public PlayerDataFetch(Player player){  }

    public void UpdateGlobalVariables(int round, string name, string cityname, int changeincredits, int roundCredits, int currentseq, int goalseq, int carboncost, int currentouput, int goaloutput)
    {
        Round = round;
        Name = name;
        CityName = cityname;
        ChangeInCredits = changeincredits;
        RoundCredits = roundCredits;
        TotalSequestration = currentseq;
        SequestrationGoal = goalseq;
        CarbonCost = carboncost;
        TotalOutput = currentouput;
        OutputGoal = goaloutput;
    } 
    public void UpdateDevelopedTechnologies(string technology)
    {
        DevelopedTechnologies.Append(technology);
    }
}