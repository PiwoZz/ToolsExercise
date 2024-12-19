using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour {

    private List<Item> _inventory = new();
    private GameObject _pointedObject;
    private Camera _mainCamera;

    [SerializeField] private float range;
    [SerializeField] private LayerMask interactionLayer;
    
    private void Start() {
        _mainCamera = Camera.main;
    }

    private void Update() {
        _pointedObject = GetPointedObject();
        if (_pointedObject && InputSystem.actions.FindAction("Interact").WasPressedThisFrame()) Interact();
        if (InputSystem.actions.FindAction("Save").WasPressedThisFrame()) Save();
        if (InputSystem.actions.FindAction("Load").WasPressedThisFrame()) Load();
    }

    private void Interact() {
        if(!_pointedObject.TryGetComponent(out IInteractible interactible)) return;
        interactible.Interact();
    }
    
    private GameObject GetPointedObject() {
        if (Physics.Raycast(_mainCamera.transform.position, _mainCamera.transform.forward, out var hitObject, range, interactionLayer)) {
            return hitObject.transform.gameObject;
        }

        return null;
    }

    private void Save() {
        using StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/save.json");
        using JsonWriter writer = new JsonTextWriter(sw);
        JsonSerializer serializer = new JsonSerializer();
        
        serializer.Serialize(writer, _inventory);
        serializer.Serialize(writer, transform);
    }

    private void Load() {
        using StreamReader sr = new StreamReader(Application.persistentDataPath + "/save.json");
        using JsonReader reader = new JsonTextReader(sr);
        JsonSerializer serializer = new JsonSerializer();
        
        _inventory = serializer.Deserialize<List<Item>>(reader);
        Transform temp = serializer.Deserialize<Transform>(reader);
        transform.position = temp.position;
        transform.rotation = temp.rotation;
    }

    public void AddToInventory(Item item) {
        _inventory.Add(item);
    }
}
