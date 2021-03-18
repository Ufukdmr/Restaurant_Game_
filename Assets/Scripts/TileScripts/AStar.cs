using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;


public class AStar : MonoBehaviour
{

    [SerializeField]
    private Tilemap tileMap;

    [SerializeField]
    private Tile[] tiles;

    [SerializeField]
    private LayerMask layerMask;
    [SerializeField]
    Camera camera;

    private Vector3Int startPos, goalPos;

    private Node current;

    private HashSet<Node> openList;

    private HashSet<Node> closedList;

    private Stack<Vector3> path;

    private Dictionary<Vector3Int, Node> allNodes = new Dictionary<Vector3Int, Node>();

    private static HashSet<Vector3Int> notWalkableTile = new HashSet<Vector3Int>();

    public static HashSet<Vector3Int> NotWalkableTile { get => notWalkableTile; }

    private static HashSet<Vector3Int> groundTile = new HashSet<Vector3Int>();

    public static HashSet<Vector3Int> GroundTile { get => groundTile; }




    void Start()
    {

    }

    public Stack<Vector3> Algorithm(Vector3 start, Vector3 goal)
    {

        startPos = tileMap.WorldToCell(start);
        goalPos = tileMap.WorldToCell(goal);

        goalPos = new Vector3Int(goalPos.x, goalPos.y, startPos.z);



        current = GetNode(startPos);
        openList = new HashSet<Node>();
        closedList = new HashSet<Node>();

        openList.Add(current);
        path = null;

        while (openList.Count > 0 && path == null)
        {
            List<Node> neighbors = FindNeighbors(current.Position);

            ExamineNeighbors(neighbors, current);

            UpdateCurrentTile(ref current);

            path = GeneratePath(current);

        }

        if (path != null)
        {

            return path;

        }
        return null;

    }

    private List<Node> FindNeighbors(Vector3Int parentPosition)
    {
        List<Node> neighbors = new List<Node>();
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                Vector3Int neighborPos = new Vector3Int(parentPosition.x - x, parentPosition.y - y, parentPosition.z);
                if (y != 0 || x != 0)
                {
                    if (neighborPos != startPos && !NotWalkableTile.Contains(neighborPos) && tileMap.GetTile(neighborPos))
                    {
                        Node neighbor = GetNode(neighborPos);
                        neighbors.Add(neighbor);
                    }

                }
            }
        }
        return neighbors;
    }

    private void ExamineNeighbors(List<Node> neighbors, Node current)
    {
        for (int i = 0; i < neighbors.Count; i++)
        {

            Node neighbor = neighbors[i];

            int gScore = DetermineGScore(neighbors[i].Position, current.Position);


            if (gScore == 14)
            {
                continue;
            }
            if (openList.Contains(neighbor))
            {
                if (current.G + gScore < neighbor.G)
                {
                    CalcValues(current, neighbor, gScore);
                }

            }
            else if (!closedList.Contains(neighbor))
            {
                CalcValues(current, neighbor, gScore);
                openList.Add(neighbor);
            }
        }
    }

    private void CalcValues(Node parent, Node neighbor, int cost)
    {
        neighbor.Parrent = parent;

        neighbor.G = parent.G + cost;

        neighbor.H = ((Mathf.Abs(neighbor.Position.x - goalPos.x)) + (Mathf.Abs(neighbor.Position.y - goalPos.y)) * 10);
        neighbor.F = neighbor.G + neighbor.H;
    }

    private int DetermineGScore(Vector3Int neighbor, Vector3Int current)
    {
        int gScore = 0;

        int x = current.x - neighbor.x;
        int y = current.y - neighbor.y;

        if (Mathf.Abs(x - y) % 2 == 1)
        {
            gScore = 10;
        }
        else
        {
            gScore = 14;
        }
        return gScore;
    }
    private void UpdateCurrentTile(ref Node current)
    {
        openList.Remove(current);
        closedList.Add(current);

        if (openList.Count > 0)
        {
            current = openList.OrderBy(x => x.F).First();
        }
    }
    private Node GetNode(Vector3Int position)
    {
        if (allNodes.ContainsKey(position))
        {
            return allNodes[position];
        }
        else
        {
            Node node = new Node(position);
            allNodes.Add(position, node);
            return node;
        }
    }


    public Stack<Vector3> GeneratePath(Node current)
    {

        if (current.Position == goalPos)
        {
            Stack<Vector3> finalPath = new Stack<Vector3>();

            while (current.Position != startPos)
            {

                finalPath.Push(tileMap.CellToWorld(current.Position));

                current = current.Parrent;
            }

            return finalPath;
        }

        return null;
    }

    public void ChangeTileAstar(Vector3Int clickPos)
    {
        tileMap.SetTile(clickPos, tiles[1]);
        TileMapManager.MyInstance.CreateTiles();
    }

    public void TileMapChange()
    {
        TileMapManager.MyInstance.Initialize(GroundTile, NotWalkableTile);
    }
}
