using UnityEngine;

public class Tir : MonoBehaviour{
    private GameObject creator;
    private int damage;
    
    public void SetCreator(GameObject objectCreator) {
        creator = objectCreator;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<GameObject>() == creator) return;
        
        ICanTakeDamage hit = other.GetComponent<ICanTakeDamage>();
        if (hit != null) {
            hit.TakeDamage(damage);
            Destroy(gameObject);
        }
        
    }
}
