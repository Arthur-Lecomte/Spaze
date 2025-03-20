using UnityEngine;

public class Assaillant : Ennemi
{
    protected override void Start()
    {
        // Appeler la méthode Start de la classe parente
        base.Start();

    }

    protected override void MoveTowardsPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > shootRange)
        {
            // Calculer la direction vers le joueur
            Vector3 direction = (player.position - transform.position).normalized;

            // Déplacer l'ennemi dans cette direction
            transform.position += direction * moveSpeed * Time.deltaTime;
        } else{

            // Faire tourner l'ennemi autour du joueur
            Vector3 orbitDirection = Vector3.Cross(Vector3.up, (player.position - transform.position).normalized);
            transform.position += orbitDirection * moveSpeed * Time.deltaTime;
        }
    }

    protected override void ShootAtPlayer()
    {
        // Appeler la méthode parente pour conserver le comportement de base
        base.ShootAtPlayer();

    }
}