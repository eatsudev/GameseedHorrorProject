using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player_FlashLight_Manager : MonoBehaviour
{
    public Light flashLight; //flashLight will now be shortened to FL for the rest of the code
    public AudioClip toggleFLClip;
    public List<Image> Charge;
    public HorizontalLayoutGroup chargeHorizontalLayoutGroup;


    [SerializeField] private float maxFLCharge;
    [SerializeField] private float FLCharge_DecreaseRate;
    private float currFLCharge;


    private bool isFLActive = false;
    void Start()
    {
        currFLCharge = maxFLCharge;
        chargeHorizontalLayoutGroup.enabled = false;
        UpdateChargeDisplay();

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.ToggleFlashLight.performed += ToggleFL;

        flashLight.enabled = false;
    }

    void Update()
    {
        ManageCharge();
    }

    #region Managing FlashLight
    private void ManageCharge()
    {
        if (isFLActive)
        {
            if(currFLCharge > 0)
            {
                currFLCharge -= FLCharge_DecreaseRate * Time.deltaTime;
            }
            else
            {
                TurnOffFL();
            }
            UpdateChargeDisplay();
        }
    }

    private void UpdateChargeDisplay()
    {
        int count = Charge.Count;

        float chargePerSections = (maxFLCharge / count);

        int amountOfSections = Mathf.RoundToInt(((maxFLCharge - currFLCharge) / chargePerSections) - 0.5f);

        if (amountOfSections > count)
        {
            Debug.Log("ERROR, more sections to deactivate than there are sections");
            return;
        }



        for (int i = 0; i < amountOfSections; i++)
        {
            Debug.Log(i);
            Charge[i].gameObject.SetActive(false);
        }
    }

    private void ToggleFL(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (isFLActive)
            {
                TurnOffFL();
            }
            else
            {
                TurnOnFL();
            }
        }
    }

    private void TurnOnFL()
    {
        flashLight.enabled = true;
        isFLActive = true;
    }

    private void TurnOffFL()
    {
        flashLight.enabled = false;
        isFLActive = false;
    }

    public void SetFLIntensity(float newIntensity)
    {
        flashLight.intensity = newIntensity;
    }
    #endregion

    #region Getting FlashLight Variables

    public bool IsFLActive() { return isFLActive; }

    public float CurrentFLCharge() { return currFLCharge; }

    #endregion
}
