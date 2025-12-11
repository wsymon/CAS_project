using UnityEngine;
using TMPro;
using System.Collections;


public class RoundSystem : MonoBehaviour
{
    public int currentRound = 0;
    public int MoneyPerRound = 100;

    public MoneyManager moneyManager;
    public Player_saving playerData;

    public TextMeshProUGUI RoundText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (PlayerPrefs.HasKey("CurrentRound"))
        {
            currentRound = PlayerPrefs.GetInt("CurrentRound");
        }
        else
        {
            currentRound = 0;
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void StartNextRound()
    {
        currentRound++;

        //Save round
        PlayerPrefs.SetInt("CurrentRound", currentRound);

        //Adds money
        moneyManager.AddMoney(MoneyPerRound);

        //Update save data
        playerData.Round = currentRound;

        //Update UI
        UpdateRoundUI();

    }

    private void UpdateRoundUI()
    {
        RoundText.text = "Current Round: " + currentRound;
    }



}

    

