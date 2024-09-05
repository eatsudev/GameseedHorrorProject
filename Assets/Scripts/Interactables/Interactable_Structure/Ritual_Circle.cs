using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Events;

public class Ritual_Circle : Base_Interactable_Structure
{
    public ParticleSystem fireParticleSystem;

    private bool isBurningFlower;

    public UnityEvent flowerIsBurning;
    private void Awake()
    {
        if (flowerIsBurning == null)
        {
            flowerIsBurning = new UnityEvent();
        }
    }
    void Start()
    {
        fireParticleSystem.Stop();
    }

    void Update()
    {
        
    }
    public override void Interact()
    {
        base.Interact();

        Base_Holdable_Items playerHoldedItem = Player_Hold_Manager.instance.GetHeldItem();

        if (!playerHoldedItem)
        {
            Player_Hold_Manager.instance.WarningOnItem("Need Flower to Burn");
            return;
        }

        Flower flower = playerHoldedItem.GetComponent<Flower>();

        if (flower)
        {
            StartCoroutine(BurnFlower());
            Debug.Log("Burn");
        }
        else
        {
            Player_Hold_Manager.instance.WarningOnItem("Need Flower to Burn");
            return;
        }
    }

    private IEnumerator BurnFlower()
    {
        Debug.Log("Burning Flower");
        isInteractable = false;

        Player_Hold_Manager.instance.ItemUsed();

        flowerIsBurning.Invoke();

        fireParticleSystem.Play();

        yield return new WaitForSeconds(1);

        Debug.Log("Flower Burned");
        isInteractable = true;

        fireParticleSystem.Stop();
    }
}
