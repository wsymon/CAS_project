using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.WSA;

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

    //defines
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

        PlayerName = player_information[0];
        PlayerCity = player_information[1];
        Round = int.Parse(player_information[2]);
        RoundCredits = int.Parse(player_information[4]);
        OutputCurrent = PlayerData.GetComponent<TileDataManagement>().CurrentTotalOutput;
        SequestrationCurrent = PlayerData.GetComponent<TileDataManagement>().CurrentTotalSeq;
        OutputGoal = (int)(1.2 * OutputCurrent);
        SequestrationGoal = 1000;
        CurrentCarbonCost = PlayerData.GetComponent<TileDataManagement>().CurrentTotalCarbonCost;
        DevelopedTech = player_information[6].Split(' ');

        //calls functions to update, could could manually but may need to reference from button click...
        PlayerData.GetComponent<CurrentPlayerData>().UpdateGlobalVariables(Round, PlayerName, PlayerCity, 0, RoundCredits, SequestrationCurrent, SequestrationGoal, CurrentCarbonCost, OutputCurrent, OutputGoal);
        CurrentPlayerData.DevelopedTechnologies = DevelopedTech;

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
    
    //does needed things at end of round 
    public void UpdateGlobalRoundEnd()
    {
        //top one just updates values in other script after scrapping the board
        PlayerData.GetComponent<TileDataManagement>().TileInfoCollection();

        PlayerName = CurrentPlayerData.Name;
        PlayerCity = CurrentPlayerData.CityName;
        Round = CurrentPlayerData.Round;
        RoundCredits = CurrentPlayerData.RoundCredits;
        ChangeInCredits = PreCredits - CurrentPlayerData.RoundCredits;
        OutputCurrent = PlayerData.GetComponent<TileDataManagement>().CurrentTotalOutput;
        SequestrationCurrent = PlayerData.GetComponent<TileDataManagement>().CurrentTotalSeq;
        OutputGoal = CurrentPlayerData.OutputGoal;
        SequestrationGoal = CurrentPlayerData.SequestrationGoal;
        CurrentCarbonCost = PlayerData.GetComponent<TileDataManagement>().CurrentTotalCarbonCost;
        
        PlayerData.GetComponent<CurrentPlayerData>().UpdateGlobalVariables(Round, PlayerName, PlayerCity, ChangeInCredits, RoundCredits, SequestrationCurrent, SequestrationGoal,CurrentCarbonCost, OutputCurrent, OutputGoal);

       //open next scene for judgement...   
        StartCoroutine(SceneLoad());
    }

    IEnumerator SceneLoad()
    {
    //get the trigger for the animation close
    AnimatedObject.GetComponent<AnimationScript>().SceneCloseAnimation();

    //pause for animation to close
    yield return new WaitForSeconds(2);

    //load map scene named "x" in BUILD SETTINGS.
    SceneManager.LoadScene(2);
    yield return new WaitForSeconds(2);
    }
}