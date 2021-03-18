using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTile : Tile
{
    private Vector3Int position;

    public Vector3Int Position { get => position;}

    public override bool StartUp(Vector3Int position,ITilemap tilemap,GameObject go)
    {
        AStar.GroundTile.Add(position);
        this.position=position;
        return base.StartUp(position,tilemap,go);
        
    }

    #if UNITY_EDITOR
        [MenuItem("Assets/Create/Tiles/GroundTile")]
        public static void CreateWallTile()
        {
            string path=EditorUtility.SaveFilePanelInProject("Save GroundTile","GroundTile","asset","Save groundTile","Assets");
            if(path=="")
            {
                return;
            }
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<GroundTile>(),path);
        }
    #endif
}
