using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Holdable_Items : MonoBehaviour, IInteractable
{
    public string textOnHover = "Press E To Pick Up";
    public new Rigidbody rigidbody;
    private bool isInteractable = true;
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }
    public virtual void Interact()
    {
        if (!isInteractable) return;
        Debug.Log("SeedInteracted" + gameObject);

        Player_Hold_Manager.instance.PickUpItem(this);

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
