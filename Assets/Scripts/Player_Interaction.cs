using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Interaction : MonoBehaviour
{
    public Transform interactorTransform;
    public float interactRange;
    public LayerMask interactLayerMask;

    public TextMeshProUGUI interactText;

    private PlayerInput playerInput;
    private IInteractable interactableObject;
    private bool interact;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += OnInteract;
        playerInputActions.Player.Interact.canceled += OnInteract;

        interactText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        GetInteract();


        interact = false;
    }

    #region Interaction

    private void GetInteract()
    {
        Ray ray = new Ray(interactorTransform.position, interactorTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, interactRange, interactLayerMask))
        {
            if (hitInfo.collider.gameObject.TryGetComponent(out interactableObject))
            {
                if (interactableObject.IsInteractable())
                {
                    interactText.gameObject.SetActive(true);
                    if (interact)
                    {
                        interactableObject.Interact();
                    }
                    return;
                }
            }
        }
        interactText.gameObject.SetActive(false);
    }

    #endregion


    #region Get Inputs
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        Debug.Log(ctx.phase);

        interact = ctx.performed;
    }

    #endregion
}
