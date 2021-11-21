using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WaitersScript : MonoBehaviour
{
    private Stack<Vector3> path;
    private Vector3 destination;
    private Vector3 goal;
  
    private AStar astar;
    public String Name { get; set; }
    public int weariness { get; set; }
    public Orders orders { get; set; }

  

    void Awake()
    {
        astar=GameObject.FindGameObjectWithTag("AStar").GetComponent<AStar>();
    }

    void Update()
    {
        ClickMove();
    }

    public void GetPath(Vector3 goal)
    {
        path = astar.Algorithm(transform.position, goal);
        if (path != null)
        {
            destination = path.Pop();
            this.goal = goal;
        }
    }
    public void ClickMove()
    {
        if (path != null)
        {        
            transform.parent.position = Vector2.MoveTowards(transform.parent.position, destination, 1 * Time.deltaTime);
            float distance = Vector2.Distance(destination, transform.parent.position);

            if (distance <= 0)
            {
                if (path.Count > 0)
                {
                    destination = path.Pop();
                }
                else
                {
                    path = null;
                    
                }
            }
        }
    }
}
