using UnityEngine;

public class Assaillant : Enemy
{
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

    public void SetSeed(int newSeed)
    {
        seed = newSeed;
    }

    protected override void MoveTowardsPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > shootRange)
        {
            // Calculer la direction vers le joueur
            Vector3 direction = (player.position - transform.position).normalized;

            // Déplacer l'ennemi dans cette direction
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
        else
        {
            // Faire tourner l'ennemi autour du joueur avec des variations basées sur la seed
            Vector3 orbitDirection = Vector3.Cross(Vector3.up, (player.position - transform.position).normalized) * rotationDirection;

            // Ajouter une variation aléatoire à la direction orbitale
            float randomOffsetX = (float)(random.NextDouble() - 0.5f); // Offset entre -0.5 et 0.5
            float randomOffsetZ = (float)(random.NextDouble() - 0.5f); // Offset entre -0.5 et 0.5
            orbitDirection += new Vector3(randomOffsetX, 0, randomOffsetZ).normalized * 0.5f;

            // Appliquer le mouvement avec rotationSpeed
            transform.position += orbitDirection * rotationSpeed * Time.deltaTime;
        }
    }

    protected override void ShootAtPlayer()
    {
        // Appeler la méthode parente pour conserver le comportement de base
        base.ShootAtPlayer();
    }
}