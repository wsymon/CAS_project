using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GlobalUI : MonoBehaviour
{
    //file references
    [SerializeField]
    GameObject PlayerData;

    [SerializeField]
    GameObject AnimatedObject;

    //textbox references
    [SerializeField]
    TextMeshProUGUI GlobalCreditText;

    [SerializeField]
    TextMeshProUGUI GlobalRoundText;

    [SerializeField]
    TextMeshProUGUI GlobalSequestrationGoalText;

    [SerializeField]
    TextMeshProUGUI GlobalSequatrionTotalText;

    [SerializeField]
    TextMeshProUGUI GlobalOutputGoalText;

    [SerializeField]
    TextMeshProUGUI GlobalOutputTotalText;
    
    //round end
    [SerializeField]
    Tilemap PlayerTileMap;

    [SerializeField]
    Tilemap constructionTileMap;



    //define
    public Dictionary<Vector3Int, TileDataManagement.TileInformation> CurrentTileData = new Dictionary<Vector3Int, TileDataManagement.TileInformation>();
    private string PlayerName;
    private string PlayerCity;
    private int Round;
    private int RoundCredits;
    private int PreCredits;
    private int ChangeInCredits;
    private int SequestrationCurrent;
    private int SequestrationGoal;
    private int OutputCurrent;
    private int OutputGoal;
    private int CurrentCarbonCost;
    private string[] DevelopedTech;
    private string[] Pre_DevelopedTech;

    private int outputStatus;
    //creating BaseOutput Goal
    private int BaseOutput;

    //current note, sequestration goal is current a constant but output goal scales with round number
    //start just sets up variables within CurrentPlayerData and the UI
    void Start()
    {
        //opens player file and checks for existing round/credit data
        string[] player_information = new string[10];
        var raw = File.ReadLines(UnityEngine.Application.dataPath + "\\saves\\Current_File.txt");
        int l = 0;
        foreach (string line in raw)
        {
            player_information[l] = line;
            l++;
        }


        //solely to have a variable here to record the initial credits to calculate the difference later...
        PreCredits = int.Parse(player_information[4]);
        //reference to get existing tile data
        PlayerData.GetComponent<TileDataManagement>().TileInfoCollection();
        //calculates output from tiles and freezes it
        var tileData = PlayerData.GetComponent<TileDataManagement>();

        tileData.SnapshotOutput = tileData.CurrentTotalOutput;
        tileData.SnapshotSequestration = tileData.CurrentTotalSeq;
        PlayerName = player_information[0];
        PlayerCity = player_information[1];
        Round = int.Parse(player_information[2]);
        RoundCredits = int.Parse(player_information[4]);
        OutputCurrent = PlayerData.GetComponent<TileDataManagement>().SnapshotOutput;
        //apparently freezes starting difficulty so it doesn't change for the first round??
        if (Round <= 1 || BaseOutput == 0)
        {
            BaseOutput = OutputCurrent;
        }
        SequestrationCurrent = PlayerData.GetComponent<TileDataManagement>().SnapshotSequestration;
        OutputGoal = (int)(BaseOutput * Mathf.Pow(1.15f, Round - 1));
        SequestrationGoal = 500;
        CurrentCarbonCost = PlayerData.GetComponent<TileDataManagement>().CurrentTotalCarbonCost;


        Pre_DevelopedTech = player_information[6].Split(' ');
        foreach(string t in Pre_DevelopedTech)
        {
            Debug.Log(t);
        }

        var temporary = new List<string>();
        foreach(string spacelessTech in Pre_DevelopedTech)
        {
            if(File.Exists(Application.dataPath + "\\Resources\\" + spacelessTech + "L1" + ".Asset") == true)
            {
                customTile D = (customTile)Resources.Load(spacelessTech + "L1");
                Debug.Log(D.StructureType);
                if(D.StructureType != null)
                {
                    Debug.Log("null");
                }
                temporary.Add(D.StructureType.ToString());
            }
            else
            {
                Debug.Log("no file foun");
            }
        }
        Pre_DevelopedTech = null;
        DevelopedTech = temporary.Distinct().ToArray();
        temporary = null;

        foreach(string t in DevelopedTech)
        {
            Debug.Log(t);
        }

        //string[] status_line = player_information[7].Split(' ');
        //scrapped method where oyu would get extra credits for fulfilling output for successive rounds (have cut, Anthony didn't use, vestigal organ present in files)

        //if(status_line[1] == "+")
        //{
        //    outputStatus = int.Parse(player_information[7].Split(' ')[0]);
        //}
        //else
        //{
        //    outputStatus = -1 * int.Parse(player_information[7].Split(' ')[0]);
        //}
        int outputStatus = 1;

        //calls functions to update, could could manually but may need to reference from button click...
        PlayerData.GetComponent<CurrentPlayerData>().UpdateGlobalVariables(Round, PlayerName, PlayerCity, 0, RoundCredits, SequestrationCurrent, SequestrationGoal, CurrentCarbonCost, OutputCurrent, OutputGoal, outputStatus);
        
        foreach(string tech in DevelopedTech)
        {
            if(tech != "")
            {
                PlayerData.GetComponent<CurrentPlayerData>().UpdateDevelopedTechnologies(tech);
            }
        }

        //sets UI values to those found
        GlobalRoundText.text = Round.ToString();
        GlobalCreditText.text = RoundCredits.ToString();
        GlobalSequatrionTotalText.text = SequestrationCurrent.ToString();
        GlobalSequestrationGoalText.text = SequestrationGoal.ToString();
        GlobalOutputGoalText.text = OutputGoal.ToString();
        GlobalOutputTotalText.text = OutputCurrent.ToString();
    }

    //updates global UI and stats off a change (tile change for example, or development if later enabled) at end of round from round click...
    public void UpdateGlobalUI()
    {
        PlayerData.GetComponent<TileDataManagement>().TileInfoCollection();
        CurrentPlayerData.TotalSequestration = PlayerData.GetComponent<TileDataManagement>().CurrentTotalSeq;
        CurrentPlayerData.TotalOutput = PlayerData.GetComponent<TileDataManagement>().CurrentTotalOutput;

        GlobalRoundText.text = CurrentPlayerData.Round.ToString();
        GlobalCreditText.text = CurrentPlayerData.RoundCredits.ToString();
        GlobalSequatrionTotalText.text = CurrentPlayerData.TotalSequestration.ToString();
        GlobalOutputTotalText.text = CurrentPlayerData.TotalOutput.ToString();
    }
    
    //does needed things at end of round and opens new scene , including data saving of global and to files
    public void UpdateGlobalRoundEnd()
    {
        Debug.Log("global round end");
        //top one just updates values in other script after scrapping the board
        PlayerData.GetComponent<TileDataManagement>().TileInfoCollection();

        PlayerName = CurrentPlayerData.Name;
        PlayerCity = CurrentPlayerData.CityName;
        Round = CurrentPlayerData.Round;
        RoundCredits = CurrentPlayerData.RoundCredits;
        ChangeInCredits = PreCredits - CurrentPlayerData.RoundCredits;
        OutputCurrent = PlayerData.GetComponent<TileDataManagement>().SnapshotOutput;
        SequestrationCurrent = PlayerData.GetComponent<TileDataManagement>().CurrentTotalSeq;
        OutputGoal = CurrentPlayerData.OutputGoal;
    //calculates new scaling output
        OutputGoal = (int)(BaseOutput * Mathf.Pow(1.15f, Round - 1));
        CurrentPlayerData.OutputGoal = OutputGoal;
        SequestrationGoal = CurrentPlayerData.SequestrationGoal;
        CurrentCarbonCost = PlayerData.GetComponent<TileDataManagement>().CurrentTotalCarbonCost;
        outputStatus = CurrentPlayerData.OutputStatus;
        
        PlayerData.GetComponent<CurrentPlayerData>().UpdateGlobalVariables(Round, PlayerName, PlayerCity, ChangeInCredits, RoundCredits, SequestrationCurrent, SequestrationGoal,CurrentCarbonCost, OutputCurrent, OutputGoal, outputStatus);

        //alter bounds later to match that of the real map
        int x = -1;
        int y = -1;
        //Vector3Int Pos; old line idk

        //loops through entire map and adds tiles within playertile to an array
        while (x < 26)
        {
            while (y < 26)
            {
                Vector3Int Pos = new Vector3Int(x, y, 0);

                if (PlayerTileMap.HasTile(Pos))
                // //if construction tilemap has a tile AND that tile, next round, will still be under construction... (update 15/5 idk how much of this is true im just doing whatever i can try T-T)
                {
                    customTile ATile = PlayerTileMap.GetTile<customTile>(Pos);

                    if (constructionTileMap.HasTile(Pos) &&
                        PlayerData.GetComponent<TileDataManagement>().TileConstructionTimeData[Pos] - 1 > 0)
                    {
                        CurrentTileData[Pos] = new TileDataManagement.TileInformation(
                            ATile.StructureType.Replace(" ",""),
                            ATile.Level,
                            PlayerData.GetComponent<TileDataManagement>().TileConstructionTimeData[Pos] - 1
                        );
                    }
                    else
                    {
                        CurrentTileData[Pos] = new TileDataManagement.TileInformation(
                            ATile.StructureType.Replace(" ",""),
                            ATile.Level,
                            0
                        );
                    }
                }

                y++;
            }
            y = 0;
            x++;
        }

        Debug.Log("file cleared");
        //clears player tile file...
        File.WriteAllText(Application.dataPath + "\\Tile Saves\\TileData" + CurrentPlayerData.Name + ".txt", "");


        Debug.Log("writing now to tile");
        //writes array to player tile file
        foreach (KeyValuePair<Vector3Int, TileDataManagement.TileInformation> Tile in CurrentTileData)
        {
            using (StreamWriter newFile = File.AppendText(Application.dataPath + "\\Tile Saves\\TileData" + CurrentPlayerData.Name + ".txt"))
            {
                Debug.Log(Tile);
                newFile.WriteLine(Tile);
            }
        }

        //adds developing technologies to global developed list 
        foreach(string developingTechnology in CurrentPlayerData.DevelopingTechnologies)
        {
            CurrentPlayerData.DevelopedTechnologies.Add(developingTechnology);
        }

        //gets developed technologies, including those just developed this round
        string tech = "";
        foreach(string developedTech in CurrentPlayerData.DevelopedTechnologies)
        {
            Debug.Log(developedTech);
            tech += " " + developedTech.Replace(" ", "");
            Debug.Log(tech);
            
        }

        //writes to data string, finds original player and current file and writes there
        string data = CurrentPlayerData.Name + "\n" + CurrentPlayerData.CityName + "\n" + CurrentPlayerData.Round + "\n0\n" + CurrentPlayerData.RoundCredits.ToString() + "\n0, 0, 0\n" + tech;
        x = 0;
        while(x < 20)
        {
            if (File.Exists(Application.dataPath + "\\Saves\\Save" + x + ".txt"))
            {
                string nametest = File.ReadLines(Application.dataPath + "\\Saves\\Save" + x + ".txt").FirstOrDefault();
                if(nametest == CurrentPlayerData.Name)
                {
                            Debug.Log("writing now to save");

                    File.WriteAllText(Application.dataPath + "\\Saves\\Save" + x + ".txt", data);
                    File.WriteAllText(Application.dataPath + "\\Saves\\Current_File.txt", data);
                    x = 21;
                }
            }   
            x++;
        }

       //open next scene for judgement...   
        StartCoroutine(SceneLoad());
    }

    IEnumerator SceneLoad()
    {
    //get the trigger for the animation close
    AnimatedObject.GetComponent<AnimationScript>().SceneCloseAnimation();

    //pause for animation to close
    yield return new WaitForSeconds(2.0f);
    SceneManager.LoadScene(2);
    }
}