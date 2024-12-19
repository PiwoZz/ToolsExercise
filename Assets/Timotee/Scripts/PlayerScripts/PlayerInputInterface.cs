using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerScripts
{
    public class PlayerInputInterface : MonoBehaviour
    {
        [Header("References")] 
        [SerializeField] private InputActionAsset _actionAsset;

        [SerializeField] private PlayerCameraController _cameraController;
        [SerializeField] private PlayerMovementController _movementController;

        private InputActionMap _actionMap;

        private void Start()
        {
            _actionMap = _actionAsset.FindActionMap("Player");
            
            var lookAction = _actionMap.FindAction("Look");
            var moveAction = _actionMap.FindAction("Move");

            lookAction.performed += _cameraController.UpdateCamera;
            moveAction.performed += _movementController.UpdateInput;
            moveAction.canceled += _movementController.ResetInput;
        }
    }
}