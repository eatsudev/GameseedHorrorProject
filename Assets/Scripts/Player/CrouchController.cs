using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using UnityStandardAssets.Characters.FirstPerson;

public class CrouchController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private CapsuleCollider characterCollider;
    [SerializeField] private CinemachineVirtualCamera cinemachineCamera;
    [SerializeField] private Transform cameraTransform;

    [Header("Crouch Variables")]
    [SerializeField] private float crouchHeight = 1.0f;
    [SerializeField] private float standHeight = 2.0f;
    [SerializeField] private float crouchCenter = 0.5f;
    [SerializeField] private float standCenter = 1.0f;
    [SerializeField] private float crouchSpeed = 5.0f;

    private CharacterController m_CharacterController;
    private PlayerInput playerInput;

    public bool isCrouching = false;
    private Vector3 originalCameraLocalPosition;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        PlayerInputActions playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();

        playerInputActions.Player.Crouch.performed += OnCrouch;
        playerInputActions.Player.Crouch.canceled += OnCrouch;

        if (characterController == null) characterController = GetComponent<CharacterController>();
        if (cameraTransform == null) cameraTransform = Camera.main.transform;

        originalCameraLocalPosition = cameraTransform.localPosition;

    }
    private void Update()
    {
        Crouch();
    }

    private void Crouch()
    {
        float targetHeight = isCrouching ? crouchHeight : standHeight;
        float targetCenter = isCrouching ? crouchCenter : standCenter;
        Vector3 targetCameraPosition = isCrouching ? new Vector3(originalCameraLocalPosition.x, originalCameraLocalPosition.y - 1f, originalCameraLocalPosition.z) : originalCameraLocalPosition;

        characterController.height = Mathf.Lerp(characterController.height, targetHeight, Time.deltaTime * crouchSpeed);
        /*float height = Mathf.Lerp(transform.localScale.y, targetHeight, Time.deltaTime * crouchSpeed);
        transform.localScale = new Vector3(1, height, 1);*/
        characterController.center = Vector3.Lerp(characterController.center, new Vector3(0, targetCenter, 0), Time.deltaTime * crouchSpeed);
        characterCollider.height = characterController.height;
        characterCollider.center = characterController.center;
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, targetCameraPosition, Time.deltaTime * crouchSpeed);

        if (cinemachineCamera)
        {
            var transposer = cinemachineCamera.GetCinemachineComponent<CinemachineTransposer>();
            if (transposer != null)
            {
                transposer.m_FollowOffset.y = Mathf.Lerp(transposer.m_FollowOffset.y, targetCameraPosition.y, Time.deltaTime * crouchSpeed);
            }
        }

        

        Debug.Log(isCrouching);
    }


    private void OnCrouch(InputAction.CallbackContext ctx)
    {
        isCrouching = ctx.performed;
    }
}
   
