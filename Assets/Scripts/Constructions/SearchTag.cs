using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
namespace Spaze {
    public abstract class SearchTag : Construction {
        [SerializeField] private string tagTarget;
        [SerializeField] protected float range;
        [SerializeField] protected float speed;

        protected Transform turnTransform;
        protected float turnSpeed = 10f;

        private SphereCollider rangeCollider;
        private float nextActionTime;
        protected readonly List<Collider> InRange = new List<Collider>();
        private GameObject currentTarget;

        /// <summary>
        /// Initialise la construction avec les paramètres de rareté spécifiés.
        /// </summary>
        /// <param name="rarityConstruction">La rareté de la construction.</param>
        public override void Initialisation(RarityConstruction rarityConstruction) {
            base.Initialisation(rarityConstruction);

            rangeCollider = gameObject.AddComponent<SphereCollider>();
            rangeCollider.isTrigger = true;
            rangeCollider.radius = range;
        }

        /// <summary>
        /// Effectue des actions spécifiques lors de l'amélioration de la construction.
        /// </summary>
        protected override void PerformUpgrade() {
            rangeCollider.radius = range;
        }

        /// <summary>
        /// Méthode appelée lorsqu'un autre collider entre dans le trigger.
        /// </summary>
        /// <param name="other">Le collider entrant.</param>
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag(tagTarget)) {
                InRange.Add(other);
            }
        }

        /// <summary>
        /// Méthode appelée lorsqu'un autre collider sort du trigger.
        /// </summary>
        /// <param name="other">Le collider sortant.</param>
        private void OnTriggerExit(Collider other) {
            if (other.CompareTag(tagTarget)) {
                if (currentTarget == other.gameObject) {
                    StopAnimation();
                }

                InRange.Remove(other);
            }
        }

        /// <summary>
        /// Méthode appelée à chaque frame pour mettre à jour l'état de la construction.
        /// </summary>
        public void Update() {
            if (InRange.Count > 0) {
                currentTarget = GetClosest();
                if (currentTarget) {
                    Rotate(currentTarget.transform);
                    if (IsAlignedWithTarget(currentTarget.transform)) {
                        DoAnimation(currentTarget.transform);
                        if (Time.time >= nextActionTime) {
                            DoAction(currentTarget.transform);
                            nextActionTime = Time.time + 1f / speed;
                        }
                    } else {
                        StopAnimation();
                    }
                }
            } else {
                currentTarget = null;
                turnTransform.rotation = Quaternion.Slerp(turnTransform.rotation, transform.parent.rotation, Time.deltaTime * turnSpeed / 10);
            }
        }

        /// <summary>
        /// Obtient l'objet le plus proche parmi ceux en range.
        /// </summary>
        /// <returns>L'objet le plus proche.</returns>
        private GameObject GetClosest() {
            GameObject closestObject = null;
            float closestDistance = Mathf.Infinity;

            foreach (Collider c in InRange.ToList()) {
                if (!c) {
                    InRange.Remove(c);
                    continue;
                }

                float distance = Vector3.Distance(transform.position, c.ClosestPoint(transform.position));
                if (distance < closestDistance) {
                    closestDistance = distance;
                    closestObject = c.gameObject;
                }
            }

            return closestObject;
        }

        /// <summary>
        /// Fait tourner la construction pour faire face à la cible spécifiée.
        /// </summary>
        /// <param name="target">La cible à viser.</param>
        private void Rotate(Transform target) {
            Vector3 direction = (target.position - turnTransform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            turnTransform.rotation = Quaternion.Slerp(turnTransform.rotation, lookRotation, Time.deltaTime * turnSpeed);
        }

        /// <summary>
        /// Vérifie si la construction est alignée avec la cible spécifiée.
        /// </summary>
        /// <param name="target">La cible à vérifier.</param>
        /// <returns>True si alignée, sinon False.</returns>
        private bool IsAlignedWithTarget(Transform target) {
            Vector3 directionToTarget = (target.position - turnTransform.position).normalized;
            float angle = Vector3.Angle(turnTransform.forward, directionToTarget);
            return angle < 10f;
        }

        /// <summary>
        /// Effectue une action sur la cible spécifiée.
        /// </summary>
        /// <param name="target">La cible de l'action.</param>
        protected virtual void DoAction(Transform target) { }

        /// <summary>
        /// Démarre l'animation pour la cible spécifiée.
        /// </summary>
        /// <param name="target">La cible de l'animation.</param>
        protected virtual void DoAnimation(Transform target) { }

        /// <summary>
        /// Arrête l'animation en cours.
        /// </summary>
        protected virtual void StopAnimation() { }
    }
}