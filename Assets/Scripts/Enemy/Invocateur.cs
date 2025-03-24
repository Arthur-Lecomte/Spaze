using UnityEngine;

public class Invocateur : Enemy
{
    [SerializeField] public Assaillant minionPrefab; // Préfabriqué du sbire (de type Assaillant)

    
    private int seed; // Seed aléatoire pour les déplacements
    private System.Random random; // Générateur pseudo-aléatoire basé sur la seed
    private int rotationDirection; // Sens de rotation basé sur la seed
    private float rotationSpeed; // Vitesse de rotation basée sur la seed

    protected override void Start()
    {
        // Appeler la méthode Start de la classe parente
        base.Start();

        if (seed == 0)
        {
            seed = Random.Range(1, int.MaxValue); // Générer une seed aléatoire
        }
        // Initialiser le générateur pseudo-aléatoire avec la seed
        random = new System.Random(seed);

        // Générer un sens de rotation aléatoire (-1 pour antihoraire, 1 pour horaire)
        rotationDirection = random.Next(0, 2) == 0 ? -1 : 1;

        // Ajouter une variation aléatoire à shootRange (entre shootRange - 5 et shootRange + 5)
        shootRange += (float)(random.NextDouble() * 10 - 5); // Variation de ±5

        // Générer une vitesse de rotation aléatoire (entre -5 et +5)
        rotationSpeed = (float)(random.NextDouble() * 10 - 5); // Variation de ±5

        Debug.Log($"Assaillant avec seed : {seed}, ShootRange : {shootRange}, RotationDirection : {rotationDirection}, RotationSpeed : {rotationSpeed}");
    }

    protected override void Update()
    {
        if (player)
        {
            // Calculer la distance entre l'invocateur et le joueur
            float distance = Vector3.Distance(transform.position, player.position);

            if (!isFleeing)
            {
                // Calculer la direction vers le joueur
                Vector3 directionToPlayer = (player.position - transform.position).normalized;

 
                // Effectuer la rotation vers le joueur

                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }


            // Déplacement vers le joueur
            MoveTowardsPlayer(distance);


            // Si le joueur est à portée, invoquer un sbire
            if (distance <= shootRange && Time.time > lastShootTime + shootInterval)
            {

                SpawnMinion();
                lastShootTime = Time.time;
            }
        }

        // Réduire le temps avant d'être une cible
        timeBeforeBeingCible -= Time.deltaTime;
    }



    protected override void MoveTowardsPlayer(float distanceToPlayer)
    {
        isFleeing = false;

        if (distanceToPlayer > shootRange)
        {

            // Se déplacer vers le joueur
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * rotationSpeed * Time.deltaTime;
            /*
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            */

        }

        if (distanceToPlayer < shootRange / 1.5) // Range pour fuir
        {
            isFleeing = true; // Activer le mode fuite

            // Faire fuir l'ennemi en s'éloignant du joueur
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            transform.position += fleeDirection * moveSpeed * Time.deltaTime;

            // Faire regarder dans la direction de fuite
            Quaternion fleeRotation = Quaternion.LookRotation(fleeDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, fleeRotation, Time.deltaTime * 5f);


        }
        else
        {
            // Faire tourner l'ennemi autour du joueur avec des variations basées sur la seed
            Vector3 orbitDirection = Vector3.Cross(Vector3.up, (player.position - transform.position).normalized) * rotationDirection;

            // Ajouter une variation aléatoire à la direction orbitale
            float randomOffsetX = (float)(random.NextDouble() - 0.5f); // Offset entre -0.5 et 0.5
            float randomOffsetZ = (float)(random.NextDouble() - 0.5f); // Offset entre -0.5 et 0.5
            orbitDirection += new Vector3(randomOffsetX, 0, randomOffsetZ).normalized * 0.5f;

            transform.position += orbitDirection * rotationSpeed * Time.deltaTime;
        }


    }

    public void SpawnMinion()
    {
        Assaillant minion = Instantiate(minionPrefab, transform.position, Quaternion.identity);
        // Lui donner une référence au joueur
        WaveManager.Instance.RegisterEnemy(minion.gameObject);
    }
}