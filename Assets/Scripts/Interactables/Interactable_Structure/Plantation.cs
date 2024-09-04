using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Plantation : Base_Interactable_Structure
{
    public static Plantation instance;
    public Transform[] plantPoints;  
    public GameObject flowerPrefab; 
    public float growTime = 5f;
    private PlayerInput playerInput;
    private Base_Holdable_Items itemPlaced;

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
        playerInput = GetComponent<PlayerInput>();
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.PlaceSeed.performed += OnPlaceSeed;
    }

    private void OnPlaceSeed(InputAction.CallbackContext ctx)
    {
        if (itemPlaced != null)
        {
            if (ctx.performed)
            {
                Interact();
            }
        }
    }
    public override void Interact()
    {
        base.Interact();

        if (Player_Hold_Manager.instance.IsHoldingItem())
        {
            Base_Holdable_Items heldItem = Player_Hold_Manager.instance.GetHeldItem();
            if (heldItem is Seed)
            {
                foreach (Transform plantPoint in plantPoints)
                {
                    if (CheckIfEmpty(plantPoint))
                    {
                        Player_Hold_Manager.instance.PlaceItem(plantPoint);
                        StartCoroutine(GrowSeed(plantPoint));
                        break;
                    }
                }
            }
        }
    }

    private IEnumerator GrowSeed(Transform plantPoint)
    {
        yield return new WaitForSeconds(growTime);

        foreach (Transform child in plantPoint)
        {
            Destroy(child.gameObject);
        }
        GameObject flower = Instantiate(flowerPrefab, plantPoint.position, Quaternion.identity);
        flower.GetComponent<Base_Interactable_Structure>().isInteractable = true;
    }

    private bool CheckIfEmpty(Transform plantPoint)
    {
        return plantPoint.childCount == 0; 
    }
}
