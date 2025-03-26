using UnityEngine;

public class TriggerAreaShop : MonoBehaviour {
    private Shop shop;

    private void Start() {
        shop = transform.parent.GetComponentInChildren<Shop>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            shop.SetIsInShopRange(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            shop.SetIsInShopRange(false);
        }
    }
}
