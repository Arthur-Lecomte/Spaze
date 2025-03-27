using UnityEngine;

public class TriggerAreaShop : MonoBehaviour {
    private Shop shop;

    /// <summary>
    /// Méthode appelée au démarrage. Initialise les composants nécessaires.
    /// </summary>
    private void Start() {
        shop = transform.parent.GetComponentInChildren<Shop>();
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre collider entre dans la zone de déclenchement.
    /// </summary>
    /// <param name="other">Le collider entrant en contact.</param>
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            shop.SetIsInShopRange(true);
        }
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre collider sort de la zone de déclenchement.
    /// </summary>
    /// <param name="other">Le collider sortant de la zone.</param>
    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            shop.SetIsInShopRange(false);
        }
    }
}
