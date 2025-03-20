using UnityEngine;

public class Ennemi : MonoBehaviour
{
    [Header("Statistiques de base")]
    [SerializeField] public int health = 200;
    [SerializeField] public float moveSpeed = 1f; // Vitesse de déplacement de l'ennemi
    [SerializeField] public float timeBeforeBeingCible = 2f;


    [Header("Comportement de tir")]
    [SerializeField] public GameObject projectilePrefab; // Préfabriqué du projectile
    [SerializeField] public float shootInterval = 1.5f; // Intervalle de tir par défaut
    private float lastShootTime;
    [SerializeField] public float shootRange = 8f; // Distance de tir par défaut

    protected Transform player; // Référence au joueur

    [Header("Point de tir")]
    [SerializeField] public Transform shootPoint; // L'endroit exact où les tirs spawnent

    protected virtual void Start()
    {
        // Trouver le joueur dans la scène
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");
            if (playerObject != null)
            {
                player = playerObject.transform;
            }
            else
            {
                Debug.LogError("Aucun joueur trouvé dans la scène ! Assurez-vous que le joueur a le tag 'Player'.");
            }
        }
    }

    protected virtual void Update()
    {
        if (player)
        {
            Vector3 enemyPosition = new Vector3(transform.position.x, 0, transform.position.z);
            Vector3 playerPosition = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            float distanceToPlayer = Vector3.Distance(enemyPosition, playerPosition);
            
            Debug.Log(playerPosition + " " + enemyPosition + " " + distanceToPlayer);

            // Déplacement vers le joueur
            MoveTowardsPlayer(distanceToPlayer);

            // Tirer sur le joueur si proche et si dans la portée de tir
            Debug.Log(distanceToPlayer+ " "+ shootRange);
            if (distanceToPlayer <= shootRange && Time.time > lastShootTime + shootInterval)
            {
                ShootAtPlayer();
                lastShootTime = Time.time;
            }
        }

        // Réduire le temps avant d'être une cible
        timeBeforeBeingCible -= Time.deltaTime;
    }

    protected virtual void MoveTowardsPlayer(float distanceToPlayer)
    {  }

    protected virtual void ShootAtPlayer()
    {
        if (projectilePrefab && shootPoint && player)
        {

            Vector3 playerPosition = player.transform.position;
            // Calculer la direction vers la position actuelle du joueur
            Vector3 direction = (playerPosition - shootPoint.position).normalized;
            // Instancier le projectile au niveau de shootPoint
            GameObject obj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));

            Rigidbody rb = obj.GetComponent<Rigidbody>();
            if (rb)
            {
                rb.linearVelocity = direction * 10f; // Ajuste la vitesse selon besoin
            }

            // Assigner l'ennemi comme créateur du projectile
            Tire tireComponent = obj.GetComponent<Tire>();
            if (tireComponent != null)
            {
                tireComponent.creator = gameObject;
            }
        }
    }

    public void TakeDamage(int damage)
    {
        health = Mathf.Max(0, health - damage);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public Transform getPlayer()
    {
        return player;
    }
}