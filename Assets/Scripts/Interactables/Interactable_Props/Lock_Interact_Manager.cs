using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Lock_Interact_Manager : MonoBehaviour, IInteractable
{
    public AudioClip lockOpenClip;
    
    private Animator animator;
    private AudioSource audioSource;

    public float distanceFromCamera = 2f; // Set the distance from the player's camera
    public float moveDuration = 0.5f;     // Time for the movement to complete
    public float rotOffset = 36;
    public Vector3 targetFinalScale = new Vector3(0.2f, 0.2f, 0.2f);

    [Serializable]
    public class Dial
    {
        public LockDial lockDial;
        public int passNum;
    }

    public List<Dial> dials;

    public bool isInteractable = true;
    private bool isUnlocked = false;

    private Transform cameraTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;
    private Vector3 originalScale;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = AudioManager.instance.sfx;

        cameraTransform = Camera.main.transform;
        originalPosition = transform.position;
        originalRotation = transform.rotation;
        originalScale = transform.localScale;

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Escape.performed += QuitInteract;

        foreach (Dial dial in dials)
        {
            dial.lockDial.SetLockManager(this);
            dial.lockDial.isInteractable = false;
            dial.lockDial.transform.eulerAngles = new Vector3(dial.lockDial.transform.eulerAngles.x, dial.lockDial.transform.eulerAngles.y, rotOffset);
        }
    }

    // Update is called once per frame
    void Update()
    {


    }

    private IEnumerator MoveToFrontOfCamera()
    {
        Player_Interaction.instance.ChangeInteractType(1);
        isInteractable = false;


        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        Vector3 targetRotation = cameraTransform.eulerAngles;
        Vector3 targetScale = targetFinalScale;

        Vector3 startPosition = transform.position;
        Vector3 startRotation = transform.eulerAngles;
        Vector3 startScale = transform.localScale;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // Smoothly interpolate the position and rotation using Euler angles
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, elapsedTime / moveDuration);
            transform.localScale = Vector3.Lerp(startScale, targetScale, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }



        // Ensure the final position and rotation are exactly at the target
        transform.position = targetPosition;
        transform.eulerAngles = targetRotation;
        transform.localScale = targetScale;
    }

    // Coroutine to return the lock manager to its original position
    private IEnumerator ReturnToOriginalPos()
    {
        yield return null;
        Player_Interaction.instance.ChangeInteractType(0);

        Vector3 startPosition = transform.position;  // Current position in front of the camera
        Vector3 startRotation = transform.eulerAngles;  // Current rotation as Euler angles
        Vector3 startScale = transform.localScale;
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // Smoothly interpolate the position and rotation back to the original using Euler angles
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsedTime / moveDuration);
            transform.eulerAngles = Vector3.Lerp(startRotation, originalRotation.eulerAngles, elapsedTime / moveDuration);
            transform.localScale = Vector3.Lerp(startScale, originalScale, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are exactly at the original
        transform.position = originalPosition;
        transform.eulerAngles = originalRotation.eulerAngles;
        transform.localScale = transform.localScale;

        if (!isUnlocked)
        {
            isInteractable = true;
        }
        else
        {
            gameObject.AddComponent<Rigidbody>();
            gameObject.GetComponent<Collider>().isTrigger = true;
        }
    }


    public void CheckCombination()
    {
        foreach(Dial dial in dials)
        {
            if(dial.lockDial.CurrentNumber() != dial.passNum)
            {
                return;
            }
        }
        CorrectCombination();
    }

    private void CorrectCombination()
    {
        foreach (Dial dial in dials)
        {
            dial.lockDial.isInteractable = false;
        }

        StartCoroutine(LockOpenedProcess());
        Debug.Log("correct combination");
    }

    private IEnumerator LockOpenedProcess()
    {
        animator.SetTrigger("Open");
        audioSource.PlayOneShot(lockOpenClip);
        yield return new WaitForSeconds(1f);


        isUnlocked = true;
        StartCoroutine(ReturnToOriginalPos());
    }

    public void Interact()
    {
        StartCoroutine(MoveToFrontOfCamera());

        foreach (Dial dial in dials)
        {
            dial.lockDial.isInteractable = true;
        }
    }

    public void QuitInteract(InputAction.CallbackContext ctx)
    {
        if (!isInteractable && ctx.performed)
        {
            StartCoroutine(ReturnToOriginalPos());
        }
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }
}
