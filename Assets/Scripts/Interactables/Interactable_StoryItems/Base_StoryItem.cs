using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Base_StoryItem : MonoBehaviour, IInteractable
{
    public Sprite itemImage;
    public AudioClip readingAudioClip;

    public AudioClip clip;

    public Vector3 eulerAngles_Offset = Vector3.zero;
    public float distanceFromCamera = 2f; // Set the distance from the player's camera
    public float moveDuration = 0.5f;     // Time for the movement to complete

    public bool isInteractable = true;

    private Transform cameraTransform;
    private Vector3 originalPosition;
    private Quaternion originalRotation;

    private AudioSource audioSource;
    private Collider collider;
    //private Rigidbody rigidbody;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        originalPosition = transform.position;
        originalRotation = transform.rotation;

        collider = GetComponent<Collider>();
        //rigidbody = GetComponent<Rigidbody>();

        audioSource = GetComponent<AudioSource>();

        if(audioSource == null )
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Escape.performed += QuitInteract;

    }

    public void MoveToFrontOfCamera()
    {
        StartCoroutine(MoveToFrontOfCameraProcess());
    }

    private IEnumerator MoveToFrontOfCameraProcess()
    {
        Player_Interaction.instance.ChangeInteractType(1);
        isInteractable = false;
        DeactivateCollider();

        Vector3 targetPosition = cameraTransform.position + cameraTransform.forward * distanceFromCamera;
        Vector3 targetRotation = cameraTransform.eulerAngles + eulerAngles_Offset;

        Vector3 startPosition = transform.position;
        Vector3 startRotation = transform.eulerAngles;

        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // Smoothly interpolate the position and rotation using Euler angles
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            transform.eulerAngles = Vector3.Lerp(startRotation, targetRotation, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are exactly at the target
        transform.position = targetPosition;
        transform.eulerAngles = targetRotation;

        Item_Description_Manager.instance.OpenDisplay(this);
    }

    public void ReturnToOriginalPos()
    {
        StartCoroutine(ReturnToOriginalPosProcess());
    }

    // Coroutine to return the lock manager to its original position
    private IEnumerator ReturnToOriginalPosProcess()
    {
        yield return null;
        Player_Interaction.instance.ChangeInteractType(0);

        Vector3 startPosition = transform.position;  // Current position in front of the camera
        Vector3 startRotation = transform.eulerAngles;  // Current rotation as Euler angles
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            // Smoothly interpolate the position and rotation back to the original using Euler angles
            transform.position = Vector3.Lerp(startPosition, originalPosition, elapsedTime / moveDuration);
            transform.eulerAngles = Vector3.Lerp(startRotation, originalRotation.eulerAngles, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final position and rotation are exactly at the original
        transform.position = originalPosition;
        transform.eulerAngles = originalRotation.eulerAngles;

        isInteractable = true;
        ActivateCollider();
    }

    public virtual void ActivateCollider()
    {
        collider.enabled = true;
    }

    public virtual void DeactivateCollider()
    {
        collider.enabled = false;
    }

    public void Interact()
    {
        MoveToFrontOfCamera();
    }

    public void QuitInteract(InputAction.CallbackContext ctx)
    {
        if (!isInteractable && ctx.performed)
        {
            ReturnToOriginalPos();
        }
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }

}
