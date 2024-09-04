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
    private Base_Holdable_Items heldItem;
    
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
            StartCoroutine(WarningOnItem("Is Already Holding Item!"));
        }
        else
        {
            itemHolded = itemToHold;
            itemHolded.transform.position = holdPoint.position;
            itemHolded.transform.parent = holdPoint;
            itemHolded.DeactivateRigidBody();
        }
    }

    public void PlaceItem(Transform placePoint)
    {
        if (heldItem != null)
        {
            heldItem.transform.SetParent(null);
            heldItem.transform.position = placePoint.position;
            heldItem.ActivateRigidBody();
            heldItem = null;
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
        itemHolded.ActivateRigidBody();
        itemHolded = null;
    }

    public IEnumerator WarningOnItem(string warningMessage)
    {
        warningText.text = warningMessage;
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        warningText.gameObject.SetActive(false);
    }

    public void ItemUsed()
    {
        itemHolded.transform.parent = null;
        Destroy(itemHolded.gameObject);
        itemHolded = null;
    }

    public bool IsHoldingItem()
    {
        return heldItem != null;
    }

    public Base_Holdable_Items GetHeldItem()
    {
        return heldItem;
    }

    #region Get Variables
    public Base_Holdable_Items ItemHolded() { return  itemHolded; }
    #endregion
}
