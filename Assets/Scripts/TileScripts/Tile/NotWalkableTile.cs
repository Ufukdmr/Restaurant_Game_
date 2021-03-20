using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NotWalkableTile : Tile
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
        AStar.NotWalkableTile.Add(tile);
        return base.StartUp(position,tilemap,go);
    }

    #if UNITY_EDITOR
        [MenuItem("Assets/Create/Tiles/NotWalkableTile")]
        public static void CreateWallTile()
        {
            string path=EditorUtility.SaveFilePanelInProject("Save NotWalkableTile","NotWalkableTile","asset","Save notWalkableTile","Assets");
            if(path=="")
            {
                return;
            }
            AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<NotWalkableTile>(),path);
        }
    #endif
}
