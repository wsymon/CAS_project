using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DevelopmentScript : MonoBehaviour
{
    [SerializeField]
    GameObject TechnologiesContent;

    [SerializeField]
    GameObject DevelopmentMenuObject;

    [SerializeField]
    TextMeshProUGUI TechEducation;

    [SerializeField]
    TextMeshProUGUI TechCreditCost;

    [SerializeField]
    TextMeshProUGUI TechSeq;

    [SerializeField]
    TextMeshProUGUI TechCarbonCost;

    [SerializeField]
    TextMeshProUGUI TechOutput;

    [SerializeField]
    TextMeshProUGUI TechName;

    [SerializeField]
    Button InvestButton;

    [SerializeField]
    GameObject PlayerData;

    [SerializeField]
    GameObject GlobalUIObject;

    [SerializeField]
    GameObject DevelopmentPopup;

    public string[] AllTechnologies = new string[10];
    public customTile SelectedTech;

    //initial setup
    public void TechDropDownSetup()
    {
        //makes first option automatically selected the windmill
        SelectedTech = Resources.Load<customTile>("WindmillL1");
        TechEducation.text = SelectedTech.Education;
        TechCarbonCost.text = SelectedTech.CarbonCost.ToString();
        TechOutput.text = SelectedTech.Output.ToString();
        TechName.text = SelectedTech.StructureType;
        TechCreditCost.text = SelectedTech.CreditCost.ToString();

        //ensures confirm button is the correct variant
        TechUpdate();
    }

    //UPDATES THE UI
    public void TechUpdate()
    {

        if(TechnologiesContent.GetComponent<ToggleGroup>().IsActive() == true)
        {
            customTile SelectTile = (customTile)Resources.Load(TechnologiesContent.GetComponent<ToggleGroup>().ActiveToggles().First().name + "L1");
            if(CurrentPlayerData.DevelopingTechnologies.Contains(SelectTile.StructureType))
            {
                TechEducation.text = "Development of this technology will be complete next Round (" + (int)(CurrentPlayerData.Round + 1) + ") " + SelectTile.Education;
            }
            else if(CurrentPlayerData.DevelopedTechnologies.Contains(SelectTile.StructureType) == false)
            {
                TechEducation.text = "Development of this technology will cost " + SelectTile.DevelopmentCost.ToString() + " credits. " + SelectTile.Education;
            }
            else
            {
                TechEducation.text = SelectTile.Education;
            }
            TechCarbonCost.text = SelectTile.CarbonCost.ToString();
            TechOutput.text = SelectTile.Output.ToString();
            TechName.text = SelectTile.StructureType;
            TechCreditCost.text = SelectTile.CreditCost.ToString();
            TechSeq.text = SelectTile.Sequestration.ToString();

            bool c = false;
            foreach(string tech in CurrentPlayerData.DevelopedTechnologies)
            {
                if(tech == SelectTile.StructureType)
                {
                    c = true;
                }
            }
            foreach(string newTech in CurrentPlayerData.DevelopingTechnologies)
            {
                if(newTech == SelectTile.StructureType)
                {
                    c = true;
                }
            }

            //if the tech is buyable and not currently bought
            if(SelectTile.DevelopmentCost < CurrentPlayerData.RoundCredits && c == false)
            {
                InvestButton.image.color = new Color(255f, 255f, 255f, 255f);
                InvestButton.interactable = true;
            }
            //if not buyable but also not yet bought
            else if(CurrentPlayerData.DevelopedTechnologies.Contains<string>(SelectTile.StructureType) == false)
            {
                //RECHECK THESE VALUES FOR CORRECT 'not usable' COLORS
                InvestButton.image.color = new Color(185, 185, 185, 255);
                InvestButton.interactable = false;
            }
            else
            {
                //make this green as if already bought, this is the sole case for that 
                InvestButton.image.color = new Color(255, 255, 255, 0);
                InvestButton.interactable = false;
            }  
        }
    }

    //confirms change and updates global and display data to match 
    public void ConfirmNewDevelopedTechnology()
    {
        //fixes the button
        InvestButton.image.color = new Color(0.75f, 0.75f, 0.75f, 0.75f);           
        InvestButton.interactable = false;

        //reducts credit amount and finds customTile of the tech
        customTile SelectTile = (customTile)Resources.Load(TechnologiesContent.GetComponent<ToggleGroup>().ActiveToggles().First().name + "L1");
        CurrentPlayerData.RoundCredits -= SelectTile.DevelopmentCost;

        CurrentPlayerData.DevelopingTechnologies.Add(SelectTile.StructureType);
        GlobalUIObject.GetComponent<GlobalUI>().UpdateGlobalUI();
        TechUpdate();

    }
}
   