using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractible {
        public static List<ChestInfos> ChestInfos = new();
        public static List<Chest> Chests = new();

        [NonSerialized] public ChestInfos MyChestInfos;

        private static int _commonId = 0;

        private void Awake() {
                MyChestInfos = new ChestInfos();
                MyChestInfos.id = (int)Mathf.Pow(2, _commonId++);
                MyChestInfos.containedItem = ItemDistributor.GetRandomItem();
                MyChestInfos.opened = false;
                ChestInfos.Add(MyChestInfos);
                Chests.Add(this);
        }

        public static void Actualise() {
                foreach (var chest in Chests) {
                        Color color = chest.MyChestInfos.opened ? Color.gray : Color.yellow;
                        chest.GetComponent<MeshRenderer>().material.color = color;
                }
        }

        public void Interact() {
                if (MyChestInfos is not { containedItem: not null, opened: false }) return;
                FindAnyObjectByType<PlayerActions>().AddToInventory(MyChestInfos.containedItem);
                MyChestInfos.opened = true;
                GetComponent<MeshRenderer>().material.color = Color.gray;
        }

        public static Chest GetChest(int id) {
                return Chests.Find(x => x.MyChestInfos.id ==  id);
        }
        
}

[Serializable]
public class ChestInfos {
        public int id;
        public Item containedItem;
        public bool opened;
}
