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

    }

    void Update()
    {
        
    }
    public override void Interact()
    {
        base.Interact();

        Base_Holdable_Items playerHoldedItem = Player_Hold_Manager.instance.ItemHolded();
        Flower flower = playerHoldedItem.GetComponent<Flower>();

        if (flower)
        {
            
        }
        else
        {
            
        }
    }

    private IEnumerator BurnFlower()
    {
        Debug.Log("Burning Flower");
        isInteractable = false;

        flowerIsBurning.Invoke();

        yield return new WaitForSeconds(4);

        Debug.Log("Flower Burned");
        isInteractable = true;


    }
}
