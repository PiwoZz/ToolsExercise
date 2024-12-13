using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEntity : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody _rb;

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        Vector2 movement = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        _rb.linearVelocity = Vector3.forward * moveSpeed * movement;
    }
}
