using UnityEngine;
namespace Spaze {
    public class Tir : MonoBehaviour {
        private bool fromPlayer;
        private float damage;
        private float range;

        [SerializeField] private float speed;
        private float distanceTraveled;
        private float ralentissement = 1;

        /// <summary>
        /// Définit les informations du tir.
        /// </summary>
        /// <param name="creator">Indique si le tir provient du joueur.</param>
        /// <param name="damageValue">La valeur des dégâts du tir.</param>
        /// <param name="rangeValue">La portée du tir.</param>
        public void SetInformations(bool creator, float damageValue, float rangeValue) {
            fromPlayer = creator;
            damage = damageValue;
            range = rangeValue + 1;
        }

        /// <summary>
        /// Méthode appelée à chaque frame pour mettre à jour l'état du tir.
        /// </summary>
        private void Update() {
            float distance = speed / ralentissement * Time.deltaTime;
            transform.Translate(Vector3.forward * distance);
            distanceTraveled += distance;

            if (distanceTraveled >= range) {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Vérifie si le tir provient du joueur.
        /// </summary>
        /// <returns>True si le tir provient du joueur, sinon False.</returns>
        public bool IsFromPlayer() {
            return fromPlayer;
        }

        /// <summary>
        /// Méthode appelée lorsqu'un autre collider entre en contact avec le tir.
        /// </summary>
        /// <param name="other">Le collider entrant en contact.</param>
        private void OnTriggerEnter(Collider other) {
            ICanTakeDamage hit = other.GetComponent<ICanTakeDamage>();
            if (hit != null && hit.AmIPlayer() != fromPlayer) {
                hit.TakeDamage(damage);
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Modifie la vitesse du tir lorsqu'il entre dans une zone de ralentissement.
        /// </summary>
        /// <param name="power">La puissance du ralentissement.</param>
        public void InSlowArea(float power) {
            ralentissement += power;
        }
    }
}