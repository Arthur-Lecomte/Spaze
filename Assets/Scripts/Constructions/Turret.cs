using System;
using UnityEngine;
namespace Spaze {
    public class Turret : SearchTag {
        [SerializeField] private float damage;
        [SerializeField] private float damageWithPercent;
        private static float percent;

        [SerializeField] private GameObject bulletPrefab;
        private LaserBeam laserBeam;

        private Transform[] missileSpawnPoints;
        private int currentSpawnPointIndex;

        public static Action<GameObject> onEnemyKilled;

        /// <summary>
        /// Initialise la tourelle avec les paramètres de rareté spécifiés.
        /// </summary>
        /// <param name="rarityConstruction">La rareté de la construction.</param>
        public override void Initialisation(RarityConstruction rarityConstruction) {
            base.Initialisation(rarityConstruction);
            turnTransform = transform.GetChild(0).GetChild(0);
            turnSpeed = 10;

            missileSpawnPoints = new Transform[turnTransform.childCount];
            for (int i = 0; i < turnTransform.childCount; i++) {
                missileSpawnPoints[i] = turnTransform.GetChild(i);
            }

            onEnemyKilled += CheckList;

            laserBeam = GetComponent<LaserBeam>();
            OnPercentChanged(TypeUpgrade.Attack, percent);
        }

        /// <summary>
        /// Effectue l'action de la tourelle sur la cible spécifiée.
        /// </summary>
        /// <param name="target">La cible de l'action.</param>
        protected override void DoAction(Transform target) {
            if (laserBeam) {
                if (laserBeam.IsLaserEnabled()) { // Si le laser est activé, on fait des dégâts à l'ennemi
                    target.gameObject.GetComponent<Enemy>().TakeDamage(damageWithPercent);

                }
            } else {
                Transform spawnPoint = missileSpawnPoints[currentSpawnPointIndex];
                currentSpawnPointIndex = (currentSpawnPointIndex + 1) % missileSpawnPoints.Length;

                Vector3 direction = (target.position - transform.position).normalized;

                GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.LookRotation(direction));
                bullet.GetComponent<Tir>().SetInformations(Vaisseau.Instance.gameObject, damageWithPercent, range);
            }
        }

        /// <summary>
        /// Démarre l'animation de la tourelle pour la cible spécifiée.
        /// </summary>
        /// <param name="target">La cible de l'animation.</param>
        protected override void DoAnimation(Transform target) {
            if (laserBeam) {
                laserBeam.EnableLaser(target);
            }
        }

        /// <summary>
        /// Arrête l'animation de la tourelle.
        /// </summary>
        protected override void StopAnimation() {
            if (laserBeam) {
                laserBeam.DisableLaser();
            }
        }

        /// <summary>
        /// Vérifie la liste des ennemis à portée lorsque l'un d'eux est tué.
        /// </summary>
        /// <param name="enemy">L'ennemi tué.</param>
        private void CheckList(GameObject enemy) {
            if (InRange.Contains(enemy.GetComponent<Collider>())) {
                InRange.Remove(enemy.GetComponent<Collider>());
                StopAnimation();
            }
        }

        /// <summary>
        /// Effectue des actions spécifiques lors de l'amélioration de la tourelle.
        /// </summary>
        protected override void PerformUpgrade() {
            OnPercentChanged(TypeUpgrade.Attack, percent);
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

            if (typeUpgrade == TypeUpgrade.Attack) {
                percent = value;
                damageWithPercent = damage * percent;
            }
        }

        /// <summary>
        /// Dessine des gizmos pour visualiser la portée de la tourelle dans l'éditeur.
        /// </summary>
        public void OnDrawGizmos() {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, range);
        }
    }
}