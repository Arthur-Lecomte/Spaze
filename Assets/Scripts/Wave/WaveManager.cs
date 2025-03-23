using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private List<GameObject> enemyPrefabs; // Liste des ennemis possibles
    [SerializeField] private float spawnRadius; // Distance autour du joueur pour spawn
    [SerializeField] private float spawnSpread; // écart possible entre les spawns
    [SerializeField] private float timeBetweenWaves; // Temps entre chaque vague
    [SerializeField] private int startEnemies; // Nombre d'ennemis de la première vague
    private TextMeshProUGUI waveInfoText; // Référence au texte UI

    private int currentWave = 0; // Numéro de la vague actuelle
    private List<GameObject> activeEnemies = new(); // Liste des ennemis actifs
    private int nextEnemyIndex = 0; // Index du prochain ennemi à spawn
    private float timeUntilNextWave; // Temps restant avant la prochaine vague

    void Start()
    {
        waveInfoText = GetComponent<TextMeshProUGUI>();
        StartCoroutine(SpawnWaves());
    }

    void Update()
    {
        if (activeEnemies.Count > 0)
        {
            waveInfoText.text = $"Ennemis restants : {activeEnemies.Count}";
        }
        else
        {
            waveInfoText.text = $"Prochaine vague : {Mathf.CeilToInt(timeUntilNextWave)} s";
        }
    }

    /// <summary>
    /// Gère l'apparaition des vagues d'ennemies.
    /// </summary>
    /// <returns></returns>
    private IEnumerator SpawnWaves()
    {
        while (true)
        {
            timeUntilNextWave = timeBetweenWaves;
            while (timeUntilNextWave > 0)
            {
                yield return null;
                timeUntilNextWave -= Time.deltaTime;
            }

            currentWave++;
            int enemyCount = startEnemies + (currentWave - 1) / 5; // Ajouter 1 ennemi toutes les 5 vagues

            Debug.Log($"Vague {currentWave} - {enemyCount} ennemis");

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(0.2f); // Petit délai entre chaque spawn
            }

            // Attendre que tous les ennemis soient détruits
            yield return new WaitUntil(() => activeEnemies.Count == 0);
        }
    }

    /// <summary>
    /// Instantie un ennemi aléatoire provenant de la liste d'ennemie si elle n'est pas vide et que le player n'est pas null.
    /// </summary>
    private void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0 || Vaisseau.Instance.gameObject == null)
        {
            Debug.LogWarning("Aucun ennemi disponible ou joueur non défini !");
            return;
        }

        // Choisir un ennemi de manière cyclique
        GameObject enemyPrefab = enemyPrefabs[nextEnemyIndex];
        nextEnemyIndex = (nextEnemyIndex + 1) % enemyPrefabs.Count;

        Vector3 spawnPosition = GetRandomSpawnPosition();
        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        activeEnemies.Add(enemy);

        // Ajouter un évènement pour retirer l'ennemi de la liste lorsqu'il est détruit
        if (enemy.TryGetComponent<Enemy>(out var enemyComponent))
        {
            enemyComponent.Level = 1 + (currentWave - 1) / 2; // Augmenter le niveau toutes les 2 vagues
            enemyComponent.OnDestroyed += (destroyedEnemy) => activeEnemies.Remove(destroyedEnemy);
        }
    }

    /// <summary>
    /// Calcule une position aléatoire de spawn d'un ennemi selon les paramètres de la class.
    /// </summary>
    /// <returns>Renvoie un Vector3 pour le spawn d'un ennemi.</returns>
    private Vector3 GetRandomSpawnPosition()
    {
        float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float offsetX = Random.Range(-spawnSpread, spawnSpread);
        float offsetZ = Random.Range(-spawnSpread, spawnSpread);

        Vector3 basePosition = new(
            Vaisseau.Instance.transform.position.x + Mathf.Cos(angle) * spawnRadius,
            Vaisseau.Instance.transform.position.y, // Garder la même hauteur que le joueur
            Vaisseau.Instance.transform.position.z + Mathf.Sin(angle) * spawnRadius
        );

        return basePosition + new Vector3(offsetX, 0, offsetZ);
    }

    public void RegisterEnemy(GameObject enemy)
    {
        activeEnemies.Add(enemy);

    }
}
