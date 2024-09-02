using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Holdable_Items : MonoBehaviour, IInteractable
{
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Interact()
    {
        Debug.Log("SeedInteracted" + gameObject);

    }
}
