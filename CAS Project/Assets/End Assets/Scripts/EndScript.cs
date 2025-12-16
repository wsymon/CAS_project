using System;
using System.Collections;
using System.ComponentModel.Design;
using System.IO;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.Mathematics;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndScript : MonoBehaviour
{
    //ALL FOR ROUND END
    [SerializeField]
    GameObject RoundEndUIGameObject;

    [SerializeField]
    TextMeshProUGUI RoundText;

    [SerializeField]
    TextMeshProUGUI PlayerNameAndCityText;

    [SerializeField]
    TextMeshProUGUI OutputText;

    [SerializeField]
    TextMeshProUGUI SeqText;

    [SerializeField]
    TextMeshProUGUI SeqPercentageText;

    [SerializeField]
    TextMeshProUGUI CreditsText;

    [SerializeField]
    TextMeshProUGUI NextRoundText;

    [SerializeField]
    TextMeshProUGUI NextRoundCreditsText;

    [SerializeField]
    TextMeshProUGUI NextRoundSeqGoalText;

    [SerializeField]
    TextMeshProUGUI NextRoundOutputGoalText;

    [SerializeField]
    GameObject SeqbutNoOutputTextGameObject;

    //ALL FOR WIN/LOSE END
    [SerializeField]
    GameObject CompletionUIGameObject;

    [SerializeField]
    TextMeshProUGUI completionNameAndCity;
    
    [SerializeField]
    TextMeshProUGUI completionOutput;

    [SerializeField]
    TextMeshProUGUI completionSeqAndCarbonCost;

    [SerializeField]
    TextMeshProUGUI completionInfo;

    //Animation
    [SerializeField]
    GameObject AnimationBackdropObject;

    public static int SuccessiveOutputCounter;
    private float SeqPercent;
    private int file_number = -1;
    private string DevelopTechs;

    //just does the logic to check whether game is over (and then if won/lost) or simply round end
    void Awake()
    {
        if(CurrentPlayerData.Round > 10 && CurrentPlayerData.TotalSequestration >= CurrentPlayerData.SequestrationGoal && CurrentPlayerData.TotalOutput >= CurrentPlayerData.OutputGoal)
        {
            Completion("Sustainability");
        }
        else if(CurrentPlayerData.Round > 10)
        {
            Completion("Unsustainability");
        }
        else
        {
            RoundEnd();
        }
    }   

    //everything that occurs if a round ends not at round 10+
    public void RoundEnd()
    {
        //this because i am too silly :3
        SeqbutNoOutputTextGameObject.SetActive(true);

        //in case of premature ending earlier than round 10
        if(CurrentPlayerData.TotalSequestration >= CurrentPlayerData.SequestrationGoal)
        {
            if(CurrentPlayerData.TotalOutput >= CurrentPlayerData.OutputGoal)
            {
                Completion("Sustainability");
            }
            else
            {
                //in the event of seq with but not enough output...
                Debug.Log("SeqCompleteButOutputFail");
                SeqText.color = new Color(255, 255, 255, 0);
                NextRoundSeqGoalText.color = new Color(255, 255, 255, 0);
                SeqPercentageText.color = new Color(255, 255, 255, 0);
                SeqbutNoOutputTextGameObject.SetActive(true);
                //SeqbutNoOutputTextGameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
        }

        //checks for output success (for later next round calculations and fails)
        if(SuccessiveOutputCounter > 0)
        {
            if (CurrentPlayerData.TotalOutput >= CurrentPlayerData.OutputGoal)
            {
                Debug.Log("Output succeeded");
                SuccessiveOutputCounter++;
                //make game object of tick/X visible or not
            }
            else
            {
                Debug.Log("output failed");
                SuccessiveOutputCounter = -1;
                //make game object visible/not, apply punishment to next round credits, look into bonus for multiple rounds of success
            }
        }
        else
        {
            if (CurrentPlayerData.TotalOutput >= CurrentPlayerData.OutputGoal)
            {
                Debug.Log("Output succeeded");
                SuccessiveOutputCounter = 1;
                //game object visible or not
            }
            else
            {
                SuccessiveOutputCounter--;
                if(SuccessiveOutputCounter <= -3)
                {
                    Completion("Output");
                }
            }
        }
        //UI changes (those for past round)
        RoundText.text = "Round # " + CurrentPlayerData.Round.ToString();
        PlayerNameAndCityText.text = CurrentPlayerData.Name + "'s city " + CurrentPlayerData.CityName + ": ";
        SeqText.text = "Total Sequestration: " + CurrentPlayerData.TotalSequestration.ToString() + " ppm                             Goal Sequestrion: "+ CurrentPlayerData.SequestrationGoal.ToString() + " ppm";
        SeqPercent = (float)Math.Round((decimal)100* CurrentPlayerData.TotalSequestration / CurrentPlayerData.SequestrationGoal, 2);
        SeqPercentageText.text = "You are " + SeqPercent + "% there!";
        CreditsText.text = "Initial Credits was " +(Math.Abs(CurrentPlayerData.ChangeInCredits + CurrentPlayerData.RoundCredits)).ToString() + 
        "C, credits spent \n were " + CurrentPlayerData.ChangeInCredits.ToString() + "C with remaining " + (Math.Abs(CurrentPlayerData.RoundCredits)).ToString() + "C.";
        
        OutputText.text = "Total Output: " + CurrentPlayerData.TotalOutput.ToString() + "kJ                Goal Output: " + CurrentPlayerData.OutputGoal.ToString() + " kJ";

        //for all the calculations
        NextRoundCalculations();
    }

    //calculations for next round to update to the currentPlayerData global values and the needed UI changes
    //BALANCE LATER
    public void NextRoundCalculations()
    {
        //changes
        CurrentPlayerData.Round = CurrentPlayerData.Round + 1;
        CurrentPlayerData.ChangeInCredits = 0;
        CurrentPlayerData.RoundCredits = CurrentPlayerData.RoundCredits + 100; //BASE ADDITIVE 
        CurrentPlayerData.SequestrationGoal =(int)Math.Round( CurrentPlayerData.CarbonCost*3.0); //the carbon goal is 300% of existing carbon causes
        CurrentPlayerData.OutputGoal = (int)Math.Round(CurrentPlayerData.TotalOutput*1.4); // the output goal is +40% of previous output (maybe change, irl is 1-2%)
        
        //updating changes to player file and current file
        string[] player_data = new string[10];
        player_data[0] = CurrentPlayerData.Name;
        player_data[1] = CurrentPlayerData.CityName; 
        player_data[2] = CurrentPlayerData.Round.ToString();
        player_data[3] = 0.ToString();
        player_data[4] = CurrentPlayerData.RoundCredits.ToString();
        player_data[5] = "rah :3";        

        DevelopTechs = "";
        foreach(string atech in CurrentPlayerData.DevelopedTechnologies)
        {
            DevelopTechs = DevelopTechs + " " + atech;
        }
        player_data[6] = DevelopTechs;

        //this finds the player file to update to in case game is quit/another player is chosen
        file_number = -1;
        int k = 0;
        while (file_number == -1)
        {
            string[] data = File.ReadAllLines(Application.dataPath + "\\Saves\\Save" + k + ".txt");
            if(data[0] == CurrentPlayerData.Name)
            {
                file_number = k;
            }
            k++;
        }
        
        //does the writing to the file...
        File.WriteAllLines(Application.dataPath + "\\Saves\\Save" + file_number + ".txt", player_data);
        File.WriteAllLines(Application.dataPath + "\\Saves\\Current_File.txt", player_data);

        //gives 30% boost to credits if output is good successively
        if(SuccessiveOutputCounter >= 3)
        {
            CurrentPlayerData.RoundCredits = (int)Math.Round(CurrentPlayerData.RoundCredits * 1.3); 
        }

        //application of changes to UI
        RoundEndUIGameObject.SetActive(true);
        CompletionUIGameObject.SetActive(false);
        NextRoundCreditsText.text = "Round " + CurrentPlayerData.Round + "'s Credits: " + CurrentPlayerData.RoundCredits + "C";
        NextRoundText.text = "Round " + CurrentPlayerData.Round;
        NextRoundSeqGoalText.text = "Round " + CurrentPlayerData.Round + "'s Sequestration Goal: " + CurrentPlayerData.SequestrationGoal;
        NextRoundOutputGoalText.text = "Round " + CurrentPlayerData.Round + "'s Output Goal: " + CurrentPlayerData.OutputGoal;
    }

    //functions that fill the completion text based on normal/early win, output loss and unsustainability 
    public void Completion(string Cause)
    {
        CompletionUIGameObject.SetActive(true);
        RoundEndUIGameObject.SetActive(false);
        completionNameAndCity.text = CurrentPlayerData.Name + "'s city " + CurrentPlayerData.CityName; 

        if(Cause == "Sustainability")
        {
            Debug.Log("Completion Success");
            completionInfo.text = "Congratulations! In " + CurrentPlayerData.Round.ToString() + " rounds, you succeeded in an energy transformation that brought " + CurrentPlayerData.CityName + " City to a sustainable and renewable system of energy production! For more unformation, press the download button for a text file of your statistics! We encourage to reflect on any new technological or systemic knowledge you learnt, and bring that awareness into another round of Barometric and your future decisions.";
            completionSeqAndCarbonCost.text = "Your society's cost in carbon of " + CurrentPlayerData.CarbonCost.ToString() + " ppm was offset through your sequestration potential of " + CurrentPlayerData.TotalSequestration.ToString() + " ppm. Nice work!";
            completionOutput.text = "Additionally, your society's need of " + CurrentPlayerData.OutputGoal.ToString() + " kJ was more that satisfied through your production of " + CurrentPlayerData.TotalOutput.ToString() + " kJ per round.";
        }
        else if(Cause == "Output")
        {
            Debug.Log("output unsustainability");
            completionOutput.GetComponent<GameObject>().SetActive(false);
            completionInfo.text = "Unfortunately, you were unable to provide support output (of " + CurrentPlayerData.TotalOutput.ToString() +" kJ instead of the required" + CurrentPlayerData.OutputGoal.ToString() + " kJ) for your society for three consecutive rounds. For more unformation, press the download button for a text file of your statistics! We encourage to reflect on any new technological or systemic knowledge you learnt, and bring that awareness into another round of Barometric and your future decisions. Thank you for playing!";
            completionSeqAndCarbonCost.text = "However, your production of " + CurrentPlayerData.TotalOutput.ToString() + "kJ ";
        }
        else if(Cause == "Unsustainability")
        {
            Debug.Log("unsustainability");
            completionInfo.text = "Unfortunately, within the allocated rounds your society was unable to provide the appropriate output nor reach sufficient sustainable practices to avoid significant environmental and ecological disaster.";
            completionSeqAndCarbonCost.text = "Out of the " + CurrentPlayerData.CarbonCost.ToString() + " ppm cost in carbon of your output, only " + CurrentPlayerData.TotalSequestration.ToString() + " ppm was sequestered per round.";
            completionOutput.text = "Additionally, your society's current output of " + CurrentPlayerData.TotalOutput.ToString() + " kJ failed to satisfy society's need for " + CurrentPlayerData.OutputGoal.ToString() + " kJ.";
        }

        else
        {
            Debug.Log("something went wrong...");
            completionInfo.text = "Not sure what you did, but you broke this system. Congratulations :3 ";
        }
        
    }

    //pretty self explanatory, remember to add animation support here later during the coroutine 
    public void OpenTitleScene()
    {
        Debug.Log("open title");
        StartCoroutine(SceneLoad(0));
    }
    public void OpenMapScene()
    {
        Debug.Log("open map");
        StartCoroutine(SceneLoad(1));
    }
    IEnumerator SceneLoad(int SceneNumber)
    {
    //open trigger for closing scene animation
    AnimationBackdropObject.GetComponent<AnimationScript>().SceneCloseAnimation();

    //pause for animation to close
    yield return new WaitForSeconds(2);

    //load map scene named "x" in BUILD SETTINGS.
    SceneManager.LoadScene(SceneNumber);
    yield return new WaitForSeconds(2);
    }
}