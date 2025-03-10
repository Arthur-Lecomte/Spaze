using UnityEngine;

public class Vaisseau : MonoBehaviour {
    public Inventory inventory;

    private void Awake() {
        inventory = GetComponent<Inventory>();
    }
}
