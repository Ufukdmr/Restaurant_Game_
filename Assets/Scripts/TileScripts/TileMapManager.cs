using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { Table, Debug }
public class TileMapManager : MonoBehaviour
{
    private TileType tileType;

    [SerializeField]
    private Tilemap[] tileMap;

    [SerializeField]
    private Grid grid;
    [SerializeField]
    private Tile[] tile;

    [SerializeField]
    private AStar aStar;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private LayerMask layerMask;

    private static TileMapManager instance;

    HashSet<Vector3Int> placeableTile;
    HashSet<Vector3Int> notPlaceableTile;

    private bool isInitialize=false;

    public static TileMapManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TileMapManager>();
            }
            return instance;
        }
    }

    [SerializeField]
    private Color placeable, notPlaceable;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(camera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                Vector3 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3Int clickPos = tileMap[0].WorldToCell(new Vector3(mouseWorldPos.x, mouseWorldPos.y, transform.position.z));

                ChangeTile(clickPos);
            }
        }
    }

    public void Initialize(HashSet<Vector3Int> groundTile, HashSet<Vector3Int> notWalkableTiles)
    {
        placeableTile = new HashSet<Vector3Int>();
        notPlaceableTile = new HashSet<Vector3Int>();
        isInitialize=true;

        foreach (Vector3Int tile in groundTile)
        {
            placeableTile.Add(tile);

        }

        foreach (Vector3Int tile in notWalkableTiles)
        {
            notPlaceableTile.Add(tile);

        }

         CreateTiles();
    }
    public void ChangeTileType(TileButton button)
    {
        if (!isInitialize)
        {
            aStar.TileMapChange();
            tileType = button.MyTileType;
        }
        else
        {
            CreateTiles();
             tileType = button.MyTileType;
        }
       
    }
    private void ChangeTile(Vector3Int clickPos)
    {
        if (placeableTile.Contains(clickPos))
        {
            if (tileType == TileType.Table)
            {
                tileMap[0].SetTile(clickPos, tile[0]);
                aStar.ChangeTileAstar(clickPos);
                AdjustmentList(clickPos);
                CreateTiles();
            }
        }


    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tileMap[1].SetTile(position, tile[1]);
        tileMap[1].SetTileFlags(position, TileFlags.None);
        tileMap[1].SetColor(position, color);
    }

    public void CreateTiles()
    {
        foreach (Vector3Int tile in placeableTile)
        {
            ColorTile(tile, placeable);
        }

        foreach (Vector3Int tile in notPlaceableTile)
        {
            ColorTile(tile, notPlaceable);
        }
    }

    void AdjustmentList(Vector3Int pos)
    {
         placeableTile.Remove(pos);
        notPlaceableTile.Add(pos);
        if(tileMap[1].GetTile(new Vector3Int(pos.x+1,pos.y,pos.z))&&placeableTile.Contains(new Vector3Int(pos.x+1,pos.y,pos.z)))
        {
            placeableTile.Remove(new Vector3Int(pos.x+1,pos.y,pos.z));
            notPlaceableTile.Add(new Vector3Int(pos.x+1,pos.y,pos.z));
        }
        if(tileMap[1].GetTile(new Vector3Int(pos.x-1,pos.y,pos.z))&&placeableTile.Contains(new Vector3Int(pos.x-1,pos.y,pos.z)))
        {
            placeableTile.Remove(new Vector3Int(pos.x-1,pos.y,pos.z));
            notPlaceableTile.Add(new Vector3Int(pos.x-1,pos.y,pos.z));
        }
        if(tileMap[1].GetTile(new Vector3Int(pos.x,pos.y+1,pos.z))&&placeableTile.Contains(new Vector3Int(pos.x,pos.y+1,pos.z)))
        {
            placeableTile.Remove(new Vector3Int(pos.x,pos.y+1,pos.z));
            notPlaceableTile.Add(new Vector3Int(pos.x,pos.y+1,pos.z));
        }
        if(tileMap[1].GetTile(new Vector3Int(pos.x,pos.y-1,pos.z))&&placeableTile.Contains(new Vector3Int(pos.x,pos.y-1,pos.z)))
        {
            placeableTile.Remove(new Vector3Int(pos.x,pos.y-1,pos.z));
            notPlaceableTile.Add(new Vector3Int(pos.x,pos.y-1,pos.z));
        }
        if(tileMap[1].GetTile(new Vector3Int(pos.x+1,pos.y+1,pos.z))&&placeableTile.Contains(new Vector3Int(pos.x+1,pos.y+1,pos.z)))
        {
            placeableTile.Remove(new Vector3Int(pos.x+1,pos.y+1,pos.z));
            notPlaceableTile.Add(new Vector3Int(pos.x+1,pos.y+1,pos.z));
        }
        if(tileMap[1].GetTile(new Vector3Int(pos.x+1,pos.y-1,pos.z))&&placeableTile.Contains(new Vector3Int(pos.x+1,pos.y-1,pos.z)))
        {
            placeableTile.Remove(new Vector3Int(pos.x+1,pos.y-1,pos.z));
            notPlaceableTile.Add(new Vector3Int(pos.x+1,pos.y-1,pos.z));
        }
        if(tileMap[1].GetTile(new Vector3Int(pos.x-1,pos.y+1,pos.z))&&placeableTile.Contains(new Vector3Int(pos.x-1,pos.y+1,pos.z)))
        {
            placeableTile.Remove(new Vector3Int(pos.x-1,pos.y+1,pos.z));
            notPlaceableTile.Add(new Vector3Int(pos.x-1,pos.y+1,pos.z));
        }
        if(tileMap[1].GetTile(new Vector3Int(pos.x-1,pos.y-1,pos.z))&&placeableTile.Contains(new Vector3Int(pos.x-1,pos.y-1,pos.z)))
        {
            placeableTile.Remove(new Vector3Int(pos.x-1,pos.y-1,pos.z));
            notPlaceableTile.Add(new Vector3Int(pos.x-1,pos.y-1,pos.z));
        }
    }
}
