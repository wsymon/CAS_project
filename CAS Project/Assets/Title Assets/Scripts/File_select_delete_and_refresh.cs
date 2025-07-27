using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class File_select_delete_and_refresh : MonoBehaviour
{

    //necessary serialized fields
    [SerializeField]
    Button Refresh;
    [SerializeField]
    Button Delete;

    //object containing main file select script  
    [SerializeField]
    GameObject file_select_object;

    //refreshes both existing global data on player files and display in main menu
    public void Refresh_files()
    {
        //clears dictionary of player names and list of file numbers
        file_select_object.GetComponent<File_select_script>().player_name_storage.Clear();
        file_select_object.GetComponent<File_select_script>().file_numbers.Clear();

        //calls function to set up select file menu
        file_select_object.GetComponent<File_select_script>().file_select_menu_setup();
        Debug.Log("refreshed!");
    }

    public void Delete_files()
    {
        //calls file finder function in main file select script 
        file_select_object.GetComponent<File_select_script>().file_finder();
        //sets a variable equal to the found links
        string deleted_link = file_select_object.GetComponent<File_select_script>().selected_file_link;
        string deleted_meta_link = deleted_link + ".meta";
        //deletes the file at that link and refreshes the menu display of available files.

        File.Delete(deleted_link);
        File.Delete(deleted_meta_link);

        Refresh_files();
    }
    }
    