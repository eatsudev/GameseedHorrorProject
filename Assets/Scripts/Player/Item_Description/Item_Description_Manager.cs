using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Item_Description_Manager : MonoBehaviour
{
    public static Item_Description_Manager instance;


    public GameObject itemDescPanel;
    public Image itemImage;
    public Button closeButton;

    private Base_StoryItem currStoryItem;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log(instance);
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        closeButton.onClick.AddListener(CloseDisplay);

        itemDescPanel.SetActive(false);

        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Escape.performed += CloseDisplayEsc;
    }

    public void OpenDisplay(Base_StoryItem storyItem)
    {
        itemDescPanel.SetActive(true);
        itemImage.sprite = storyItem.itemImage;
        currStoryItem = storyItem;
    }

    public void CloseDisplay()
    {
        itemDescPanel.SetActive(false);

        currStoryItem.ReturnToOriginalPos();
        currStoryItem = null;
    }

    private void CloseDisplayEsc(InputAction.CallbackContext ctx)
    {
        if (itemDescPanel.activeInHierarchy && ctx.performed)
        {
            CloseDisplay();
        }
    }
}
