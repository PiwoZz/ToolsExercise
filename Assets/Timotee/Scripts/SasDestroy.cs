using System;
using UnityEngine;

public class SasDestroy : SasInteractable
{
    [Header("Settings")] 
    [SerializeField] private bool _expectedValue = false;
    
    private bool _destroyed = false;
    
    public override void Interact(bool value)
    {
        if (value == _expectedValue)
            _destroyed = true;
    }

    public void LateUpdate()
    {
        if (_destroyed)
            Destroy(gameObject);
    }
}