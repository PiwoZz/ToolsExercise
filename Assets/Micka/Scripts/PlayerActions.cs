using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerActions : MonoBehaviour {

    private readonly List<Item> _inventory = new();
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
        
        string output = JsonConvert.SerializeObject((_inventory, Chest.ChestInfos), Formatting.Indented);
        sw.Write(output);
    }

    private void Load() {
        using StreamReader sr = new StreamReader(Application.persistentDataPath + "/save.json");
        using JsonReader reader = new JsonTextReader(sr);
        reader.SupportMultipleContent = true;
        
        ItemDistributor itemDistributor = FindFirstObjectByType<ItemDistributor>();
        JsonSerializer serializer = new JsonSerializer();

        var readPair = serializer.Deserialize<(List<Item>, List<ChestInfos>)>(reader);
        
        _inventory.Clear();
        foreach (Item index in readPair.Item1) _inventory.Add(itemDistributor.GetItem(index.id));
        foreach (ChestInfos chest in readPair.Item2) {
            Chest temp = Chest.GetChest(chest.id);
            temp.MyChestInfos.opened = chest.opened;
            temp.MyChestInfos.containedItem = itemDistributor.GetItem(temp.MyChestInfos.containedItem.id);
        }
        Chest.Actualise();
    }

    public void AddToInventory(Item item) {
        _inventory.Add(item);
    }
}
