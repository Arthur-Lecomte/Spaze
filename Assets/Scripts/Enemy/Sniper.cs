using UnityEngine;

public class Sniper : Enemy
{
    bool canAttack = true;

    protected override void MoveTowardsPlayer(float distanceToPlayer)
    {
        canAttack = true;
        isFleeing = false;

        if (distanceToPlayer > shootRange)
        {
            
            // Se déplacer vers le joueur
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;

            // Faire regarder le sniper vers le joueur
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        if (distanceToPlayer < 30f) // Range pour fuir
        {
            isFleeing = true; // Activer le mode fuite

            // Faire fuir l'ennemi en s'éloignant du joueur
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            transform.position += fleeDirection * moveSpeed * Time.deltaTime;

            // Faire regarder le sniper dans la direction de fuite
            Quaternion fleeRotation = Quaternion.LookRotation(fleeDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, fleeRotation, Time.deltaTime * 5f);

            // Empêcher l'ennemi d'attaquer
            canAttack = false;
        }
    }





    protected override void ShootAtPlayer()
    {
        if (canAttack)
        {
            base.ShootAtPlayer();
        }
        // Appeler la méthode parente pour conserver le comportement de base


    }
}