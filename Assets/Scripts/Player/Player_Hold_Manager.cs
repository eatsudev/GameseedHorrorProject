using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Hold_Manager : MonoBehaviour
{
    public static Player_Hold_Manager instance;
    private IInteractable holdItem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
