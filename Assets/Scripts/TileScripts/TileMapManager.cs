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

    HashSet<TileControl> placeableTile;
    HashSet<TileControl> notPlaceableTile;

    private bool isInitialize = false;

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

    public void Initialize(HashSet<TileControl> groundTile, HashSet<TileControl> notWalkableTiles, HashSet<TileControl> wallTile)
    {
        placeableTile = new HashSet<TileControl>();
        notPlaceableTile = new HashSet<TileControl>();
        isInitialize = true;

        if (tileType == TileType.Table)
        {
            foreach (TileControl tile in groundTile)
            {
                if (tile.I != 2)
                {
                    placeableTile.Add(tile);
                }
                else
                {
                    notPlaceableTile.Add(tile);
                }
            }
            foreach (TileControl tile in wallTile)
            {
                if (tile.I != 2)
                {
                    placeableTile.Add(tile);
                }
                else
                {
                    notPlaceableTile.Add(tile);
                }
            }
            foreach (TileControl tile in notWalkableTiles)
            {
                notPlaceableTile.Add(tile);

            }
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

        foreach (TileControl tileControl in placeableTile)
        {
            if (tileControl.Position == clickPos)
            {
               
                if (tileType == TileType.Table)
                {

                    tileMap[0].SetTile(clickPos, tile[0]);
                    aStar.ChangeTileAstar(clickPos);
                    AdjustmentList(new TileControl(clickPos, tileControl.I));
                    CreateTiles();
                    break;
                }
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
        foreach (TileControl tile in placeableTile)
        {
            ColorTile(tile.Position, placeable);
        }

        foreach (TileControl tile in notPlaceableTile)
        {
            ColorTile(tile.Position, notPlaceable);
        }
    }

    void AdjustmentList(TileControl tileControl)
    {
        placeableTile.Remove(tileControl);
        notPlaceableTile.Add(tileControl);
      

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <=1; y++)
            {
                for (int k = 0; k < 3; k++)
                {
                    if (tileMap[1].GetTile(new Vector3Int(tileControl.Position.x + x, tileControl.Position.y + y, tileControl.Position.z)) && placeableTile.Contains(new TileControl(new Vector3Int(tileControl.Position.x + x, tileControl.Position.y + y, tileControl.Position.z), k)))
                    {
                        Debug.Log(x+","+y);
                        placeableTile.Remove(new TileControl(new Vector3Int(tileControl.Position.x + x, tileControl.Position.y + y, tileControl.Position.z), k));
                        notPlaceableTile.Add(new TileControl(new Vector3Int(tileControl.Position.x + x, tileControl.Position.y + y, tileControl.Position.z), k));
                      
                    }
                    if (tileMap[1].GetTile(new Vector3Int(tileControl.Position.x - x, tileControl.Position.y - y, tileControl.Position.z)) && placeableTile.Contains(new TileControl(new Vector3Int(tileControl.Position.x - x, tileControl.Position.y - y, tileControl.Position.z), k)))
                    {
                        placeableTile.Remove(new TileControl(new Vector3Int(tileControl.Position.x - x, tileControl.Position.y - y, tileControl.Position.z), k));
                        notPlaceableTile.Add(new TileControl(new Vector3Int(tileControl.Position.x - x, tileControl.Position.y - y, tileControl.Position.z), k));
                       
                    }
                }

            }

        }
        
    }
}
