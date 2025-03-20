using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour, ICanTakeDamage {
    public delegate void EnemyDestroyedHandler(GameObject enemy);
    public event EnemyDestroyedHandler OnDestroyed;
    
    [Header("Statistiques de base")] public int Level = 1;
    [SerializeField] public int health = 200;
    [SerializeField] public float moveSpeed = 1f; // Vitesse de déplacement de l'ennemi
    [SerializeField] public float timeBeforeBeingCible = 2f;
    [SerializeField] public int damageEnemy = 50;


    [Header("Comportement de tir")]
    [SerializeField]
    public GameObject projectilePrefab; // Préfabriqué du projectile
    [SerializeField] public float shootInterval = 1.5f; // Intervalle de tir par défaut
    public float lastShootTime;
    [SerializeField] public float shootRange = 8f; // Distance de tir par défaut

    protected Transform player; // Référence au joueur

    [Header("Point de tir")]
    [SerializeField]
    public Transform shootPoint; // L'endroit exact où les tirs spawnent

    protected virtual void Start() {
        // Trouver le joueur dans la scène
        if (player == null) {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null) {
                player = playerObject.transform;
            } else {
                Debug.LogError("Aucun joueur trouvé dans la scène ! Assurez-vous que le joueur a le tag 'Player'.");
            }
        }

        ScaleStats();
    }

    protected virtual void Update() {
        if (player) {
            // Calculer la distance entre l'ennemi et le joueur
            float distance = Vector3.Distance(transform.position, player.position);

            // Calculer la direction vers le joueur
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

            // Effectuer la rotation vers le joueur
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

            // Vérifier si la rotation est terminée
            float angleDifference = Quaternion.Angle(transform.rotation, lookRotation);
            bool isRotationComplete = angleDifference < 25f;

            // Déplacement vers le joueur
            MoveTowardsPlayer(distance);

            // Si le joueur est à portée et que la rotation est terminée, attaquer
            if (distance <= shootRange && isRotationComplete && Time.time > lastShootTime + shootInterval) {
                ShootAtPlayer();
                lastShootTime = Time.time;
            }
        }

        // Réduire le temps avant d'être une cible
        timeBeforeBeingCible -= Time.deltaTime;
    }


    protected virtual void MoveTowardsPlayer(float distanceToPlayer) {
    }

    protected virtual void ShootAtPlayer() {
        if (projectilePrefab && shootPoint && player) {
            Vector3 playerPosition = player.transform.position;
            // Calculer la direction vers la position actuelle du joueur
            Vector3 direction = (playerPosition - transform.position).normalized;
            // Instancier le projectile au niveau de shootPoint
            GameObject obj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb) {
                rb.linearVelocity = direction * 10f; // Ajuste la vitesse selon besoin
            }
        }
    }

    protected virtual void ScaleStats() {
        // Multiplier les statistiques par un facteur basé sur le niveau
        health = Mathf.RoundToInt(health * (1 + (Level - 1) * 0.2f)); // Augmente de 20% par niveau
        damageEnemy = Mathf.RoundToInt(damageEnemy * (1 + (Level - 1) * 0.1f)); // Augmente de 10% par niveau
        shootInterval = Mathf.Max(0.1f, shootInterval * (1 - (Level - 1) * 0.05f)); // Diminue de 5% par niveau () Mini = 0.1f
        moveSpeed *= (1 + (Level - 1) * 0.05f); // Augmente de 5% par niveau
    }
    public Transform setPlayer(Transform player) {
        this.player = player;
        return player;
    }

    public int GetDamage() {
        return damageEnemy;
    }

    public void SetLevel(int newLevel) {
        Level = newLevel;
        ScaleStats();
    }
    
    private void OnDestroy() {
        OnDestroyed?.Invoke(gameObject);
    }
    
    public void TakeDamage(int damage) {
        health = Mathf.Max(0, health - damage);

        if (health <= 0) {
            Destroy(gameObject);
        }
    }

    public GameObject WhoAmI() {
        return gameObject;
    }
}
