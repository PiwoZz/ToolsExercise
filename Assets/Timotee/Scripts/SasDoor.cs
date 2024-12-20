using System;
using UnityEngine;

public class SasDoor : SasInteractable
{
    [SerializeField] private float _closedHeight;
    [SerializeField] private float _openHeight;

    [SerializeField] private float _speed;

    private float _currentValue = 0;
    private bool _closed = false;

    private void Update()
    {
        switch (_closed)
        {
            case true when _currentValue == 0:
            case false when _currentValue == 1:
                return;
        }
        
        float value = _speed * Time.deltaTime;
        float newValue = _currentValue + value * (_closed ? -1 : 1);

        newValue = Mathf.Clamp(newValue, 0, 1);
        _currentValue = newValue;
        
        float height = Mathf.Lerp(_closedHeight, _openHeight, _currentValue);
        transform.position = new Vector3(transform.position.x, height, transform.position.z);
    }

    public override void Interact(bool value)
    {
        _closed = value;
    }
}