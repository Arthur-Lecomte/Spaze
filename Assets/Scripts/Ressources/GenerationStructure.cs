using System.Collections.Generic;
using UnityEngine;

public class GenerationStructure : MonoBehaviour {
    [Header("Ressource Data")]
    [SerializeField] private AsteroidData asteroidData;
    [SerializeField] private WreckData wreckData;
    [SerializeField] private GameObject shopPrefab;

    [Header("Generation Settings")]
    [SerializeField] private int cellSize = 50; // Taille des cellules de la grille
    [SerializeField] private int spawnRadius = 3; // Nombre de cellules autour du joueur à charger
    [SerializeField] private int seed; // Seed aléatoire pour générer les structures

    [Header("Player")]
    [SerializeField] private Transform player;

    private HashSet<Vector2Int> loadedCells = new();
    private Dictionary<Vector2Int, List<GameObject>> spawnedObjects = new();

    private Transform asteroidParent;
    private Transform wreckParent;
    private Transform shopParent;

    private System.Random random;

    void Start() {
        // Génération d'une seed unique
        seed = Random.Range(0, 100000);
        random = new System.Random(seed);

        // Création des parents dynamiquement
        Transform structureParent = new GameObject("STRUCTURE").transform;
        asteroidParent = new GameObject("ASTEROIDE").transform;
        wreckParent = new GameObject("EPAVE").transform;
        shopParent = new GameObject("SHOP").transform;

        // Assigner les parents à STRUCTURE
        asteroidParent.parent = structureParent;
        wreckParent.parent = structureParent;
        shopParent.parent = structureParent;
    }

    void Update() {
        UpdateLoadedCells();
    }

    void UpdateLoadedCells() {
        Vector2Int playerCell = GetCellCoordinates(player.position);
        HashSet<Vector2Int> newLoadedCells = new();

        // Charger les nouvelles cellules
        for (int x = -spawnRadius; x <= spawnRadius; x++) {
            for (int y = -spawnRadius; y <= spawnRadius; y++) {
                Vector2Int cellCoord = new Vector2Int(playerCell.x + x, playerCell.y + y);
                newLoadedCells.Add(cellCoord);

                if (!loadedCells.Contains(cellCoord)) {
                    GenerateCell(cellCoord);
                }
            }
        }

        // Supprimer les anciennes cellules qui ne sont plus dans la zone de spawn
        HashSet<Vector2Int> cellsToRemove = new HashSet<Vector2Int>(loadedCells);
        cellsToRemove.ExceptWith(newLoadedCells);

        foreach (Vector2Int cell in cellsToRemove) {
            DestroyCell(cell);
        }

        // Mettre à jour la liste des cellules chargées
        loadedCells = newLoadedCells;
    }

    void GenerateCell(Vector2Int cellCoord) {
        random = new System.Random(seed + cellCoord.x * 73856093 + cellCoord.y * 19349663); // Seed unique par cellule

        float asteroidChance = (float)random.NextDouble();
        float wreckChance = (float)random.NextDouble();
        float shopChance = (float)random.NextDouble();

        Vector3 cellCenter = new Vector3(cellCoord.x * cellSize, 0, cellCoord.y * cellSize);
        List<GameObject> objectsInCell = new List<GameObject>();

        // Limiter à une seule structure par cellule
        if (asteroidChance < 0.5f) // 50% de chance d'apparition d'un astéroïde
        {
            TryInstantiateVariant(asteroidData.variants, cellCenter, asteroidParent, objectsInCell);
        } else if (wreckChance < 0.2f) // 20% de chance pour une épave
          {
            TryInstantiateVariant(wreckData.variants, cellCenter, wreckParent, objectsInCell);
        } else if (shopChance < 0.1f) // 10% de chance pour un magasin
          {
            TryInstantiateObject(shopPrefab, cellCenter, shopParent, objectsInCell);
        }

        spawnedObjects[cellCoord] = objectsInCell;
        loadedCells.Add(cellCoord);

        Debug.Log($"Cell generated at {cellCoord}");
    }

    void TryInstantiateVariant(List<VariantData> variants, Vector3 cellCenter, Transform parent, List<GameObject> objectsInCell) {
        float randomValue = (float)random.NextDouble();
        float cumulativeProbability = 0f;

        foreach (var variant in variants) {
            cumulativeProbability += variant.probability;
            if (randomValue < cumulativeProbability) {
                TryInstantiateObject(variant.prefab, cellCenter, parent, objectsInCell, variant.ressourceType);
                break;
            }
        }
    }

    void TryInstantiateObject(GameObject prefab, Vector3 cellCenter, Transform parent, List<GameObject> objectsInCell) {
        Vector3 spawnPosition = GetRandomPositionInCell(cellCenter);
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
        objectsInCell.Add(obj);
    }

    void TryInstantiateObject(GameObject prefab, Vector3 cellCenter, Transform parent, List<GameObject> objectsInCell, TypeRessource ressourceType) {
        Vector3 spawnPosition = GetRandomPositionInCell(cellCenter);
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
        objectsInCell.Add(obj);

        // Assigner une ressource à la structure si applicable
        Structure structure = obj.GetComponent<Structure>();
        if (structure != null) {
            int quantite = random.Next(1, 101); // Quantité aléatoire entre 1 et 100
            structure.Ressource = new Ressource(ressourceType, quantite);
        }
    }

    void DestroyCell(Vector2Int cellCoord) {
        if (spawnedObjects.ContainsKey(cellCoord)) {
            foreach (GameObject obj in spawnedObjects[cellCoord]) {
                if (obj != null) Destroy(obj);
            }
            spawnedObjects.Remove(cellCoord);
        }

        Debug.Log($"Cell destroyed at {cellCoord}");
    }

    Vector2Int GetCellCoordinates(Vector3 position) {
        return new Vector2Int(Mathf.FloorToInt(position.x / cellSize), Mathf.FloorToInt(position.z / cellSize));
    }

    Vector3 GetRandomPositionInCell(Vector3 cellCenter) {
        Vector3 randomOffset = new Vector3(Random.Range(-cellSize / 2, cellSize / 2), 0, Random.Range(-cellSize / 2, cellSize / 2));
        return cellCenter + randomOffset;
    }

    void OnDrawGizmos() {
        if (loadedCells == null) return;

        Gizmos.color = Color.green;
        foreach (Vector2Int cell in loadedCells) {
            Vector3 cellCenter = new Vector3(cell.x * cellSize, 0, cell.y * cellSize);
            Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 1, cellSize));
        }
    }
}
