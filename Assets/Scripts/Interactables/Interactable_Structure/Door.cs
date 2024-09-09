using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Base_Interactable_Structure
{
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;

    private Animator animator;
    private Collider collider;
    private AudioSource audioSource;

    private bool isOpen;
    

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
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

        audioSource.clip = openDoorClip;
        audioSource.Play();

        StartCoroutine(DisableColliderDuringAnimation());
        
    }

    public void CloseDoor()
    {
        animator.SetTrigger("CloseDoor");

        audioSource.clip = closeDoorClip;
        audioSource.Play();

        StartCoroutine(DisableColliderDuringAnimation());
    }

    private IEnumerator DisableColliderDuringAnimation()
    {
        collider.enabled = false;

        yield return new WaitForSeconds(1f);

        collider.enabled = true;
    }
}
