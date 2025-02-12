using UnityEngine;

public class SpawnRessources : MonoBehaviour {
    public GameObject asteroidPrefab; // Préfabriqué de l'astéroïde

    void Start() {
        SpawnAsteroids(25);
    }

    void SpawnAsteroids(int count) {
        for (int i = 0; i < count; i++) {
            Vector3 randomPosition = new Vector3(Random.Range(-100, 101), 0, Random.Range(-100, 101));
            Instantiate(asteroidPrefab, randomPosition, Quaternion.identity);
        }
    }
}
