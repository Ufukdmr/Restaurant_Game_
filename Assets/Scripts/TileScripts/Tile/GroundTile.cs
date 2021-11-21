using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GroundTile : Tile
{
    
    public override bool StartUp(Vector3Int position,ITilemap tilemap,GameObject go)
    {
        TileControl tile=new TileControl(position,-1);
       if(this.sprite.name=="Z-1")
       {
           tile=new TileControl(position,0);
       }
       else if(this.sprite.name=="Z-2")
       {
           tile=new TileControl(position,1);
       }
       else if(this.sprite.name=="Z-3")
       {
           tile=new TileControl(position,2);
       }
       AStar.GroundTile.Add(tile.Position);
        AStar.TileControls.Add(tile);
        
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
