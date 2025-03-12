using UnityEngine;

public class Ennemi : MonoBehaviour
{
    [Header("Statistiques de base")]
    public int damage = 200;
    public int health = 200;
    public float moveSpeed = 1f; // Vitesse de déplacement de l'ennemi
    public float timeBeforeBeingCible = 2f;

    [Header("Comportement de tir")]
    public GameObject projectilePrefab; // Préfabriqué du projectile
    public float shootInterval = 1f; // Intervalle de tir en secondes
    private float lastShootTime;
    public float shootRange = 10f; // Distance de tir spécifique à chaque ennemi

    [Header("Références")]
    public Transform player; // Référence au joueur
    
    [Header("Point de tir")]
    public Transform shootPoint; // L'endroit exact où les tirs spawnent

void Update() {
        if(player) {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Déplacement vers le joueur
            MoveTowardsPlayer(distanceToPlayer);

            // Tirer sur le joueur si proche et si dans la portée de tir
            if(distanceToPlayer <= shootRange && Time.time > lastShootTime + shootInterval) {
                ShootAtPlayer();
                lastShootTime = Time.time;
            }
        }

        // Réduire le temps avant d'être une cible
        timeBeforeBeingCible -= Time.deltaTime;
    }

    // Déplacement vers le joueur
    void MoveTowardsPlayer(float distanceToPlayer) {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * moveSpeed);

        // Si l'ennemi est loin du joueur, il se déplace vers lui
        if(distanceToPlayer > 15f) {
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    // Tirer un projectile
    void ShootAtPlayer() {
        if (projectilePrefab && shootPoint && player) {
            // Calculer la direction du joueur
            Vector3 direction = (player.position - shootPoint.position).normalized;

            // Instancier le projectile au niveau de shootPoint
            GameObject obj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));

            // Si le projectile a un Rigidbody, applique une force
            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb) {
                rb.linearVelocity = direction * 10f; // Ajuste la vitesse selon besoin
            }

            obj.GetComponent<Tire>().creator = gameObject;
        }
    }
    // Prendre des dégâts
    public void TakeDamage(int damage) {
        health = Mathf.Max(0, health - damage);

        if(health <= 0) {
            Destroy(gameObject);
        }
    }
}