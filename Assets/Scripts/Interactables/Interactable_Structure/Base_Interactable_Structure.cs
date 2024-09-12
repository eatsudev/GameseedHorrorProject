using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Interactable_Structure : MonoBehaviour, IInteractable
{
    public string textOnHover = "E To Interact";
    public bool isInteractable = true;

    public bool IsInteractable()
    {
        return isInteractable;
    }
    public virtual void Interact()
    {
        if(!isInteractable)
        {
            return;
        }
    }
}
