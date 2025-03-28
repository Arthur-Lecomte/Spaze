using System.Collections.Generic;
using UnityEngine;
namespace Spaze {
    public class RegenerationShield : SearchShield {
        [SerializeField] private float capacity;
        [SerializeField] private float regenerationMax;
        [SerializeField] private float range;

        private float regenerationPerShield;
        public float RegenerationPerShield => regenerationPerShield;

        private Dictionary<Shield, LaserBeam> laserBeams;
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private GameObject firePoint;

        /// <summary>
        /// Initialise le bouclier de régénération avec les paramètres de rareté spécifiés.
        /// </summary>
        /// <param name="rarityConstruction">La rareté de la construction.</param>
        public override void Initialisation(RarityConstruction rarityConstruction) {
            base.Initialisation(rarityConstruction);
            range = 1; // DEBUG !!!
            TypeToSearch = typeof(Shield);
            laserBeams = new Dictionary<Shield, LaserBeam>();
            newShield += ListSearchShields;
        }

        /// <summary>
        /// Définit le parent de cette construction et ajuste sa position et son échelle.
        /// </summary>
        /// <param name="parent">Le parent à définir.</param>
        /// <param name="onModule">Indique si la construction est sur un module.</param>
        public override void SetChildOf(Transform parent, bool onModule = true) {
            base.SetChildOf(parent, onModule);

            ListSearchShields();
        }

        /// <summary>
        /// Liste les boucliers à proximité et configure les faisceaux laser.
        /// </summary>
        private void ListSearchShields() {
            foreach (LaserBeam laserBeam in laserBeams.Values) {
                laserBeam.DisableLaser();
                Destroy(laserBeam);
            }
            laserBeams.Clear();

            foreach (SearchShield searchShield in NearbySearchShields) {
                searchShield.RemoveSearchShield(this);
            }
            NearbySearchShields = new List<SearchShield>();

            if (firePoint) {
                foreach (Transform child in firePoint.transform) {
                    Destroy(child.gameObject);
                }
            }

            if (this == null) {
                Debug.Log("BUG");
                newShield -= ListSearchShields;
                return;
            }

            AddonModule moduleOn = transform.parent.GetComponent<AddonModule>();
            if (moduleOn != null) {
                NearbySearchShields = moduleOn.GetSearchShieldAtDistance(TypeToSearch, (int)range);
                foreach (SearchShield shield in NearbySearchShields) {
                    shield.AddSearchShield(this);
                }
            }

            regenerationPerShield = Mathf.Min(regenerationMax, capacity / NearbySearchShields.Count);
            foreach (SearchShield t in NearbySearchShields) {
                LaserBeam lb = gameObject.AddComponent<LaserBeam>();
                lb.collisionMask = LayerMask.GetMask("Default");
                lb.firePoint = firePoint;
                lb.laser = Instantiate(laserPrefab, firePoint.transform);
                lb.ignoreObjectInFront = true;
                lb.DisableLaser();
                laserBeams[(Shield)t] = lb;
            }
        }

        /// <summary>
        /// Effectue des actions spécifiques lors de l'amélioration du bouclier de régénération.
        /// </summary>
        protected override void PerformUpgrade() {
            ListSearchShields();
        }

        /// <summary>
        /// Active ou désactive le faisceau laser pour un bouclier spécifique.
        /// </summary>
        /// <param name="shield">Le bouclier cible.</param>
        /// <param name="active">Indique si le faisceau laser doit être activé ou désactivé.</param>
        public void ActiveLaserBeam(Shield shield, bool active) {
            if (this == null) {
                Debug.Log("BUG2");
                return;
            }

            if (active) {
                laserBeams[shield].EnableLaser(shield.transform);
            } else {
                laserBeams[shield].DisableLaser();
            }
        }

        /// <summary>
        /// Méthode appelée lors de la destruction de l'objet.
        /// </summary>
        private void OnDestroy() {
            newShield -= ListSearchShields;

            foreach (LaserBeam laserBeam in laserBeams.Values) {
                laserBeam.DisableLaser();
                Destroy(laserBeam);
            }
            laserBeams.Clear();

            foreach (SearchShield searchShield in NearbySearchShields) {
                searchShield.RemoveSearchShield(this);
            }
            NearbySearchShields = new List<SearchShield>();

            if (firePoint) {
                foreach (Transform child in firePoint.transform) {
                    Destroy(child.gameObject);
                }
            }
        }
    }
}