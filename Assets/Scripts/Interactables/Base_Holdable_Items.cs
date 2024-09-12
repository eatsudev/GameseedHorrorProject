using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Base_Holdable_Items : MonoBehaviour, IInteractable
{
    public string textOnHover = "E To Pick Up";
    public Rigidbody rigidbody;
    public Vector3 rotationOnHold = Vector3.zero;
    public bool isInteractable = true;
    public float outlineDistance = 5f;

    private Player_Entity player;
    private Outline outline;
    public virtual void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        player = Entities_Manager.Instance.player;

        outline = GetComponent<Outline>();

        if(outline == null)
        {
            Debug.Log(gameObject + " Missing Outline");
        }

        outline.OutlineWidth = outlineDistance;
        outline.enabled = false;
    }

    private void FixedUpdate()
    {
        if(Vector3.Distance(player.transform.position, transform.position) < outlineDistance)
        {
            outline.enabled = true;
        }
        else
        {
            outline.enabled = false;
        }
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

    public virtual void ActivateRigidBody()
    {
        rigidbody.useGravity = true;
        rigidbody.constraints = RigidbodyConstraints.None;
    }

    public virtual void DeactivateRigidBody()
    {
        rigidbody.useGravity = false;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        transform.localEulerAngles = rotationOnHold;
    }
}
