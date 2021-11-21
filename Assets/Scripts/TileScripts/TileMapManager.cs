using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TileType { Table4, Table3U, Table3D, Table3L, Table3R, Debug, Null }
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
    HashSet<TileControl> TileControls;

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

    public void Initialize(HashSet<Vector3Int> groundTile, HashSet<Vector3Int> notWalkableTiles, HashSet<Vector3Int> wallTile, HashSet<TileControl> tileControls)
    {
        placeableTile = new HashSet<Vector3Int>();
        notPlaceableTile = new HashSet<Vector3Int>();
        TileControls = new HashSet<TileControl>();
        isInitialize = true;

        if (tileType == TileType.Table4 || tileType == TileType.Table3U || tileType == TileType.Table3D || tileType == TileType.Table3L || tileType == TileType.Table3R)
        {

            foreach (TileControl tile in tileControls)
            {
                TileControls.Add(tile);
                if (notWalkableTiles.Contains(tile.Position))
                {
                    notPlaceableTile.Add(tile.Position);

                    if (tileMap[0].GetTile(tile.Position))
                    {
                         Debug.Log("Bok");
                        for (int x = -1; x <= 1; x++)
                        {
                            for (int y = -1; y <= 1; y++)
                            {
                                if (tileMap[2].GetTile(new Vector3Int(tile.Position.x + x, tile.Position.y + y, tile.Position.z)))
                                {                                   
                                    notPlaceableTile.Add(new Vector3Int(tile.Position.x + x, tile.Position.y + y, tile.Position.z));
                                }
                                if (tileMap[2].GetTile(new Vector3Int(tile.Position.x - x, tile.Position.y - y, tile.Position.z)))
                                {
                                    notPlaceableTile.Add(new Vector3Int(tile.Position.x - x, tile.Position.y - y, tile.Position.z));
                                }
                            }
                        }
                    }
                }
                else if (groundTile.Contains(tile.Position))
                {
                    if (tile.I != 2)
                    {
                        if (!notPlaceableTile.Contains(tile.Position))
                        {
                            if (tileType == TileType.Table4)
                            {

                                if (!tileMap[3].GetTile(tile.Position) && !tileMap[4].GetTile(tile.Position))
                                {
                                    placeableTile.Add(tile.Position);
                                }
                                else
                                {
                                    notPlaceableTile.Add(tile.Position);
                                }
                            }
                            else if (tileType == TileType.Table3U)
                            {

                                if (!tileMap[2].GetTile(new Vector3Int(tile.Position.x, tile.Position.y - 1, tile.Position.z)) || !tileMap[2].GetTile(new Vector3Int(tile.Position.x + 1, tile.Position.y, tile.Position.z)) || !tileMap[2].GetTile(new Vector3Int(tile.Position.x - 1, tile.Position.y, tile.Position.z)))
                                {
                                    notPlaceableTile.Add(tile.Position);
                                }
                                else
                                {
                                    placeableTile.Add(tile.Position);
                                }
                            }
                            else if (tileType == TileType.Table3D)
                            {

                                if (!tileMap[2].GetTile(new Vector3Int(tile.Position.x, tile.Position.y + 1, tile.Position.z)) || !tileMap[2].GetTile(new Vector3Int(tile.Position.x + 1, tile.Position.y, tile.Position.z)) || !tileMap[2].GetTile(new Vector3Int(tile.Position.x - 1, tile.Position.y, tile.Position.z)))
                                {
                                    notPlaceableTile.Add(tile.Position);
                                }
                                else
                                {
                                    placeableTile.Add(tile.Position);
                                }
                            }
                            else if (tileType == TileType.Table3L)
                            {

                                if (!tileMap[2].GetTile(new Vector3Int(tile.Position.x, tile.Position.y - 1, tile.Position.z)) || !tileMap[2].GetTile(new Vector3Int(tile.Position.x, tile.Position.y + 1, tile.Position.z)) || !tileMap[2].GetTile(new Vector3Int(tile.Position.x + 1, tile.Position.y, tile.Position.z)))
                                {
                                    notPlaceableTile.Add(tile.Position);
                                }
                                else
                                {
                                    placeableTile.Add(tile.Position);
                                }
                            }
                            else if (tileType == TileType.Table3R)
                            {

                                if (!tileMap[2].GetTile(new Vector3Int(tile.Position.x, tile.Position.y - 1, tile.Position.z)) || !tileMap[2].GetTile(new Vector3Int(tile.Position.x, tile.Position.y + 1, tile.Position.z)) || !tileMap[2].GetTile(new Vector3Int(tile.Position.x - 1, tile.Position.y, tile.Position.z)))
                                {
                                    notPlaceableTile.Add(tile.Position);
                                }
                                else
                                {
                                    placeableTile.Add(tile.Position);
                                }
                            }
                        }
                    }
                    else
                    {

                        notPlaceableTile.Add(tile.Position);
                    }
                }

            }
        }


        CreateTiles();
    }
    public void ChangeTileType(TileButton button)
    {
        if (!isInitialize)
        {
            tileType = button.MyTileType;
            aStar.TileMapChange();
        }
        else
        {
            if (tileType != button.MyTileType)
            {
                tileType = button.MyTileType;
                aStar.TileMapChange();
            }
            else
            {
                tileType = button.MyTileType;
                CreateTiles();
            }

        }

    }
    private void ChangeTile(Vector3Int clickPos)
    {
        if (GameManager.MyInstance.Editmode)
        {
            foreach (TileControl tileControl in TileControls)
            {
                if (tileControl.Position == clickPos)
                {
                    if (placeableTile.Contains(tileControl.Position))
                    {
                        if (tileType == TileType.Table4)
                        {

                            tileMap[0].SetTile(clickPos, tile[0]);
                            aStar.ChangeTileAstar(clickPos);
                            AdjustmentList(clickPos);
                            CreateTiles();
                            break;
                        }
                        if (tileType == TileType.Table3U)
                        {

                            tileMap[0].SetTile(clickPos, tile[1]);
                            aStar.ChangeTileAstar(clickPos);
                            AdjustmentList(clickPos);
                            CreateTiles();
                            break;
                        }
                        if (tileType == TileType.Table3D)
                        {

                            tileMap[0].SetTile(clickPos, tile[2]);
                            aStar.ChangeTileAstar(clickPos);
                            AdjustmentList(clickPos);
                            CreateTiles();
                            break;
                        }
                        if (tileType == TileType.Table3L)
                        {

                            tileMap[0].SetTile(clickPos, tile[3]);
                            aStar.ChangeTileAstar(clickPos);
                            AdjustmentList(clickPos);
                            CreateTiles();
                            break;
                        }
                        if (tileType == TileType.Table3R)
                        {

                            tileMap[0].SetTile(clickPos, tile[4]);
                            aStar.ChangeTileAstar(clickPos);
                            AdjustmentList(clickPos);
                            CreateTiles();
                            break;
                        }
                    }

                }

            }
        }


    }

    public void ColorTile(Vector3Int position, Color color)
    {
        tileMap[1].SetTile(position, tile[5]);
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


        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (tileMap[1].GetTile(new Vector3Int(pos.x + x, pos.y + y, pos.z)) && placeableTile.Contains(new Vector3Int(pos.x + x, pos.y + y, pos.z)))
                {

                    placeableTile.Remove(new Vector3Int(pos.x + x, pos.y + y, pos.z));
                    notPlaceableTile.Add(new Vector3Int(pos.x + x, pos.y + y, pos.z));

                }
                if (tileMap[1].GetTile(new Vector3Int(pos.x - x, pos.y - y, pos.z)) && placeableTile.Contains(new Vector3Int(pos.x - x, pos.y - y, pos.z)))
                {
                    placeableTile.Remove(new Vector3Int(pos.x - x, pos.y - y, pos.z));
                    notPlaceableTile.Add(new Vector3Int(pos.x - x, pos.y - y, pos.z));
                }
            }

        }

    }

    public void ColoredTileMap()
    {
        foreach (TileControl tileControl in TileControls)
        {
            tileMap[1].SetTile(tileControl.Position, null);
        }
        tileType = TileType.Null;
        isInitialize = false;
    }
}
