using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class LockDial : MonoBehaviour, IInteractable
{
    private int currNumber = 0;
    private Lock_Interact_Manager manager;

    public bool isInteractable = true;

    private void Rotate()
    {
        StartCoroutine(RotationProcess());
    }

    private IEnumerator RotationProcess()
    {
        // Set up the initial rotation target based on the current number
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = startRotation * Quaternion.Euler(36f, 0f, 0f); // Rotate 36 degrees on the x-axis

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

        manager.CheckCombination();

        isInteractable = true;
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
