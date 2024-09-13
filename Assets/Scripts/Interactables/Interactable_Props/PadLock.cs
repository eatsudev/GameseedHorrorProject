using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PadLock : Base_Interactable_Structure
{
    public AudioClip lockedClip;
    public AudioClip unlockedClip;

    public Door door;
    public Key key;


    private AudioSource audioSource;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        if(key == null)
        {
            Debug.Log(gameObject + " Need Key");
        }

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        base.Interact();

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

        if (heldKey != key)
        {
            Locked();

            Player_Hold_Manager.instance.WarningOnItem("This Key doesn't match");
            return;
        }

        if (heldKey == key)
        {
            UnlockDoor();

            Player_Hold_Manager.instance.ItemUsed();
            Player_Hold_Manager.instance.WarningOnItem("Door Unlocked");
            return;
        }
    }

    private void UnlockDoor()
    {
        animator.SetTrigger("Unlocked");
        audioSource.PlayOneShot(unlockedClip);

        door.Unlock();

        Destroy(gameObject, 5f);
    }

    private void Locked()
    {
        animator.SetTrigger("Locked");
        audioSource.PlayOneShot(lockedClip);
    }
}
