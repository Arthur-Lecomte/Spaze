using UnityEngine;

public class Sniper : Ennemi
{
    bool canAttack = true;
    protected override void Start()
    {
        // Appeler la méthode Start de la classe parente
        base.Start();


    }


    protected override void MoveTowardsPlayer(float distanceToPlayer)
    {

        canAttack = true;

        if (distanceToPlayer > shootRange)
        {

            Vector3 direction = (player.position - transform.position).normalized;
            // Déplacer l'ennemi dans cette direction
            transform.position += direction * moveSpeed * Time.deltaTime;


        }

        if (distanceToPlayer < 30f) //Range pour fuir
        {

            // Faire fuir l'ennemi en s'éloignant du joueur
            Vector3 fleeDirection = (transform.position - player.position).normalized;
            transform.position += fleeDirection * moveSpeed * Time.deltaTime;

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