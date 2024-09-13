using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Stunning_Enemy_Manager : MonoBehaviour
{
    private Player_FlashLight_Manager player_FlashLight_Manager;
    private Enemy_Entity enemy;

    public float stunModeLength = 3f;
    public float stunEnemyLength = 5f;
    public float flashLightIntensityMultiplier = 5f;
    public float newDecreaseRateOnStunMode = 10f;
    public float maxDistance = 30f;
    public float stunningAngle = 60f;
    public LayerMask obstructionLayer;

    private float distance;
    private bool isStunMode = false;


    // Start is called before the first frame update
    void Start()
    {
        player_FlashLight_Manager = Entities_Manager.Instance.player.GetComponent<Player_FlashLight_Manager>();
        enemy = Entities_Manager.Instance.enemy;

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.StunFlashLightToggle.performed += ToggleStun;
    }

    void Update()
    {

    }

    public bool isStunning()
    {
        return isStunMode;
    }
    private void ToggleStun(InputAction.CallbackContext ctx)
    {
        if (isStunMode || !player_FlashLight_Manager.IsFLActive())
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
        Debug.Log("start stunning");
        isStunMode = true;

        float originDecreaseRate = player_FlashLight_Manager.DecreaseRate();

        player_FlashLight_Manager.MultiplyIntensity(flashLightIntensityMultiplier);
        player_FlashLight_Manager.ChangeDecreaseRate(newDecreaseRateOnStunMode);

        float currTime = 0f;

        while (currTime < stunModeLength)
        {
            if (!enemy.IsStunned())
            {
                if (DetectEnemyWithFOV())
                {
                    Debug.Log("enemy Stunned");
                    enemy.StartStun(stunEnemyLength);
                }
            }

            currTime += Time.deltaTime;
            yield return null;
        }

        player_FlashLight_Manager.ResetIntensity();
        player_FlashLight_Manager.ChangeDecreaseRate(originDecreaseRate);

        isStunMode = false;
        Debug.Log("done stunning");
    }

    private bool DetectEnemyWithFOV()
    {
        Vector3 dir = (enemy.transform.position - transform.position).normalized;

        distance = Vector3.Distance(enemy.transform.position, transform.position);        

        if(distance > maxDistance)
        {
            return false;
        }

        if (Vector3.Angle(transform.forward, dir) < stunningAngle / 2)
        {
            if (!Physics.Raycast(transform.position, dir, distance, obstructionLayer))
            {
                Debug.Log("Detected Player");
                return true;
            }
        }

        return false;
    }
}
