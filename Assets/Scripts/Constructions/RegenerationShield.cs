using System.Collections.Generic;
using UnityEngine;
namespace Spaze {
    public class RegenerationShield : SearchShield {
        [SerializeField] private float regeneration;
        [SerializeField] private float regenerationMax;
        [SerializeField] private float range;

        private float regenerationPerShield;
        public float RegenerationPerShield => regenerationPerShield;

        private Dictionary<Shield, LaserBeam> laserBeams;
        [SerializeField] private GameObject laserPrefab;
        [SerializeField] private GameObject firePoint;

        /// <summary>
        /// Initialise le bouclier de r�g�n�ration avec les param�tres de raret� sp�cifi�s.
        /// </summary>
        /// <param name="rarityConstruction">La raret� de la construction.</param>
        public override void Initialisation(RarityConstruction rarityConstruction) {
            base.Initialisation(rarityConstruction);
            range = 1; // DEBUG !!!
            TypeToSearch = typeof(Shield);
            laserBeams = new Dictionary<Shield, LaserBeam>();
            newShield += ListSearchShields;
        }

        /// <summary>
        /// D�finit le parent de cette construction et ajuste sa position et son �chelle.
        /// </summary>
        /// <param name="parent">Le parent � d�finir.</param>
        /// <param name="onModule">Indique si la construction est sur un module.</param>
        public override void SetChildOf(Transform parent, bool onModule = true) {
            base.SetChildOf(parent, onModule);

            ListSearchShields();
        }

        /// <summary>
        /// Liste les boucliers � proximit� et configure les faisceaux laser.
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

            regenerationPerShield = Mathf.Min(regenerationMax, regeneration / NearbySearchShields.Count);
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
        /// Effectue des actions sp�cifiques lors de l'am�lioration du bouclier de r�g�n�ration.
        /// </summary>
        protected override void PerformUpgrade() {
            ListSearchShields();
        }

        /// <summary>
        /// Active ou d�sactive le faisceau laser pour un bouclier sp�cifique.
        /// </summary>
        /// <param name="shield">Le bouclier cible.</param>
        /// <param name="active">Indique si le faisceau laser doit �tre activ� ou d�sactiv�.</param>
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
        /// M�thode appel�e lors de la destruction de l'objet.
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
