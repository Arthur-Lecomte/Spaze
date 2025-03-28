using UnityEngine;

namespace Spaze {
    public class Slower : Construction {
        [SerializeField] private float range;
        [SerializeField] private float slowPower;

        private SphereCollider trigger;
        [SerializeField] private Transform effect;

        /// <summary>
        /// Initialise le ralentisseur avec les paramètres de rareté spécifiés.
        /// </summary>
        /// <param name="rarityConstruction">La rareté de la construction.</param>
        public override void Initialisation(RarityConstruction rarityConstruction) {
            base.Initialisation(rarityConstruction);

            trigger = GetComponent<SphereCollider>();
            trigger.radius = range;
            effect.localScale = new Vector3(range, range, range);
        }

        /// <summary>
        /// Effectue des actions spécifiques lors de l'amélioration du ralentisseur.
        /// </summary>
        protected override void PerformUpgrade() {
            trigger.radius = range; // Augmente la portée de l'onde
            effect.localScale = new Vector3(range, range, range);
        }

        /// <summary>
        /// Méthode appelée lorsqu'un autre collider entre dans le trigger.
        /// </summary>
        /// <param name="other">Le collider entrant.</param>
        private void OnTriggerEnter(Collider other) {
            Tir projectile = other.GetComponent<Tir>();
            if (projectile != null && !projectile.IsFromPlayer()) {
                projectile.InSlowArea(slowPower);
            }
        }

        /// <summary>
        /// Méthode appelée lorsqu'un autre collider sort du trigger.
        /// </summary>
        /// <param name="other">Le collider sortant.</param>
        private void OnTriggerExit(Collider other) {
            Tir projectile = other.GetComponent<Tir>();
            if (projectile != null && !projectile.IsFromPlayer()) {
                projectile.InSlowArea(-slowPower);
            }
        }

        /// <summary>
        /// Dessine des gizmos pour visualiser la portée du ralentisseur dans l'éditeur.
        /// </summary>
        public void OnDrawGizmos() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}
