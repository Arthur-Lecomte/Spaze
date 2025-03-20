using UnityEngine;

public class Tire : MonoBehaviour
{
    public GameObject creator; // Qui a tiré ce projectile ?
    public float speed = 10f; // Vitesse de déplacement
    public int damage = 50; // Dégâts infligés

    void Update() {
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    void OnTriggerEnter(Collider other) {
        if (other.gameObject != creator) { // Évite de toucher celui qui a tiré
            Ennemi ennemi = other.GetComponent<Ennemi>();
            if (ennemi) {
                ennemi.TakeDamage(damage);
            }
            Destroy(gameObject); // Détruit le projectile
        }
    }
    //Détruit le projectile après 5 seccondes
    void Start()
    {
        Destroy(gameObject, 3f);
    }
    
}