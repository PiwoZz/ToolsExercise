using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[JsonObject(MemberSerialization.OptIn)]
public class Chest : MonoBehaviour, IInteractible {
        public static List<ChestInfos> Chests = new();

        [JsonProperty] private ChestInfos _myChestInfos;

        private static int _commonId = 0;

        private void Awake() {
                _myChestInfos.id = (int)Mathf.Pow(2, _commonId++);
                _myChestInfos.containedItem = ItemDistributor.GetRandomItem();
                _myChestInfos.opened = false;
                Chests.Add(_myChestInfos);
        }

        public void Interact() {
                if(_myChestInfos is { containedItem: not null, opened: false }) FindAnyObjectByType<PlayerActions>().AddToInventory(_myChestInfos.containedItem);
                _myChestInfos.opened = true;
        }

        private void OnDestroy() {
                Chests.Remove(_myChestInfos);
        }

        public static ChestInfos GetChest(int id) {
                return Chests.Find(x => x.id == id);
        }
        
}

[Serializable]
public struct ChestInfos : IEquatable<ChestInfos> {
        public int id;
        public Item containedItem;
        public bool opened;

        public bool Equals(ChestInfos other) {
                return id == other.id && Equals(containedItem, other.containedItem) && opened == other.opened;
        }

        public override bool Equals(object obj) {
                return obj is ChestInfos other && Equals(other);
        }

        public override int GetHashCode() {
                return HashCode.Combine(id, containedItem, opened);
        }
}
