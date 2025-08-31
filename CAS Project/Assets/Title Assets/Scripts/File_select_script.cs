using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Reflection;
using JetBrains.Annotations;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class File_select_script : MonoBehaviour
{
    //to reference text fields in each button for the 
    [SerializeField]
    TextMeshProUGUI file_1;
    [SerializeField]
    TextMeshProUGUI file_2;
    [SerializeField]
    TextMeshProUGUI file_3;
    //serialized field for file select button/delete button opener to remove it if no player files found
    [SerializeField]
    Button select_file_button;
    [SerializeField]
    Button Delete_button;

    //serialised buttons so i can make even/remove others if only 1/2 player files.
    [SerializeField]
    GameObject file_button_1;
    [SerializeField]
    GameObject file_button_2;
    [SerializeField]
    GameObject file_button_3;

    //serial field for file confirmation button so it can be uninteractable if player has made no choice. 
    [SerializeField]
    Button File_confirm_button;
    public Dictionary<int, string> player_name_storage = new Dictionary<int, string>();

    //object for player script so i can reference to get selected file 
    [SerializeField]
    GameObject Player_data_object;

    public List<int> file_numbers;
    public int p;
    public int y = 0;

    //an empty string that is updated to be the name of the file selected on button press
    public string selected_file = "";

    //public string of last clicked button (to have old ones deactviate when another is pressed)
    public string last_clicked = "";

    //string reference to selected file to be linked to in delete function...
    public string selected_file_link = "";

    Color usable = new Color32(255, 255, 255, 255);
    Color unusable = new Color32(255, 255, 255, 150);


    public void Start()
    {
        file_select_menu_setup();
    }


    public void file_button_press1()
    {
        //defines for opacity of selected and unselected buttons
        Color pressed = new Color32(0, 153, 153, 150);
        Color unpressed = new Color(1, 1, 1, 1);
        Color current_color = file_button_1.GetComponent<Button>().image.color;
        //note that Color32 is RGB as normal (1->255) but just Color is unity's scale from 0->1.

        //changes color of button to indicate whether or not it is currently selected
        if (current_color == unpressed)
        {
            file_button_1.GetComponent<Button>().image.color = pressed;
            //sets global selected file to text in file 1

            //sets color of other buttons to normal (so multiple aren't pressed at once visually)
            if (last_clicked == "2")
            {
                file_button_2.GetComponent<Button>().image.color = unpressed;
            }
            if (last_clicked == "3")
            {
                file_button_3.GetComponent<Button>().image.color = unpressed;
            }
            selected_file = "";
            selected_file = file_1.text;
            last_clicked = "1";
        }
        if (current_color == pressed)
        {
            file_button_1.GetComponent<Button>().image.color = unpressed;
            selected_file = "";
            Debug.Log(selected_file + 1);
        }
       
    }
    public void file_button_press2()
    {
        //same as first function for pressing...
        Color pressed = new Color32(0, 153, 153, 150);
        Color unpressed = new Color(1, 1, 1, 1);
        Color current_color = file_button_2.GetComponent<Button>().image.color;

        if (current_color == unpressed)
        {
            file_button_2.GetComponent<Button>().image.color = pressed;
            selected_file = "";
            selected_file = file_2.text;
            if (last_clicked == "1")
            {
                file_button_1.GetComponent<Button>().image.color = unpressed;
            }
            if (last_clicked == "3")
            {
                file_button_3.GetComponent<Button>().image.color = unpressed;
            }
            last_clicked = "2";

        }
        if (current_color == pressed)
        {
            file_button_2.GetComponent<Button>().image.color = unpressed;
            selected_file = "";
        }

        
    }
    public void file_button_press3()
    {
        //same as first function
        Color pressed = new Color32(0, 153, 153, 150);
        Color unpressed = new Color(1, 1, 1, 1);
        Color current_color = file_button_3.GetComponent<Button>().image.color;


        if (current_color == unpressed)
        {
            file_button_3.GetComponent<Button>().image.color = pressed;
            selected_file = "";
            selected_file = file_3.text;
            if (last_clicked == "1")
            {
                file_button_1.GetComponent<Button>().image.color = unpressed;
            }
            if (last_clicked == "2")
            {
                file_button_2.GetComponent<Button>().image.color = unpressed;
            }
            last_clicked = "3";

        }
        else if (current_color == pressed)
        {
            file_button_3.GetComponent<Button>().image.color = unpressed;
            selected_file = "";
            Debug.Log(selected_file + 3);
        }
        
    }

    //writing data of selected file to current file  
    public void file_confirm_press()
    {
        string city = "";
        foreach (int linker in file_numbers)
        {
            //no need to check if current name is valid as button is uninteractable otherwise( see Update())
            foreach (KeyValuePair<int, string> name in player_name_storage)
            {
                if (player_name_storage[linker] == selected_file)
                {
                    //copies save 
                    selected_file_link = Application.dataPath + "\\Saves\\Save" + linker + ".txt";
                    string[] information = File.ReadAllLines(selected_file_link);
                    city = information[1];
                    File.WriteAllLines(selected_file_link, information);
                    }
            }
        }
        //sends information to update selected player file once player confirms choice
        Player_data_object.GetComponent<CurrentPlayerDataScript>().SetCurrentPlayerFileOld(selected_file, selected_file_link, city);
    }

    //goes through list and dictionary to find selected file, then writes file link down to a public string
    public void file_finder()
    {
        foreach (int number in file_numbers)
        {
            if (player_name_storage[number] == selected_file)
            {
                selected_file_link = Application.dataPath + "\\Saves\\Save" + number + ".txt";
               // Debug.Log("link found: " + selected_file_link);
            }
        }
    }

    //adds each file to the dictionary using file numbers found in file finder
    public void existing_file_counter()
    {
        player_name_storage.Clear();

        foreach (int thing in file_numbers)
        {
            if (File.Exists(Application.dataPath + "\\Saves\\Save" + thing + ".txt"))
            {
                //if the file exists, read the first line (player name)
                string file_name_temp = File.ReadLines(Application.dataPath + "\\Saves\\Save" + thing + ".txt").ElementAtOrDefault(0);
                //add entry to dictionary under first entry 
                player_name_storage.Add(thing, file_name_temp);
            }
        }

    }
    //function to make file select menu work
    public void file_select_menu_setup()
    {
//        Debug.Log("in setup");
        //in the set up defines these and calls the other function
        bool c1 = false;
        bool c2 = false;
        bool c3 = false;

        //resetting text on buttons just in case
        file_1.text = "";
        file_2.text = "";
        file_3.text = "";

        //adding all file numbers of existing files to 
        int x = 0;
        while (x < 100)
        {
            if (File.Exists(Application.dataPath + "\\Saves\\Save" + x + ".txt"))
            {
                file_numbers.Add(x);
//                Debug.Log(x);
                y++;
            }
            x++;
        }


        

        //links to function that adds first line of text in player files to dictionary 
        existing_file_counter();

        int c = Directory.GetFiles(Application.dataPath + "\\Saves").Length;
        int l = (c - 4) / 2;
        if (l == 0)
        {

            //setting file select button unusable if no files.
            select_file_button.interactable = false;
            Color blueWithAlpha = new Color(255f, 255f, 255f, 150f);
            select_file_button.image.color = blueWithAlpha;

            Delete_button.interactable = false;
        }
        else if (l == 1)
        {
            file_button_2.SetActive(false);
            file_button_3.SetActive(false);
            //file_button_1.transform.position = new UnityEngine.Vector3(0f, 0f, 0f); 

        }
        else if (l == 2)
        {
            file_button_3.SetActive(false);
        }
        else
        {
            file_button_1.SetActive(true);
            file_button_2.SetActive(true);
            file_button_3.SetActive(true);
        }


        foreach (int numb in file_numbers)
        {
            //retreiving z entry in dictionary
            if (player_name_storage.ContainsKey(numb))
            {
                string current_name = player_name_storage[numb];
      //          Debug.Log(current_name);

                if (c1 == false)
                {
                    file_1.text = current_name;
                    c1 = true;

                }
                else if (c2 == false)
                {
                    file_2.text = current_name;
                    c2 = true;

                }
                else if (c3 == false)
                {
                    file_3.text = current_name;
                    c1 = true;

                }
                //else if is necessary here, changes text on button to current name of dictionary entry
            }
        }

    }

    void Update()
    {
        if (selected_file == "")
        {
            File_confirm_button.interactable = false;
            File_confirm_button.GetComponent<Image>().color = unusable;
        }
        else
        {
            File_confirm_button.interactable = true;
            File_confirm_button.GetComponent<Image>().color = usable;
        }
    }
}