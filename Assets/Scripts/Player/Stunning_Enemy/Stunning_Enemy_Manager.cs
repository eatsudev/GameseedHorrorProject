using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stunning_Enemy_Manager : MonoBehaviour
{
    private Player_FlashLight_Manager player_FlashLight_Manager;
    private Enemy_Entity enemy;

    public float stunModeLength = 3f;
    public float stunEnemyLength = 5f;

    private bool isStunMode = false;

    // Start is called before the first frame update
    void Start()
    {
        player_FlashLight_Manager = Entities_Manager.Instance.player.GetComponent<Player_FlashLight_Manager>();

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.StunFlashLightToggle.performed += ToggleStun;
    }

    void Update()
    {
        
    }

    private void ToggleStun(InputAction.CallbackContext ctx)
    {
        if (isStunMode)
        {
            return;
        }

        if(ctx.performed)
        {
            StartStun();
        }

    }

    private void StartStun()
    {
        StartCoroutine(StunProcess());
    }

    private IEnumerator StunProcess()
    {
        isStunMode = true;

        float temp = 0f;

        while(temp < stunModeLength)
        {
            if (!enemy.IsStunned())
            {
                enemy.ActivateStun();
            }

            temp += Time.deltaTime;
            yield return new WaitForSeconds(stunModeLength);
        }


        isStunMode = false;
    }
}
