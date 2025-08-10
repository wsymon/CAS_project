using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;
using UnityEngine.UI;


public class Highlighting : MonoBehaviour
{
    [SerializeField]
    private Tilemap Grid;
    [SerializeField]
    Input input;

    public Vector3 gridPosition;

    public AnimatedTile highlight;

    public AnimatedTile Hover;
    public Vector3Int pastTile = new Vector3Int(0, 0, 0);

    [SerializeField]
    public GameObject player_data;


    private void Update()
    {
        Tile_highlighter_updater();
    }

    public void Tile_highlighter_updater()
    {
        var mousePosReal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var mousePosGrid = Grid.WorldToCell(mousePosReal);
        //if the player has left clicked to select a file
        if (player_data.GetComponent<Map_interactable_script>().Selection == false)
        {
            //reads value of mouse position when clicked and converts to position on the tilemap

            //sets highlight tile to that point in the grid
            Grid.SetTile(mousePosGrid, Hover);
            //checks if previous tile is the same as the current one
            if (pastTile != mousePosGrid)
            {
                Grid.SetTile(pastTile, null);
            }
            //sets next frame's past tile to the current tile the mouse is on
            pastTile = mousePosGrid;
        }
        else if(pastTile.x != 0 && pastTile.y != 0 && pastTile.z != 0)
        {
            Grid.SetTile(pastTile, null);
        }
    }
}