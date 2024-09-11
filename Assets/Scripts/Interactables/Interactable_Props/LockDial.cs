using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LockDial : MonoBehaviour, IInteractable
{
    public AudioClip dialSpinClip;

    private int currNumber = 0;
    private Lock_Interact_Manager manager;
    private AudioSource audioSource;

    public bool isInteractable = true;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Rotate()
    {
        isInteractable = false;

        audioSource.PlayOneShot(dialSpinClip);

        StartCoroutine(RotationProcess());
    }

    private IEnumerator RotationProcess()
    {
        // Set up the initial rotation target based on the current number
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(0f, 0f, -36f); // Rotate 36 degrees on the x-axis

        // Define how long the rotation should take
        float rotationDuration = 0.5f; // Adjust the duration as needed
        float elapsedTime = 0f;

        // Smoothly rotate the dial over time
        while (elapsedTime < rotationDuration)
        {
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the rotation ends exactly at the target rotation
        transform.rotation = endRotation;

        currNumber += 1;

        if(currNumber > 9)
        {
            currNumber = 0;
        }

        isInteractable = true;

        manager.CheckCombination();
    }
    public void Interact()
    {
        Rotate();
    }

    public bool IsInteractable()
    {
        return isInteractable;
    }
    public int CurrentNumber()
    {
        return currNumber;
    }
    public void SetLockManager(Lock_Interact_Manager manager)
    {
        this.manager = manager;
    }
}
