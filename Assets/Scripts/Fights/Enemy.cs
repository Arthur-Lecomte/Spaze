using UnityEngine;

public class Enemy : MonoBehaviour, ICanTakeDamage {
    public void TakeDamage(float damage) {
        //TakeDamage
    }

    public GameObject WhoAmI() {
        return gameObject;
    }
}