using UnityEngine;

public class Vaisseau : MonoBehaviour {
    public Inventory inventory;

    private void Awake() {
        
        if (inventory == null) {
            inventory = GetComponent<Inventory>();
            if (inventory == null) {
                Debug.LogError("Inventory n'est pas assigné et n'a pas pu être trouvé sur le GameObject.");
            }
        }
    }
}
