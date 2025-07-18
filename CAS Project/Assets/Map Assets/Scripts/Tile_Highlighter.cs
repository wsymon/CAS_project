using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;


public class Highlighting : MonoBehaviour
{
    [SerializeField]
    private Tilemap Grid;
    [SerializeField]
    Input input;

    public Vector3 gridPosition;

    public AnimatedTile highlight;
    public Vector3Int pastTile = new Vector3Int(0, 0, 0);


    private void Update() { 
            //reads value of mouse position when clicked and converts to position on the tilemap
            var mousePosReal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePosGrid = Grid.WorldToCell(mousePosReal);

            Debug.Log(mousePosGrid);

            //sets highlight tile to that point in the grid
            Grid.SetTile(mousePosGrid, highlight);
            //checks if previous tile is the same as the current one
            if (pastTile != mousePosGrid) {
                Grid.SetTile(pastTile, null);
            }
            //sets next frame's past tile to the current tile the mouse is on
            pastTile = mousePosGrid;
    }
}