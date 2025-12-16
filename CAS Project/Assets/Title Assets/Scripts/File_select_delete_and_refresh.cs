using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class File_select_delete_and_refresh : MonoBehaviour
{
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
    }

    public void Delete_files()
    {
        //calls file finder function in main file select script 
        file_select_object.GetComponent<File_select_script>().file_finder();
        //sets a variable equal to the found links
        string deleted_link = file_select_object.GetComponent<File_select_script>().selected_file_link;
        string deleted_meta_link = deleted_link + ".meta";
        //deletes the file at that link and refreshes the menu display of available files.

        //gets information for the tileset thing
        string current_file_name = file_select_object.GetComponent<File_select_script>().selected_file;
        Debug.Log(current_file_name);
        string current_tile_data = Application.dataPath + "\\Tile Saves\\TileData" + current_file_name + ".txt";
        string current_tile_data_meta = current_tile_data + ".meta";
        //just for debugging...
        if (File.Exists(current_tile_data) == true)
        {
            Debug.Log("File exists");
            File.Delete(current_tile_data);
            File.Delete(current_tile_data_meta);
        }
        else
        {
            Debug.Log("User tile data not found.");
        }

        File.Delete(deleted_link);
        File.Delete(deleted_meta_link);

        //refreshes display to show changes
        file_select_object.GetComponent<File_select_script>().file_select_menu_setup();
    }
    }