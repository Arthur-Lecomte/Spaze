using SmallHedge.SoundManager;
using UnityEngine;

public class Enemy : MonoBehaviour, ICanTakeDamage
{
    public delegate void EnemyDestroyedHandler(GameObject enemy);
    public event EnemyDestroyedHandler OnDestroyed;

    [Header("Statistiques de base")] public int Level = 1;
    [SerializeField] public float health = 200;
    [SerializeField] public float moveSpeed = 1f; // Vitesse de déplacement de l'ennemi
    [SerializeField] public float damageEnemy = 15;

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

    protected bool isFleeing = false; // Indique si l'ennemi est en mode fuite
    [SerializeField] private GameObject portal; // Référence au portail

    private Renderer portalRenderer;

    private int lastColorLevel = 0;

    /// <summary>
    /// Méthode appelée au démarrage. Initialise les paramètres de l'ennemi.
    /// </summary>
    protected virtual void Start()
    {
        // Trouver le joueur dans la scène
        if (portal)
        {
            portalRenderer = portal.GetComponent<Renderer>();
        }
        player = Vaisseau.Instance.transform;
        if (player == null)
        {
            Debug.LogError("Aucun joueur trouvé dans la scène ! Assurez-vous que le joueur a le tag 'Player'.");
        }

        ScaleStats();
    }

    /// <summary>
    /// Méthode appelée à chaque frame pour mettre à jour l'état de l'ennemi.
    /// </summary>
    protected virtual void Update()
    {
        if (player)
        {
            // Calculer la distance entre l'ennemi et le joueur
            float distance = Vector3.Distance(transform.position, player.position);

            if (!isFleeing)
            {
                // Calculer la direction vers le joueur
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));

                // Effectuer la rotation vers le joueur
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

                // Vérifier si la rotation est terminée
                float angleDifference = Quaternion.Angle(transform.rotation, lookRotation);
                bool isRotationComplete = angleDifference < 25f;

                // Si le joueur est à portée et que la rotation est terminée, attaquer
                if (distance <= shootRange && isRotationComplete && Time.time > lastShootTime + shootInterval)
                {
                    ShootAtPlayer();
                    lastShootTime = Time.time;
                }
            }

            // Déplacement vers le joueur
            MoveTowardsPlayer(distance);
        }
    }

    /// <summary>
    /// Déplace l'ennemi vers le joueur.
    /// </summary>
    /// <param name="distanceToPlayer">La distance entre l'ennemi et le joueur.</param>
    protected virtual void MoveTowardsPlayer(float distanceToPlayer)
    {
    }

    /// <summary>
    /// Tire sur le joueur.
    /// </summary>
    protected virtual void ShootAtPlayer()
    {
        if (projectilePrefab && shootPoint && player)
        {
            // Calculer la direction vers la position actuelle du joueur
            Vector3 direction = (player.transform.position - transform.position).normalized;
            // Instancier le projectile au niveau de shootPoint
            GameObject bullet = Instantiate(projectilePrefab, shootPoint.position, Quaternion.LookRotation(direction));
            bullet.GetComponent<Tir>().SetInformations(false, damageEnemy, shootRange);
        }
    }

    /// <summary>
    /// Met à l'échelle les statistiques de l'ennemi en fonction de son niveau.
    /// </summary>
    protected virtual void ScaleStats()
    {
        // Multiplier les statistiques par un facteur basé sur le niveau
        health = Mathf.RoundToInt(health * (1 + (Level - 1) * 0.002f));
        damageEnemy = damageEnemy * (1 + (Level - 1) * 0.0015f);
        shootInterval = Mathf.Max(0.1f, shootInterval * (1 - (Level - 1) * 0.002f));
        moveSpeed = moveSpeed* (1 + (Level - 1) * 0.001f);

    }

    

    /// <summary>
    /// Définit le niveau de l'ennemi et met à jour ses statistiques.
    /// </summary>
    /// <param name="newLevel">Le nouveau niveau de l'ennemi.</param>
    public void SetLevel(int newLevel)
    {
        Level = newLevel;
        UpdatePortalEffect();
        ScaleStats();
    }

    /// <summary>
    /// Méthode appelée lors de la destruction de l'ennemi.
    /// </summary>
    private void OnDestroy()
    {
        OnDestroyed?.Invoke(gameObject);
    }

    /// <summary>
    /// Applique des dégâts à l'ennemi.
    /// </summary>
    /// <param name="damage">La quantité de dégâts à appliquer.</param>
    public void TakeDamage(float damage)
    {
        health = Mathf.Max(0, health - damage);

        if (health <= 0)
        {
            Destroy(gameObject);
            Turret.onEnemyKilled?.Invoke(gameObject);
            SoundManager.PlaySound(SoundType.EXPLOSIONVAISSEAU);
        }
    }

    /// <summary>
    /// Indique si l'objet est contrôlé par le joueur.
    /// </summary>
    /// <returns>False car il s'agit d'un ennemi.</returns>
    public bool AmIPlayer()
    {
        return false;
    }

    /// <summary>
    /// Méthode appelée lors de la validation des propriétés dans l'inspecteur (pour le débogage).
    /// </summary>
    protected virtual void OnValidate()
    {
        ScaleStats();
        UpdatePortalEffect();
    }

    /// <summary>
    /// Met à jour l'effet du portail en fonction du niveau de l'ennemi.
    /// </summary>
    public void UpdatePortalEffect()
    {
        if (portalRenderer)
        {
            // Utiliser renderer.material pour modifier uniquement l'instance
            Material material = portalRenderer.material;
            if (material != null && material.shader != null)
            {
                // Vérifier si un nouveau palier est franchi
                if (Level > 20 && lastColorLevel < 20)
                {
                    material.SetColor("_Color", Color.red); // Appliquer la couleur rouge
                    lastColorLevel = 20; // Mettre à jour le dernier palier franchi
                }
                else if (Level > 10 && lastColorLevel < 10)
                {
                    material.SetColor("_Color", Color.yellow); // Appliquer la couleur jaune
                    lastColorLevel = 10; // Mettre à jour le dernier palier franchi
                }

                if (Level <= 30)
                {
                    Color hdrColor = material.GetColor("_Color");
                    hdrColor *= 1.3f; // Augmenter légèrement l'intensité
                    material.SetColor("_Color", hdrColor);
                    float baseSpeed = material.GetFloat("_Speed");
                    material.SetFloat("_Speed", baseSpeed * 1.1f);
                }
            }
        }
    }
}
