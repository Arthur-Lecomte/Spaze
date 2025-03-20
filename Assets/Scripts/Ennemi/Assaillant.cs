using UnityEngine;

public class Assaillant : Ennemi
{
    protected override void Start()
    {
        // Appeler la méthode Start de la classe parente
        base.Start();

        Debug.Log("Assaillant initialisé avec des valeurs spécifiques.");
    }

    protected override void MoveTowardsPlayer(float distanceToPlayer)
    {
        if (distanceToPlayer > 1f)
        {
            // Calculer la direction sur le plan ZX
            Vector3 direction = (new Vector3(player.position.x, 0, player.position.z) - new Vector3(transform.position.x, 0, transform.position.z)).normalized;

            // Déplacer l'ennemi dans cette direction
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    protected override void ShootAtPlayer()
    {
        // Appeler la méthode parente pour conserver le comportement de base
        base.ShootAtPlayer();

    }
}