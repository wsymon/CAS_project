using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CompletedFilesScript : MonoBehaviour
{
    [SerializeField]
    GameObject Content;

    [SerializeField]
    GameObject CompletedFilesObject;
    
    [SerializeField]
    TextMeshProUGUI nameandcityText;

    [SerializeField]
    TextMeshProUGUI completionCauseText;

    [SerializeField]
    Button completedFileButton;

    [SerializeField]
    GameObject SaveItemPrefab;

    [SerializeField]
    TextMeshProUGUI finalSeqText;
    
    [SerializeField]
    TextMeshProUGUI finalOutputText;


    public string[] SavePaths;
    public List<string[]> CompletedSaveData = new List<string[]> { };
    public string[] player_data;
    
    //setup of first run...
    public void Start()
    {
        CompletedFilesObject.SetActive(true);
        //finds save paths (only txt not unity's assets)
        SavePaths = Directory.GetFiles(Application.dataPath + "\\Completed Saves", "*.txt");
        foreach(string savePath in SavePaths)
        {
            player_data = File.ReadAllLines(savePath);
            CompletedSaveData.Add(player_data);
        }

        //in the event no completed saves where found, 
        if(CompletedSaveData.Count == 0)
        {
            completedFileButton.interactable = false;
            completedFileButton.GetComponent<Image>().color = new Color(185, 185, 185, 255);
        }

        //if some amount of files exist
        else
        {
            //sets all the necessary attributes to the created list items (toggle group,parent, change value, text)
            foreach(string[] data in CompletedSaveData)
            {
                GameObject Item = Instantiate(SaveItemPrefab);
                Item.transform.SetParent(Content.transform);
                Item.transform.localScale = new Vector3(1, 1, 1);
                Item.GetComponent<Toggle>().group = Content.GetComponent<ToggleGroup>();
                Item.GetComponent<Toggle>().onValueChanged.AddListener(delegate{
                UpdateCompletedFileDisplay();});
                Item.GetComponentInChildren<TextMeshProUGUI>().text = data[0];
                Item.GetComponentInChildren<TextMeshProUGUI>().fontSize = 40;
                Item.GetComponentInChildren<TextMeshProUGUI>().overflowMode = TextOverflowModes.Ellipsis;


            }
            
            //sets default display info (on first loading) as the first file found in the folder 
            nameandcityText.text = CompletedSaveData[0][0] + CompletedSaveData[0][1];
            completionCauseText.text = CompletedSaveData[0][11];
            finalOutputText.text = CompletedSaveData[0][9];
            finalSeqText.text = CompletedSaveData[0][7];
        }
        CompletedFilesObject.SetActive(false);
    }

    //updates display data (but not options/file data)
    public void UpdateCompletedFileDisplay()
    {
        GameObject OnToggle = Content.GetComponent<ToggleGroup>().ActiveToggles().First().gameObject;
        string[] NewData = new string[] {};
        foreach(string[] data in CompletedSaveData)
        {
            if (data[0] == OnToggle.GetComponentInChildren<TextMeshProUGUI>().text)
            {
                nameandcityText.text = data[0] + " of " + data[1];
                completionCauseText.text = data[11];
                finalOutputText.text = data[9];
                finalSeqText.text = data[7];
            }
        }
    }

    //deletes selected save file and data, then updates list options and display 
    public void DeleteCompletedSave()
    {
        //gets and removes deleted data from CompletedSaveData (no need to redo whole thing)
        GameObject OnToggle = Content.GetComponent<ToggleGroup>().ActiveToggles().First().gameObject;
        string[] dataToRemove = new string[] {};
        foreach(string[] data in CompletedSaveData)
        {
            if (data[0] == OnToggle.GetComponentInChildren<TextMeshProUGUI>().text)
            {
                dataToRemove = data;
            }
        }
        //removes data from this script's list
        CompletedSaveData.Remove(dataToRemove);

        //deletes files and destroys gameobject
        File.Delete(Application.dataPath + "\\Completed Saves\\Save" + OnToggle.GetComponentInChildren<TextMeshProUGUI>().text + ".txt"); 
        File.Delete(Application.dataPath + "\\Completed Saves\\Save" + OnToggle.GetComponentInChildren<TextMeshProUGUI>().text + ".txt.meta");
        Destroy(OnToggle);

        //if file is deleted and no more exist, then shut menu make inaccessible. 
        if(CompletedSaveData.Count() == 0)
        {
            OpenAndCloseCompletedFilePage();
            completedFileButton.interactable = false;
            completedFileButton.GetComponent<Image>().color = new Color(185, 185, 185, 255);   
        }
        else
        {
            UpdateCompletedFileDisplay();
        }
    }

    //simply opens the file
    public void OpenAndCloseCompletedFilePage()
    {
        if(CompletedFilesObject.activeSelf == true)
        {
            CompletedFilesObject.SetActive(false);
        }
        else
        {
            CompletedFilesObject.SetActive(true);
        }
    }
}