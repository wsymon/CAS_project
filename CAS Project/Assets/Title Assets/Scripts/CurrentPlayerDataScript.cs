using System.IO;
using System.Linq;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrentPlayerDataScript : MonoBehaviour
{
    //serializations to access player input for new file 
    [SerializeField]
    TextMeshProUGUI name_field;
    [SerializeField]
    TextMeshProUGUI city_field;
    [SerializeField]
    GameObject Title_functions_object;

    //serialization to access selected file for old file
    [SerializeField]
    GameObject file_select_object;

    //serialisation to talk with tile management script
    [SerializeField]
    GameObject currentPlayerData;

    //the important things
    public string Selected_File_name = "";
    public string Selected_File_city = "";
    public string Selected_File_source = "";

    public void SetCurrentPlayerFileNew()
    {
        Selected_File_name = name_field.text.ToString();
        Selected_File_city = city_field.text.ToString();
        Selected_File_source = Title_functions_object.GetComponent<title_functions>().File_source;
        //writes player info to current player file (txt in saves) so file sin next scene can read
        string[] player_information = new string[10];
        var raw = File.ReadLines(Selected_File_source);
        int l = 0;
        foreach (string line in raw)
        {
            player_information[l] = line;
            l++;
        }
        File.WriteAllLines(Application.dataPath + "\\Saves\\Current_File.txt", player_information);
    }

    //function that sets global current file data to what has been selected once old file is confirmed
    public void SetCurrentPlayerFileOld(string name, string source, string city)
    {
        //does as expected for selected file name source and city
        Selected_File_name = name;
        Selected_File_source = source;
        Selected_File_city = city;

        //writes player info to current player file (txt in saves) so file sin next scene can read
        string[] player_information = new string[10];
        var raw = File.ReadLines(Selected_File_source);
        int l = 0;
        foreach (string line in raw)
        {
            player_information[l] = line;
            l++;
        }
        File.WriteAllLines(Application.dataPath + "\\Saves\\Current_File.txt", player_information);
    }
}