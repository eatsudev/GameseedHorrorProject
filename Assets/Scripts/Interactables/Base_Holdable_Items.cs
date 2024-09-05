using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base_Holdable_Items : MonoBehaviour, IInteractable
{
    public string textOnHover = "E To Pick Up";
    public Rigidbody rigidbody;
    public Vector3 rotationOnHold = Vector3.zero;
    public bool isInteractable = true;
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

    public void ActivateRigidBody()
    {
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
    }

    public void DeactivateRigidBody()
    {
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        transform.localEulerAngles = rotationOnHold;
    }
}
