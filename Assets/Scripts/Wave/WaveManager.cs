using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private GameObject player; // Référence au joueur
    [SerializeField] private List<GameObject> enemyPrefabs; // Liste des ennemis possibles
    [SerializeField] private float spawnRadius; // Distance autour du joueur pour spawn
    [SerializeField] private float spawnSpread; // Écart possible entre les spawns
    [SerializeField] private float timeBetweenWaves; // Temps entre chaque vague
    [SerializeField] private int startEnemies; // Nombre d'ennemis de la première vague
    [SerializeField] private int enemiesIncrement; // Combien d'ennemis en plus à chaque vague

    private int currentWave = 0; // Numéro de la vague actuelle

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    /// <summary>
    /// Gère l'apparaition des vagues d'ennemies.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWaves()
    {
        yield return new WaitForSeconds(timeBetweenWaves); // Délai avant la première vague

        while (true)
        {
            currentWave++;
            int enemyCount = startEnemies + (currentWave - 1) * enemiesIncrement;

            Debug.Log($"Vague {currentWave} - {enemyCount} ennemis");

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.2f); // Petit délai entre chaque spawn
            }

            yield return new WaitForSeconds(timeBetweenWaves); // Attente avant la prochaine vague
        }
    }

    /// <summary>
    /// Instantie un ennemie aléatoire provénant de la liste d'ennemie si elle n'est pas vide et que le player n'est pas null.
    /// </summary>
    private void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0 || player == null)
        {
            Debug.LogWarning("Aucun ennemi disponible ou joueur non défini !");
            return;
        }

        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)]; // Choisir un ennemi aléatoire

        Vector2 spawnPosition = GetRandomSpawnPosition();
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// Calcule une position aléatoire de spawn d'un ennemie selon les paramètres de la class.
    /// </summary>
    /// <returns>Renvoie un Vector2D pour le spawn d'un ennemie.</returns>
    private Vector2 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float offsetX = Random.Range(-spawnSpread, spawnSpread);
        float offsetY = Random.Range(-spawnSpread, spawnSpread);

        Vector2 basePosition = new(
            player.transform.position.x + Mathf.Cos(angle) * spawnRadius,
            player.transform.position.y + Mathf.Sin(angle) * spawnRadius
        );

        return basePosition + new Vector2(offsetX, offsetY);
    }
}
