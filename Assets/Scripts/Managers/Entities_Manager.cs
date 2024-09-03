using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entities_Manager : MonoBehaviour
{
    public static Entities_Manager Instance;

    public Player_Entity player;
    public Enemy_Entity enemy;
    public Ritual_Circle ritual_Circle;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
}
