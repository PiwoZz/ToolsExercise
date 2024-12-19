using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemDistributor : MonoBehaviour {
    private static readonly List<Item> Items = new();
    
    [SerializeField] private List<Item> serializedItems = new();

    private void Awake() {
        Items.AddRange(serializedItems);
    }

    public static Item GetRandomItem() {
        if (Items.Count == 0) return null;
        Item giveItem = Items[Random.Range(0, Items.Count)];
        Items.Remove(giveItem);
        return giveItem;
    }
}
