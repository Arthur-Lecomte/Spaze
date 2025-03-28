using UnityEngine;
namespace Spaze {
    public class Tank : Enemy {

        /// <summary>
        /// Déplace le tank vers le joueur si celui-ci est hors de portée de tir.
        /// </summary>
        /// <param name="distanceToPlayer">La distance entre le tank et le joueur.</param>
        protected override void MoveTowardsPlayer(float distanceToPlayer) {
            if (distanceToPlayer > shootRange) {
                // Calculer la direction vers le joueur
                Vector3 direction = (player.position - transform.position).normalized;

                // Déplacer l'ennemi dans cette direction
                transform.position += direction * moveSpeed * Time.deltaTime;
            }
        }
    }
}