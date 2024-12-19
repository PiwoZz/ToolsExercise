using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _trackingTargetPivot = null;
    
    [Header("Camera Settings")]
    [SerializeField] private float _cameraSensitivity = 0.5f;
    [SerializeField] private Vector2 _cutoff = Vector2.zero;
    
    private InputActionMap _inputMap;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CutoffRotation(ref Vector2 rotation)
    {
        if (Math.Abs(rotation.x) < _cutoff.x)
            rotation.x = 0;
        if (Math.Abs(rotation.y) < _cutoff.y)
            rotation.y = 0;
    }
    
    public void UpdateCamera(InputAction.CallbackContext obj)
    {
        Vector2 cameraRotation = obj.ReadValue<Vector2>();
        CutoffRotation(ref cameraRotation);
        
        if (cameraRotation == Vector2.zero)
            return;
        
        cameraRotation *= _cameraSensitivity;
        _trackingTargetPivot.Rotate(cameraRotation.y, cameraRotation.x, 0, Space.World);
    }
}