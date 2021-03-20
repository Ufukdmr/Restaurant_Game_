﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Characters : MonoBehaviour
{
    private Stack<Vector3> path;
    private Vector3 destination;
    private Vector3 goal;

    [SerializeField]
    private AStar astar;

    [SerializeField]
    private Tilemap tilemap;

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

    private void ClickMove()
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
