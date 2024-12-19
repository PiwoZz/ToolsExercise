using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private CharacterController _characterController;

    [Header("Movement Settings")] 
    [SerializeField] private float _movementSpeed;
    
    private Vector2 movementInput;

    private void Update()
    {
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        if (movementInput == Vector2.zero)
        {
            _characterController.Move(Vector3.zero);
            return;
        }

        Vector2 normalizedInput = movementInput.normalized;
        Vector3 movement = normalizedInput.x * _playerCamera.transform.right + normalizedInput.y * _playerCamera.transform.forward;
        movement.y = 0;
        movement.Normalize();
        movement *= _movementSpeed;

        _characterController.Move(movement * Time.deltaTime);
    }
    
    public void UpdateInput(InputAction.CallbackContext obj)
    {
        movementInput = obj.ReadValue<Vector2>();
    }

    public void ResetInput(InputAction.CallbackContext obj)
    {
        movementInput = Vector2.zero;
    }
}