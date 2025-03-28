using System.Collections.Generic;
using UnityEngine;
namespace Spaze {
    public class ArrowPointer : MonoBehaviour {
        private Transform playerTransform;
        [SerializeField] private GameObject arrowPrefab; // Le prefab de la flèche
        [SerializeField] private float distanceFromPlayer; // Distance des flèches par rapport au joueur
        private List<GameObject> arrows = new();
        private GameObject arrowContainer;

        /// <summary>
        /// Méthode appelée au démarrage. Initialise les composants nécessaires.
        /// </summary>
        private void Start() {
            playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
            arrowContainer = new GameObject("ARROW POINTER");
        }

        /// <summary>
        /// Méthode appelée à chaque frame pour mettre à jour l'état des flèches.
        /// </summary>
        void Update() {
            UpdateArrows();
        }

        /// <summary>
        /// Met à jour les flèches pour qu'elles pointent vers les ennemis actifs.
        /// </summary>
        private void UpdateArrows() {
            List<GameObject> activeEnemies = WaveManager.Instance.activeEnemies;

            // Supprimer les flèches en trop
            while (arrows.Count > activeEnemies.Count) {
                Destroy(arrows[^1]);
                arrows.RemoveAt(arrows.Count - 1);
            }

            // Ajouter des flèches si nécessaire
            while (arrows.Count < activeEnemies.Count) {
                GameObject arrow = Instantiate(arrowPrefab, playerTransform.position, Quaternion.identity, arrowContainer.transform);
                arrows.Add(arrow);
            }

            // Mettre à jour la position et la rotation des flèches
            for (int i = 0; i < arrows.Count; i++) {
                Transform enemy = activeEnemies[i].transform;
                Vector3 direction = (enemy.position - playerTransform.position).normalized;
                Vector3 arrowPosition = playerTransform.position + new Vector3(direction.x, 0, direction.z) * distanceFromPlayer;
                arrows[i].transform.position = arrowPosition;

                // Ajuster la rotation pour que les flèches pointent vers les ennemis
                float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
                arrows[i].transform.rotation = Quaternion.Euler(90, 90, angle);
            }
        }
    }
}