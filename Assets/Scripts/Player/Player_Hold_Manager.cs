using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Hold_Manager : MonoBehaviour
{
    public static Player_Hold_Manager instance;

    public TextMeshProUGUI warningText;
    public Transform holdPoint;

    private Base_Holdable_Items itemHolded;

    private PlayerInput playerInput;

    private void Awake()
    {
        if (instance == null)
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
        warningText.gameObject.SetActive(false);

        playerInput = GetComponent<PlayerInput>();
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.DropItem.performed += OnDropItem;
        playerInputActions.Player.DropItem.canceled += OnDropItem;
    }

    void Update()
    {
        
    }

    public void PickUpItem(Base_Holdable_Items itemToHold)
    {
        if (itemHolded != null)
        {
            StartCoroutine(UnableToPickUpItemWarning());
        }
        else
        {
            itemHolded = itemToHold;
            itemHolded.transform.position = holdPoint.position;
            itemHolded.transform.parent = holdPoint;
            itemHolded.DeactivateGravity();
        }
    }

    private void OnDropItem(InputAction.CallbackContext ctx)
    {
        if (itemHolded != null)
        {
            if (ctx.performed)
            {
                DropItem();
            }
        }
    }

    private void DropItem()
    {
        itemHolded.transform.parent = null;
        itemHolded.ActivateGravity();
        itemHolded = null;
    }

    private IEnumerator UnableToPickUpItemWarning()
    {
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        warningText.gameObject.SetActive(false);
    }
}
