using UnityEngine;
namespace Spaze {
    public class Extracteur : SearchTag {
        [SerializeField] private float quantity;
        [SerializeField] private float quantityWithPercent;
        private static float percent;

        private LaserBeam laserBeam;

        private Animator animator;
        private static readonly int Extract = Animator.StringToHash("Extract");

        /// <summary>
        /// Initialise l'extracteur avec les paramètres de rareté spécifiés.
        /// </summary>
        /// <param name="rarityConstruction">La rareté de la construction.</param>
        public override void Initialisation(RarityConstruction rarityConstruction) {
            base.Initialisation(rarityConstruction);
            turnTransform = transform.GetChild(0);

            animator = GetComponentInChildren<Animator>();
            laserBeam = GetComponent<LaserBeam>();
            OnPercentChanged(TypeUpgrade.Extraction, percent);
        }

        /// <summary>
        /// Effectue l'action d'extraction sur la cible spécifiée.
        /// </summary>
        /// <param name="target">La cible de l'extraction.</param>
        protected override void DoAction(Transform target) {
            if (laserBeam.IsLaserEnabled()) { // Si le laser est activé, on extrait les ressources
                if (!target.gameObject.GetComponent<Structure>().Extract(quantityWithPercent)) {
                    // Il n'y a plus de ressource à extraire
                    InRange.Remove(target.gameObject.GetComponent<Collider>());
                    StopAnimation();
                }
            }
        }

        /// <summary>
        /// Démarre l'animation d'extraction sur la cible spécifiée.
        /// </summary>
        /// <param name="target">La cible de l'animation.</param>
        protected override void DoAnimation(Transform target) {
            animator.SetBool(Extract, true);
            laserBeam.EnableLaser(target);
        }

        /// <summary>
        /// Arrête l'animation d'extraction.
        /// </summary>
        protected override void StopAnimation() {
            animator.SetBool(Extract, false);
            laserBeam.DisableLaser();
        }

        /// <summary>
        /// Effectue des actions spécifiques lors de l'amélioration de l'extracteur.
        /// </summary>
        protected override void PerformUpgrade() {
            OnPercentChanged(TypeUpgrade.Extraction, percent);
        }

        /// <summary>
        /// Méthode appelée lorsque le pourcentage change.
        /// </summary>
        /// <param name="typeUpgrade">Le type d'amélioration.</param>
        /// <param name="value">Le pourcentage de changement.</param>
        protected override void OnPercentChanged(TypeUpgrade typeUpgrade, float value) {
            if (value == 0) {
                UpgradeColumn.getValue(typeUpgrade);
                return;
            }

            if (typeUpgrade == TypeUpgrade.Extraction) {
                percent = value;
                quantityWithPercent = quantity * percent;
            }
        }

        /// <summary>
        /// Dessine des gizmos pour visualiser la portée de l'extracteur dans l'éditeur.
        /// </summary>
        public void OnDrawGizmos() {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}