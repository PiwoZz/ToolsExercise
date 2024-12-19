using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create Item")]
public class Item : ScriptableObject {
    public int id;
    public string itemName;
    public Sprite icon;
}
