using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGame_UI_Manager : MonoBehaviour
{
    public static InGame_UI_Manager Instance;

    public JumpscareUI jumpscareUI;
    public DeathScreenUI deathScreenUI;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        jumpscareUI = GetComponentInChildren<JumpscareUI>();
        deathScreenUI = GetComponentInChildren<DeathScreenUI>();
    }
}
