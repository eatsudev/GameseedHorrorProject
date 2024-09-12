using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEditor.ShaderGraph.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityStandardAssets.Characters.FirstPerson;

public class Player_Interaction : MonoBehaviour
{
    public static Player_Interaction instance;

    public Transform interactorTransform;
    public float interactRange;
    public LayerMask interactLayerMask;

    public TextMeshProUGUI interactText;

    private PlayerInput playerInput;

    private bool interacting;
    private bool raycastInteracting;

    public bool regularInteractMode = true;
    public bool raycastInteractMode = false;

    private string regularInteractText = "E to Interact";
    private string raycastInteractText = "Click to Interact";
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.UI.Enable();

        playerInputActions.Player.Interact.performed += OnInteract;
        playerInputActions.Player.Interact.canceled += OnInteract;

        playerInputActions.UI.Click.started += OnRaycastInteract;
        playerInputActions.UI.Click.performed += OnRaycastInteract;

        interactText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (regularInteractMode)
        {
            GetInteract();
        }
        else if(raycastInteractMode)
        {
            GetRaycastInteract();
        }
        


        interacting = false;
        raycastInteracting = false;
    }

    #region Interaction

    public void ChangeInteractType(int type)
    {
        if(type == 0)
        {
            regularInteractMode = true;
            raycastInteractMode = false;

            interactText.text = regularInteractText;

            Entities_Manager.Instance.player.transform.parent.GetComponent<FirstPersonController>().enabled = true;
            Entities_Manager.Instance.player.transform.parent.GetComponent<CrouchController>().enabled = true;
        }
        else if(type == 1)
        {
            regularInteractMode = false;
            raycastInteractMode = true;

            interactText.text = raycastInteractText;

            Entities_Manager.Instance.player.transform.parent.GetComponent<FirstPersonController>().enabled = false;
            Entities_Manager.Instance.player.transform.parent.GetComponent<CrouchController>().enabled = false;
        }
        else
        {
            Debug.Log("No interact type of args");
        }
    }

    private void GetInteract()
    {
        Ray ray = new Ray(interactorTransform.position, interactorTransform.forward);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, interactRange, interactLayerMask))
        {
            if (!raycastHit.collider) return;

            IInteractable interactable = raycastHit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (interactable.IsInteractable())
                {
                    interactText.gameObject.SetActive(true);
                    if (interacting)
                    {
                        interactable.Interact();
                        interacting = false;
                    }
                    return;
                }
            }
        }

        interactText.gameObject.SetActive(false);
    }

    private void GetRaycastInteract()
    {
        RaycastHit raycastHit;

        if(Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out raycastHit, interactRange))
        {
            if (!raycastHit.collider) return;

            IInteractable interactable = raycastHit.collider.GetComponent<IInteractable>();

            if(interactable != null)
            {
                if(interactable.IsInteractable())
                {
                    interactText.gameObject.SetActive(true);
                    if (raycastInteracting)
                    {
                        Debug.Log("interact");
                        interactable.Interact();
                        raycastInteracting = false;
                    }
                    return;
                }
            }
        }
        interactText.gameObject.SetActive(false);
    }

    #endregion

    #region Interact Visual


    #endregion

    #region Get Inputs
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        interacting = ctx.performed;
    }
    public void OnRaycastInteract(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        raycastInteracting = true;
    }

    #endregion
}
