using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class Door : Base_Interactable_Structure
{
    public AudioClip lockedClip;
    public AudioClip unlockedClip;
    public AudioClip openDoorClip;
    public AudioClip closeDoorClip;
    public Key key;
    public PadLock padLock;

    private Animator animator;
    private Collider collider;
    private AudioSource audioSource;
    private Animator handAnimator;

    private bool isOpen;
    private bool isLocked = false;
    // Start is called before the first frame update
    void Start()
    {
        if (key || padLock)
        {
            isLocked = true;

            if (padLock)
            {
                isInteractable = false;
                padLock.door = this;
            }
        }

        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        handAnimator = Player_Hold_Manager.instance.leftHandAnimator;

        audioSource = GetComponent<AudioSource>();
        if (!audioSource)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.outputAudioMixerGroup = AudioManager.instance.sfx;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Interact()
    {
        base.Interact();
        
        StartCoroutine(InteractAnimationProcess());

        if (isLocked && !padLock)
        {
            Base_Holdable_Items heldItem = Player_Hold_Manager.instance.GetHeldItem();

            if (!heldItem)
            {
                Locked();

                Player_Hold_Manager.instance.WarningOnItem("I need a Key");
                return;
            }

            Key heldKey = heldItem.GetComponent<Key>();

            if (!heldKey)
            {
                Locked();

                Player_Hold_Manager.instance.WarningOnItem("I need a Key");
                return;
            }

            if(heldKey != key)
            {
                Locked();

                Player_Hold_Manager.instance.WarningOnItem("This Key doesn't match");
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

        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }

    private IEnumerator InteractAnimationProcess()
    {
        handAnimator.SetTrigger("InteractDoor");

        yield return null;

        Base_Holdable_Items heldItem = Player_Hold_Manager.instance.GetHeldItem();

        if (heldItem)
        {
            heldItem.gameObject.SetActive(false);

            yield return new WaitForSeconds(0.479f);

            heldItem.gameObject.SetActive(true);
        }

        
    }
    private void UnlockDoor()
    {
        animator.SetTrigger("Unlocked");
        audioSource.PlayOneShot(unlockedClip);

        Unlock();
    }

    public void Unlock()
    {
        isLocked = false;
        isInteractable = true;
    }

    private void Locked()
    {
        animator.SetTrigger("Locked");
        audioSource.PlayOneShot(lockedClip);
    }

    public void OpenDoor()
    {
        isOpen = true;

        animator.SetTrigger("OpenDoor");

        audioSource.clip = openDoorClip;
        audioSource.Play();

        StartCoroutine(DisableColliderDuringAnimation());
        
    }

    public void CloseDoor()
    {
        isOpen = false;

        animator.SetTrigger("CloseDoor");

        audioSource.clip = closeDoorClip;
        audioSource.Play();

        StartCoroutine(DisableColliderDuringAnimation());
    }

    private IEnumerator DisableColliderDuringAnimation()
    {
        if (collider)
        {
            collider.enabled = false;
        }
        

        yield return new WaitForSeconds(0.5f);

        if (collider)
        {
            collider.enabled = true;
        }
        
    }
}
