using UnityEngine;

public class Ennemi : MonoBehaviour {
    public Transform player; // Référence au joueur
    public float moveSpeed = 5f; // Vitesse de déplacement de l'ennemi
    public GameObject projectilePrefab; // Préfabriqué du projectile
    public float shootInterval = 3f; // Intervalle de tir en secondes
    private float lastShootTime;

    void Update() {
        if(player) {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if(distanceToPlayer > 15f) {
                // Calculer la direction vers le joueur
                Vector3 direction = (player.position - transform.position).normalized;

                // Déplacer l'ennemi vers le joueur
                transform.position += direction * moveSpeed * Time.deltaTime;

                // Faire tourner l'ennemi pour faire face à la direction du mouvement
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
            } else {
                // Faire tourner l'ennemi pour faire face au joueur
                Vector3 direction = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);
            }

            if(distanceToPlayer <= 25f && Time.time > lastShootTime + shootInterval) {
                ShootAtPlayer();
                lastShootTime = Time.time;
            }
        }
    }

    void ShootAtPlayer() {
        if(projectilePrefab) {
            GameObject obj = Instantiate(projectilePrefab, transform.position + transform.forward, transform.rotation);
            obj.GetComponent<Tire>().creator = gameObject;
        }
    }
    
    public void TakeDamage() {
        Destroy(gameObject);
    }
}
