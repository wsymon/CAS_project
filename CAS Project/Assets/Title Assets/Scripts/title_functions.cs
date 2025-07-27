using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;


public class title_functions : MonoBehaviour
{
    //field for object with script that counts number of existing plyaer files
    [SerializeField]
    GameObject file_Select_Script;

    private string[] path;
    private void Start()
    {
        //var used to check if existing files are there (and thus whether 'select_file' button exists)
        int checker = 0;
        //new string (the paths of saves) where APplication.dataPath automatically finds the Assets folder...
        path = new string[3];
        path[0] = Application.dataPath + "\\Saves\\Save0.txt";
        path[1] = Application.dataPath + "\\Saves\\Save1.txt";
        path[2] = Application.dataPath + "\\Saves\\Save2.txt";

        //really ugly and clunky way of checking if any of the 3 saves spots are there. could make a for loop for the dictionary...
        if (File.Exists(path[0]))
        {
            checker = +1;
        }
        if (File.Exists(path[1]))
        {
            checker = +1;
        }
        if (File.Exists(path[2]))
        {
            checker = +1;
        }
        if (checker == 0)
        {
            //if no saves were found and thus checker = 0, make select_file button uninteractable and reduce opacity. 
            selectFile_Object.interactable = false;
            Color blueWithAlpha = new Color(255f, 255f, 255f, 150f);
            selectFile_Object.image.color = blueWithAlpha;
        }

    }
    //select file object used in Start() to reduce opacity/interactability if no saves found
    [SerializeField]
    Button selectFile_Object;


    //begin menu object(with panel inside) to make it openable (via isActive)\
    [SerializeField]
    GameObject new_game_menu;

    //[SerializeField]
    //the data from the text field the person typed in

    //file select menu object to make it openable (via isActive)
    [SerializeField]
    GameObject File_select_menu;

    //fields in  unity for the input fields for city/names of player save
    [SerializeField]
    TextMeshProUGUI nameInput;
    [SerializeField]
    TextMeshProUGUI cityInput;
  
    public void Quit()
    {
        //shuts application down for Quit button
        Application.Quit();
    }

    
    public void new_game_opener()
    {
        //checks if new game menu isActive when select file is clicked to make select file menu appear
        if (new_game_menu.activeSelf == true)
        {
            new_game_menu.SetActive(false);
        }
        else
        {
            new_game_menu.SetActive(true);
        }
    }

    public void File_select_menu_opener()
    {
        //checks if 
        if (File_select_menu.activeSelf == true)
        {
            File_select_menu.SetActive(false);
        }
        else
        {
            File_select_menu.SetActive(true);
        }
    }
    public void Save_names()
    {
        //counts number of files in saves folder, then subtracts 4 for the template and current files (and their meta data) before cutting in half (metadata) and adding ibe
        int n = 0;
        while (n < 100)
        {
            if (File.Exists(Application.dataPath + "\\Saves\\Save" + n + ".txt"))
            {
                n++;
            }
            else
            {
                string Name = nameInput.text.ToString();
                string cityName = cityInput.text.ToString();
                string content = Name + "\n" + cityName + "\n0\n1\n100\n0, 0, 0";
                //writes data to new file with number y 
                File.WriteAllText(Application.dataPath + "\\Saves\\Save" + n + ".txt", content);
                break;
            }
        }
    }
}