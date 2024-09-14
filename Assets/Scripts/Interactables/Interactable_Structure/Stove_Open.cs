using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stove_Open : Base_Interactable_Structure
{
    private bool isOpen;
    private Animator anim;
    private Collider coli;
    private Animator handAnimator;
    public AudioSource openSFX;
    public AudioSource closeSFX;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        coli = GetComponent<Collider>();

        openSFX = GetComponent<AudioSource>();
        closeSFX = GetComponent<AudioSource>();

        handAnimator = Player_Hold_Manager.instance.leftHandAnimator;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interact()
    {
        base.Interact();
        if (!anim)
        {
            Debug.Log("no animation");
            return;
        }

        StartCoroutine(InteractAnimationProcess());

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
        heldItem.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.479f);

        heldItem.gameObject.SetActive(true);
    }

    public void OpenDoor()
    {
        isOpen = true;
        anim.SetTrigger("Open");
        openSFX.Play();
        StartCoroutine(DisableColliderDuringAnimation());
    }

    public void CloseDoor()
    {
        isOpen = false;
        anim.SetTrigger("Close");
        closeSFX.Play();
        StartCoroutine(DisableColliderDuringAnimation());
    }

    private IEnumerator DisableColliderDuringAnimation()
    {
        if (coli)
        {
            coli.enabled = false;
        }

        yield return new WaitForSeconds(0.5f);

        if (coli)
        {
            coli.enabled = true;
        }
    }
}
