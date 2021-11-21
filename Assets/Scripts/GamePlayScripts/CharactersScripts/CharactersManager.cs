using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersManager : MonoBehaviour
{
    private static CharactersManager instance;
    public static CharactersManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CharactersManager>();
            }
            return instance;
        }
    }

    private List<GameObject> waiters;
    private GameObject waiter;


    [SerializeField]
    private GameObject waiterPrefab;

    
    public void HireWaiters()
    {
        waiter=Instantiate(waiterPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        waiters.Add(waiter);    
        waiter.GetComponentInChildren<WaitersScript>().GetPath(new Vector3(0, -1.5f, 0));
    }

    public void SelectWaiter()
    {

    }
}
