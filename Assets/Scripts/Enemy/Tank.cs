using UnityEngine;

public class Tank : Enemy
{
    protected override void Start()
    {
        // Appeler la méthode Start de la classe parente
        base.Start();
    }

     protected override void ShootAtPlayer()
    {
        // Appeler la méthode parente pour conserver le comportement de base
        base.ShootAtPlayer();

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
    }
}