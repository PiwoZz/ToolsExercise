using UnityEngine;

public class Chest : MonoBehaviour, IInteractible {
        public Item containedItem;

        private void Awake() {
                containedItem = ItemDistributor.GetRandomItem();
        }

        public void Interact() {
                if(containedItem) FindAnyObjectByType<PlayerActions>().AddToInventory(containedItem);
                Destroy(gameObject);
        }
}
