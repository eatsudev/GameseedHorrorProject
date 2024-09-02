using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Holdable_Items : MonoBehaviour, IInteractable
{
    public string textOnHover = "Press E To Pick Up";
    public new Rigidbody rigidbody;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public virtual void Interact()
    {
        Debug.Log("SeedInteracted" + gameObject);

        Player_Hold_Manager.instance.PickUpItem(this);

    }
    public virtual Base_Holdable_Items Get_Base_Holdable_Item()
    {
        return this;
    }

    public void ActivateGravity()
    {
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void DeactivateGravity()
    {
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }
}
