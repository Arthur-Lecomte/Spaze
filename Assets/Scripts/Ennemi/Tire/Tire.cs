using UnityEngine;

public class Tire : MonoBehaviour
{ //DEBUG!!! à supprimer et remplacer par Fight/Tir
    public GameObject creator; 
    [SerializeField] public float speed = 10f; // Vitesse de déplacement


    private void Start() {
        // Ignorer les collisions entre le projectile et le créateur
        if (creator != null) {
            Collider creatorCollider = creator.GetComponent<Collider>();
            Collider projectileCollider = GetComponent<Collider>();
            if (creatorCollider != null && projectileCollider != null) {
                Physics.IgnoreCollision(projectileCollider, creatorCollider);
            }
        }

        // Détruire le projectile après 3 secondes
        Destroy(gameObject, 3f);
    }

    private void Update() {
        // Déplacer le projectile vers l'avant
        transform.position += transform.forward * speed * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        // Vérifier si l'objet touché n'est pas le créateur
        if (other.gameObject == creator) {
            return; // Ignorer la collision avec le créateur
        }

        // Appliquer des dégâts si l'objet touché est un vaisseau
        Vaisseau vaisseau = other.GetComponent<Vaisseau>();
        if (vaisseau != null) {
            Enemy enemy = creator.GetComponent<Enemy>();
            vaisseau.TakeDamage(enemy.GetDamage()); // Appliquer les dégâts au vaisseau
            Destroy(gameObject);
        }

        
        
    }
}