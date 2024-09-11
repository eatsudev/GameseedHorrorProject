using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunning_Enemy_Manager : MonoBehaviour
{
    private Player_FlashLight_Manager player_FlashLight_Manager;

    public float stunModeLength;


    private bool isStunMode = false;

    // Start is called before the first frame update
    void Start()
    {
        player_FlashLight_Manager = Entities_Manager.Instance.player.GetComponent<Player_FlashLight_Manager>();
    }

    void Update()
    {
        
    }
}
