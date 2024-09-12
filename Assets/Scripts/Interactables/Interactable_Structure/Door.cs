using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : Base_Interactable_Structure
{
    public AudioClip lockedClip;
    public AudioClip unlockedClip;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;
    public Key key;

    private Animator animator;
    private Collider collider;
    private AudioSource audioSource;

    private bool isOpen;
    private bool isLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        if (key)
        {
            isLocked = true;
        }

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
        if(isLocked)
        {
            Key heldKey = Player_Hold_Manager.instance.GetHeldItem().GetComponent<Key>();

            if (!heldKey)
            {
                Locked();

                Player_Hold_Manager.instance.WarningOnItem("Need Key");
                return;
            }

            if(heldKey != key)
            {
                Locked();

                Player_Hold_Manager.instance.WarningOnItem("Wrong Key");
                return;
            }

            if(heldKey == key)
            {
                UnlockDoor();

                Player_Hold_Manager.instance.ItemUsed();
                Player_Hold_Manager.instance.WarningOnItem("Door Unlocked");
                return;
            }
        }

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

    private void UnlockDoor()
    {
        animator.SetTrigger("Unlocked");
        audioSource.PlayOneShot(unlockedClip);

        isLocked = false;
    }

    private void Locked()
    {
        animator.SetTrigger("Locked");
        audioSource.PlayOneShot(lockedClip);
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
