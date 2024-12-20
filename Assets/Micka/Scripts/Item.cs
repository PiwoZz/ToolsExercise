using System;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
[JsonObject(MemberSerialization.OptIn)]
public class Item {
    [JsonProperty] public int id;
    public string itemName;
    public Sprite icon;
}
