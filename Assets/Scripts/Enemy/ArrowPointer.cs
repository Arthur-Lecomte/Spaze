using System.Collections.Generic;
using UnityEngine;

public class ArrowPointer : MonoBehaviour {
    private Transform playerTransform;
    [SerializeField] private GameObject arrowPrefab; // Le prefab de la flèche
    public float distanceFromPlayer = 2.0f; // Distance des flèches par rapport au joueur
    private List<GameObject> arrows = new();

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update() {
        UpdateArrows();
    }

    private void UpdateArrows() {
        List<GameObject> activeEnemies = WaveManager.Instance.activeEnemies;

        // Supprimer les flèches en trop
        while (arrows.Count > activeEnemies.Count) {
            Destroy(arrows[^1]);
            arrows.RemoveAt(arrows.Count - 1);
        }

        // Ajouter des flèches si nécessaire
        while (arrows.Count < activeEnemies.Count) {
            GameObject arrow = Instantiate(arrowPrefab, playerTransform.position, Quaternion.identity);
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
