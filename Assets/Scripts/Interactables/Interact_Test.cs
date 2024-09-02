using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Test : MonoBehaviour, IInteractable
{
    void Start()
    {
        
    }

    public void Interact()
    {
        Debug.Log("Interacted" + gameObject);
    }
    public Base_Holdable_Items Get_Base_Holdable_Item()
    {
        return null;
    }
}
