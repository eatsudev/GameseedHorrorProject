using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flower : Base_Holdable_Items
{
    public bool isBurning = false;
    public override void CheckOutline()
    {
        base.CheckOutline();

        if (isBurning)
        {
            outlineDistance = 0f;
        }
    }

    public override void Interact()
    {
        base.Interact();

    }
}