using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Experimental.GlobalIllumination;

public class Player_FlashLight_Manager : MonoBehaviour
{
    public List<Light> flashLights; //flashLight will now be shortened to FL for the rest of the code
    public AudioClip toggleFLClip;
    public List<Image> Charge;
    public HorizontalLayoutGroup chargeHorizontalLayoutGroup;
    public TextMeshProUGUI warningText;
    public AudioSource flashlightSFX;
    public Animator animator;

    private Stunning_Enemy_Manager stunning_Enemy_Manager; 

    [SerializeField] private float maxFLCharge;
    [SerializeField] private float FLCharge_DecreaseRate;
    private float currFLCharge;

    private List<float> FLOriginalIntensity =  new List<float>(); 

    private bool isFLActive = false;
    void Start()
    {
        stunning_Enemy_Manager = Entities_Manager.Instance.player.GetComponent<Stunning_Enemy_Manager>();

        currFLCharge = maxFLCharge;

        StartCoroutine(Initialize());
        UpdateChargeDisplay();

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.ToggleFlashLight.performed += ToggleFL;
        playerInputActions.Player.ReloadFlashlight.performed += OnReload;

        foreach (Light light in flashLights)
        {
            light.enabled = false;
            FLOriginalIntensity.Add(light.intensity);
        }
        
    }

    void Update()
    {
        ManageCharge();
    }

    private IEnumerator Initialize()
    {
        chargeHorizontalLayoutGroup.enabled = true;
        yield return new WaitForSeconds(0.1f);
        chargeHorizontalLayoutGroup.enabled = false;

        UpdateChargeDisplay();
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
            BatteryLowWarning();
            UpdateChargeDisplay();
        }
    }

    private void OnReload(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Battery battery = Player_Hold_Manager.instance.GetHeldItem().GetComponent<Battery>();
            if (battery != null)
            {
                if(currFLCharge >= 95)
                {
                    Player_Hold_Manager.instance.WarningOnItem("Battery is still Full");
                    return;
                }
                Player_Hold_Manager.instance.ItemUsed();
                StartCoroutine(BatteryReloaded());

                
            }
            else
            {
                warningText.enabled = false;
                Player_Hold_Manager.instance.WarningOnItem("Need Battery");
            }
        }
    }

    private IEnumerator BatteryReloaded()
    {


        yield return new WaitForSeconds(0.3f);

        currFLCharge = maxFLCharge;
        Debug.Log(currFLCharge);
        UpdateChargeDisplay();
    }
    private void ToggleFL(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (stunning_Enemy_Manager.isStunning())
            {
                return;
            }

            if (isFLActive)
            {
                TurnOffFL();
            }
            else
            {
                TurnOnFL();
            }

            animator.SetTrigger("ToggleFL");
        }
    }

    private void TurnOnFL()
    {
        foreach (Light light in flashLights)
        {
            light.enabled = true;
        }
        isFLActive = true;
        flashlightSFX.Play();
    }

    private void TurnOffFL()
    {
        foreach (Light light in flashLights)
        {
            light.enabled = false;
        }
        isFLActive = false;
        flashlightSFX.Play();
    }

    public void MultiplyIntensity(float multiplyIntensity)
    {
        foreach (Light light in flashLights)
        {
            light.intensity *= multiplyIntensity;
        }
    }

    public void ResetIntensity()
    {
        int i = 0;
        foreach (Light light in flashLights)
        {
            light.intensity = FLOriginalIntensity[i];
            i += 1;
        }
    }

    public float DecreaseRate()
    {
        return FLCharge_DecreaseRate;
    }

    public void ChangeDecreaseRate(float newDecreaseRate)
    {
        FLCharge_DecreaseRate = newDecreaseRate;
    }

    #endregion

    #region Visual Indicator
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

        foreach (Image image in Charge)
        {
            image.gameObject.SetActive(true);
        }
        Debug.Log(amountOfSections);

        for (int i = 0; i < amountOfSections; i++)
        {
            Debug.Log(i);
            Charge[i].gameObject.SetActive(false);
        }
    }

    private void BatteryLowWarning()
    {
        if (currFLCharge <= 25)
        {
            warningText.gameObject.SetActive(true);
        }
        else
        {
            warningText.gameObject.SetActive(false);
        }
    }
    #endregion

    #region Getting FlashLight Variables

    public bool IsFLActive() { return isFLActive; }

    public float CurrentFLCharge() { return currFLCharge; }

    #endregion
}
