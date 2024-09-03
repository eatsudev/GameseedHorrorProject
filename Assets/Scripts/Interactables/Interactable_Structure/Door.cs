using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Base_Interactable_Structure
{
    private Animator animator;
    private new Collider collider;

    private bool isOpen;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        base.Interact();
        if (!animator)
        {
            Debug.Log("Door have no animation lah");
            return;
        }

        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    public void OpenDoor()
    {
        animator.SetTrigger("OpenDoor");
        StartCoroutine(DisableColliderDuringAnimation());
    }

    public void CloseDoor()
    {
        animator.SetTrigger("CloseDoor");
        StartCoroutine(DisableColliderDuringAnimation());
    }

    private IEnumerator DisableColliderDuringAnimation()
    {
        collider.enabled = false;

        yield return new WaitForSeconds(1f);

        collider.enabled = true;
    }
}
