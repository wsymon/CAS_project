using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class title_functions : MonoBehaviour
{
    //field for object with script that counts number of existing plyaer files
    [SerializeField]
    GameObject file_Select_Script;

    [SerializeField]
    GameObject playerData;

    private string[] path;
    private void Start()
    {
        //var used to check if existing files are there (and thus whether 'select_file' button exists)
        int checker = 0;
        int file_suffix_check = 0;
        while (file_suffix_check < 100)
        {
            if (File.Exists(Application.dataPath + "\\Saves\\Save" + file_suffix_check + ".txt"))
            {
                checker++; 
            }
            file_suffix_check++;
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

    [SerializeField]
    GameObject File_select_menu_button;

    [SerializeField]
    GameObject Create_file_menu_button;
    

    //so that the current player data script can access 
    public static string File_source;

    //fields in  unity for the input fields for city/names of player save
    [SerializeField]
    TextMeshProUGUI nameInput;
    [SerializeField]
    TextMeshProUGUI cityInput;
  
    public void Quit()
    {
        StartCoroutine(DelayedQuit());        
    }

    IEnumerator DelayedQuit()
    {
        yield return new WaitForSeconds(2.0f);
        Application.Quit();
    }

    //checks if new game menu isActive when select file is clicked to make select file menu appear
    public void new_game_opener()
    {
        if (new_game_menu.activeSelf == true)
        {
            Create_file_menu_button.GetComponent<UIAnimationScript>().UIAnimation();
        }
        else
        {
            Create_file_menu_button.GetComponent<UIAnimationScript>().UIAnimation();
            if (File_select_menu.activeSelf == true)
            {
                File_select_menu_button.GetComponent<UIAnimationScript>().UIAnimation();
            }
        }
    }

    //same but for select menu 
    public void File_select_menu_opener()
    {
        //checks if 
        if (File_select_menu.activeSelf == true)
        {
            File_select_menu_button.GetComponent<UIAnimationScript>().UIAnimation();
        }
        else
        {
            File_select_menu_button.GetComponent<UIAnimationScript>().UIAnimation();
            if (new_game_menu.activeSelf == true)
            {
                //if the other menu was open when this menu was clicked, shut the other one. 
                Create_file_menu_button.GetComponent<UIAnimationScript>().UIAnimation();
            }
        }

    }

    //function that makes new save file and writes to current file
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
                string content = Name + "\n" + cityName + "\n1\n1\n100\n0, 0, 0\nWindmill CoalStation Forest SolarPanel GasStation\n1 +";
                File_source = Application.dataPath + "\\Saves\\Save" + n + ".txt";
                //writes data to new file with number y and to the current file 
                File.WriteAllText(File_source, content);
                File.WriteAllText(Application.dataPath + "\\Saves\\Current_File.txt", content);
                break;
            }
        }
    }
}