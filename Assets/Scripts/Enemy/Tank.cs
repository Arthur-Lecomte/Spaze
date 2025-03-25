using UnityEngine;

public class Tank : Enemy
{

    protected override void MoveTowardsPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > shootRange)
        {
            // Calculer la direction vers le joueur
            Vector3 direction = (player.position - transform.position).normalized;

            // Déplacer l'ennemi dans cette direction
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }
}