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

    private Base_Holdable_Items itemHeld;

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
        if (itemHeld != null)
        {
            WarningOnItem("Is Already Holding Item!");
        }
        else
        {
            itemHeld = itemToHold;
            itemHeld.transform.position = holdPoint.position;
            itemHeld.transform.parent = holdPoint;
            itemHeld.DeactivateRigidBody();
        }
    }

    private void OnDropItem(InputAction.CallbackContext ctx)
    {
        if (itemHeld != null)
        {
            if (ctx.performed)
            {
                DropItem();
            }
        }
    }

    public void DropItem()
    {
        itemHeld.transform.parent = null;
        itemHeld.ActivateRigidBody();
        itemHeld = null;
    }

    public void WarningOnItem(string warningMessage)
    {

        StartCoroutine(WarningOnItemProcess(warningMessage));

    }

    public IEnumerator WarningOnItemProcess(string warningMessage)
    {
        warningText.text = warningMessage;
        warningText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        warningText.gameObject.SetActive(false);
    }

    public void ItemUsed()
    {
        itemHeld.transform.parent = null;
        Destroy(itemHeld.gameObject);
        itemHeld = null;
    }

    

    #region Get Variables

    public Base_Holdable_Items GetHeldItem() { return  itemHeld; }

    public bool IsHoldingItem()
    {
        return itemHeld != null;
    }

    #endregion
}
