using UnityEngine;
using TMPro;
using System.Collections;


public class RoundSystem : MonoBehaviour
{
    public int currentround = 0;
    public int MoneyPerRound = 100;

    public MoneyManager moneyManager;
    public Player_saving playerData;

    public TextMeshProUGUI RoundText;


    public void StartNextRound()
    {
        currentround++;

        //Adds money
        moneyManager.AddMoney(MoneyPerRound);

        //Update save data
        playerData.Round = currentround;

        //Update UI
        UpdateRoundUI();

    }

    private void UpdateRoundUI()
    {
        RoundText.text = "Current Round: " + currentround;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {

    }

}

    

