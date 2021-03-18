using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
  [SerializeField]
    private Camera mainCamera;

    [SerializeField]
    private LayerMask walkableLayer,clickableLayer;

    [SerializeField]
    private Characters player;
  
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
        if(Input.GetMouseButtonDown(0))
        {
             
            RaycastHit2D hit=Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,Mathf.Infinity,walkableLayer);
           
            if(hit.collider!=null)
            {               
                 
                player.GetPath(mainCamera.ScreenToWorldPoint(Input.mousePosition));
            }
            else
            {
                hit=Physics2D.Raycast(mainCamera.ScreenToWorldPoint(Input.mousePosition),Vector2.zero,Mathf.Infinity,clickableLayer);

                if(hit.collider!=null)
                {
                    player.GetPath(mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,(Input.mousePosition.y+1),Input.mousePosition.z)));
                }
            }
        }
    }
}
