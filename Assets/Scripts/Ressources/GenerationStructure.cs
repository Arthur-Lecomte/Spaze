using System.Collections.Generic;
using UnityEngine;

public class GenerationStructure : MonoBehaviour
{
    public GameObject asteroidPrefab;
    public GameObject wreckPrefab;
    public GameObject shopPrefab;

    public int cellSize = 50; // Taille des cellules de la grille
    public int spawnRadius = 3; // Nombre de cellules autour du joueur à charger
    public int seed; // Seed aléatoire pour générer les structures

    [SerializeField] private Transform player;
    private HashSet<Vector2Int> loadedCells = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, List<GameObject>> spawnedObjects = new Dictionary<Vector2Int, List<GameObject>>();

    [SerializeField] private Transform asteroidParent;
    [SerializeField] private Transform wreckParent;
    [SerializeField] private Transform shopParent;

    private System.Random random;

    void Start()
    {
        seed = Random.Range(0, 100000); // Génération d'une seed unique
        random = new System.Random(seed);
    }

    void Update()
    {
        UpdateLoadedCells();
    }

    void UpdateLoadedCells()
    {
        Vector2Int playerCell = GetCellCoordinates(player.position);
        HashSet<Vector2Int> newLoadedCells = new HashSet<Vector2Int>();

        // Charger les nouvelles cellules
        for (int x = -spawnRadius; x <= spawnRadius; x++)
        {
            for (int y = -spawnRadius; y <= spawnRadius; y++)
            {
                Vector2Int cellCoord = new Vector2Int(playerCell.x + x, playerCell.y + y);
                newLoadedCells.Add(cellCoord);

                if (!loadedCells.Contains(cellCoord))
                {
                    GenerateCell(cellCoord);
                }
            }
        }

        // Supprimer les anciennes cellules qui ne sont plus dans la zone de spawn
        HashSet<Vector2Int> cellsToRemove = new HashSet<Vector2Int>(loadedCells);
        cellsToRemove.ExceptWith(newLoadedCells);

        foreach (Vector2Int cell in cellsToRemove)
        {
            DestroyCell(cell);
        }

        // Mettre à jour la liste des cellules chargées
        loadedCells = newLoadedCells;
    }

    void GenerateCell(Vector2Int cellCoord)
    {
        random = new System.Random(seed + cellCoord.x * 73856093 + cellCoord.y * 19349663); // Seed unique par cellule

        float asteroidChance = (float)random.NextDouble();
        float wreckChance = (float)random.NextDouble();
        float shopChance = (float)random.NextDouble();

        Vector3 cellCenter = new Vector3(cellCoord.x * cellSize, 0, cellCoord.y * cellSize);
        List<GameObject> objectsInCell = new List<GameObject>();

        TryInstantiateObject(asteroidChance, 0.5f, asteroidPrefab, cellCenter, asteroidParent, objectsInCell);
        TryInstantiateObject(wreckChance, 0.2f, wreckPrefab, cellCenter, wreckParent, objectsInCell);
        TryInstantiateObject(shopChance, 0.1f, shopPrefab, cellCenter, shopParent, objectsInCell);

        spawnedObjects[cellCoord] = objectsInCell;
        loadedCells.Add(cellCoord);
    }

    void TryInstantiateObject(float chance, float threshold, GameObject prefab, Vector3 cellCenter, Transform parent, List<GameObject> objectsInCell)
    {
        if (chance < threshold)
        {
            Vector3 spawnPosition = GetRandomPositionInCell(cellCenter);
            GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
            objectsInCell.Add(obj);
        }
    }

    void DestroyCell(Vector2Int cellCoord)
    {
        if (spawnedObjects.ContainsKey(cellCoord))
        {
            foreach (GameObject obj in spawnedObjects[cellCoord])
            {
                if (obj != null) Destroy(obj);
            }
            spawnedObjects.Remove(cellCoord);
        }
    }

    Vector2Int GetCellCoordinates(Vector3 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x / cellSize), Mathf.FloorToInt(position.z / cellSize));
    }

    Vector3 GetRandomPositionInCell(Vector3 cellCenter)
    {
        Vector3 randomOffset = new Vector3(Random.Range(-cellSize / 2, cellSize / 2), 0, Random.Range(-cellSize / 2, cellSize / 2));
        return cellCenter + randomOffset;
    }
}
