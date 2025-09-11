using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map_interactable_script : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool Selection = false;

    [SerializeField]
    Grid MapGrid;

    [SerializeField]
    Tilemap tile_highlighter_grid;

    [SerializeField]
    GameObject playerData;

    [SerializeField]
    Tilemap playerTileMap;

    private Vector3Int pastSelectedTile = new Vector3Int(0, 0, 0);
    private Vector3Int currentSelectedFile = new Vector3Int(0, 0, 0);


    // Update is called once per frame
    void Update()
    {

        //TO AVOID CONFLICT WITH ui, EITHER MAKE UI FULL SCREEN MENU AND DEACTIVE MAP OR CHECK HERE IS MOUSE IS ON THE TILEMAP
        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log("Leftclick");
            var mousePosReal = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var mousePosGrid = MapGrid.WorldToCell(mousePosReal);
            currentSelectedFile = mousePosGrid;
            //            Debug.Log(mousePosGrid + " " + pastSelectedTile + " " + currentSelectedFile);
            Selection = false;
            if (playerTileMap.HasTile(mousePosGrid))
            {
                playerData.GetComponent<TileDataManagement>().ExistingTileDataCollection(playerTileMap, mousePosGrid);
            }
            else
            {
                Debug.Log("no tile there on player map, adjust here for getting base map data");
            }
        }
    }
}