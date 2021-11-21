using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private LayerMask walkableLayer, clickableLayer,players;

    [SerializeField]
    private CharactersManager player;

    [SerializeField]
    private GameObject WaitersPrefab;

    public bool Editmode = false;

    private static GameManager instance;
    public static GameManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
            }
            return instance;
        }
    }
    void Start()
    {

    }

    void Update()
    {

        if (Input.GetMouseButtonDown(0) && !Editmode)
        {

            // RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, walkableLayer);

            // if (hit.collider != null)
            // {
            //     player.GetPath(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            // }
            // else
            // {               
            RaycastHit2D hit = Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, players);

            if (hit.collider != null)
            {
                if(player!=null)
                {
                    Debug.Log(hit.collider.)
                }
            
            }
            // }
        }
        if (Editmode)
        {
            if(player!=null)
            {
                // player.GetPath(new Vector3(0, -1.5f, 0));
            }
           
        }

    }
   
}
