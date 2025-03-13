using UnityEngine;

public class Vaisseau : MonoBehaviour {

    public static Vaisseau Instance;
    public Inventory inventory;

    void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
}
