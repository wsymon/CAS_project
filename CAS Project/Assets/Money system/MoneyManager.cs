using UnityEngine;
using TMPro;

public class MoneyManager : MonoBehaviour
{

    public TextMeshProUGUI MoneyText;
    public int currentMoney;

    public Player_saving playerData;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(PlayerPrefs.HasKey("CurrentMoney"))
        {
            currentMoney = PlayerPrefs.GetInt("CurrentMoney");
        }
        else 
        {
            currentMoney = 0;
        }

        //Sync to player data
        playerData.Credits = currentMoney;

        UpdateMoneyUI();

    }

    // Update is called once per frame
    void Update()
    {


    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        PlayerPrefs.SetInt("CurrentMoney", currentMoney);

        playerData.Credits = currentMoney;
        UpdateMoneyUI();
    }

    void UpdateMoneyUI()
    {
        MoneyText.text = "Credits: " + currentMoney;
    }
}
