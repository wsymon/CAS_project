using System;
using Microsoft.Unity.VisualStudio.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


//allows unity editor menu to show option to makea  custom tile
#if UnityEditor
using UnityEditor;
#endif

//class for custom tiles deriving from Tile
public class customTile : AnimatedTile
{
    //various values that are associated with tile
    [SerializeField]
    //public Sprite[] spriteAnimations;

    public Vector3Int tileGrid;
    public string StructureType;
    public int Level;
    public int Output;
    public int Sequestration;
    public string Education;

    //honestly note sure what this does 
    public override void RefreshTile(Vector3Int position, ITilemap tilemap)
    {
        base.RefreshTile(position, tilemap);
    }

    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        return base.GetTileAnimationData(position, tilemap, ref tileAnimationData);
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref UnityEngine.Tilemaps.TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
    }

    [MenuItem("Assets/Create/2D/CustomTiles/CustomTile")]
    public static void createCustomTile()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Custom Tile", "New Custom Tile", "Asset", "Save Custom Tile", "Assets");
        if (path == "") return;
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<customTile>(), path);
    }

    public Sprite GetTileSprite(customTile CustomTile)
    {
        Sprite sprite = CustomTile.m_AnimatedSprites[0];
        return sprite;
    }
}