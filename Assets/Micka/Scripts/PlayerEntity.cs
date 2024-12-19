using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerEntity : MonoBehaviour {
    [SerializeField] private float moveSpeed = 5f;

    private Rigidbody _rb;
    private List<Item> _inventory = new();

    private void Awake() {
        _rb = GetComponent<Rigidbody>();
        
        
    }

    private void Update() {
        Vector2 movement = InputSystem.actions.FindAction("Move").ReadValue<Vector2>();
        _rb.linearVelocity = Vector3.forward * moveSpeed * movement;

        if (InputSystem.actions.FindAction("Save").WasPressedThisFrame()) {
            Save();
        }
    }

    public void AddItem(Item item) {
        _inventory.Add(item);
    }

    public void Save() {
        using StreamWriter sw = new StreamWriter(Application.persistentDataPath + "/save.json");
        using JsonWriter writer = new JsonTextWriter(sw);
        JsonSerializer serializer = new JsonSerializer();
        serializer.Serialize(writer, _inventory);
        Debug.Log("Saved at : " + Application.persistentDataPath + "/save.json");
    }

    public void Load() {
        using StreamReader sr = new StreamReader(Application.persistentDataPath + "/save.json");
        using JsonReader reader = new JsonTextReader(sr);
        JsonSerializer serializer = new JsonSerializer();
        _inventory = serializer.Deserialize<List<Item>>(reader);
        Debug.Log("Loaded from : " + Application.persistentDataPath + "/save.json");
    }
}
