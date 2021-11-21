using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    Animator animator;
    bool Edit = false;

    void Start()
    {

    }


    void Update()
    {

    }

    public void EditAnimStart()
    {
        if (!Edit)
        {

            animator.SetBool("Edit", true);
            GameManager.MyInstance.Editmode = true;
            Edit = true;

        }
        else
        {
            animator.SetBool("Edit", false);
            TileMapManager.MyInstance.ColoredTileMap();
            GameManager.MyInstance.Editmode = false;
            Edit = false;

        }

    }

}
