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
        Vector2Int playerCell = GetCellCoordinates(Vaisseau.Instance.transform.position);
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
        if (cellCoord == Vector2Int.zero) return; // Ne pas générer de structure à la position (0, 0)
        
        int cellSeed = seed + cellCoord.x * 73856093 + cellCoord.y * 19349663; // Seed unique par cellule
        random = new System.Random(cellSeed);

        float asteroidChance = (float)random.NextDouble();
        float wreckChance = (float)random.NextDouble();
        float shopChance = (float)random.NextDouble();

        Vector3 cellCenter = new Vector3(cellCoord.x * cellSize, 0, cellCoord.y * cellSize);
        List<GameObject> objectsInCell = new List<GameObject>();

        // Limiter à une seule structure par cellule
        if (asteroidChance < 0.7f) // 70% de chance d'apparition d'un astéroïde
        {
            TryInstantiateVariant(asteroidData.variants, cellCenter, asteroidParent, objectsInCell, cellSeed);
        } else if (wreckChance < 0.34f) // 10% de chance pour une épave
          {
            TryInstantiateVariant(wreckData.variants, cellCenter, wreckParent, objectsInCell, cellSeed);
        } else if (shopChance < 0.26f) // 2% de chance pour un magasin
          {
            TryInstantiateObject(shopPrefab, cellCenter, shopParent, objectsInCell, cellSeed);
        }

        spawnedObjects[cellCoord] = objectsInCell;
        loadedCells.Add(cellCoord);

        //Debug.Log($"Cell generated at {cellCoord}");
    }

    void TryInstantiateVariant(List<VariantData> variants, Vector3 cellCenter, Transform parent, List<GameObject> objectsInCell, int cellSeed) {
        float randomValue = (float)random.NextDouble();
        float cumulativeProbability = 0f;

        foreach (var variant in variants) {
            cumulativeProbability += variant.probability;
            if (randomValue < cumulativeProbability) {
                TryInstantiateObject(variant.prefab, cellCenter, parent, objectsInCell, cellSeed, variant.ressourceType);
                break;
            }
        }
    }

    void TryInstantiateObject(GameObject prefab, Vector3 cellCenter, Transform parent, List<GameObject> objectsInCell, int cellSeed) {
        Vector3 spawnPosition = GetRandomPositionInCell(cellCenter, cellSeed);
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
        objectsInCell.Add(obj);
    }

    void TryInstantiateObject(GameObject prefab, Vector3 cellCenter, Transform parent, List<GameObject> objectsInCell, int cellSeed, TypeRessource ressourceType) {
        Vector3 spawnPosition = GetRandomPositionInCell(cellCenter, cellSeed);
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
        objectsInCell.Add(obj);

        // Assigner une ressource à la structure si applicable
        Structure structure = obj.GetComponent<Structure>();
        if (structure != null) {
            if (structure.isAsteroid) {
                ChoseQuantityAsteroid(structure, ressourceType);
            } else {
                ChoseQuantityEpave(structure, ressourceType);
            }
        }
    }
    
    private void ChoseQuantityAsteroid(Structure structure, TypeRessource ressourceType) {
        int ramdomValue = Random.Range(0, 101);
        Transform structureTransform = structure.transform;
        int returnRessourceValue;
        switch (ressourceType) {    
            case TypeRessource.Cuivre:
                returnRessourceValue = ramdomValue <= 90 ? Random.Range(0, 51) : 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * Mathf.Max(0.4f, returnRessourceValue / 25.0f);
                break;
            case TypeRessource.Argent:
                returnRessourceValue = ramdomValue <= 80 ? Random.Range(0, 51) : 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * Mathf.Max(0.6f, returnRessourceValue / 25.0f);
                break;
            case TypeRessource.Or:
                returnRessourceValue = ramdomValue <= 60 ? Random.Range(0, 26) : 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * Mathf.Max(0.7f, returnRessourceValue / 12.5f);
                break;
            case TypeRessource.Platine:
                returnRessourceValue = ramdomValue <= 40 ? Random.Range(0, 11) : 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * Mathf.Max(0.8f, returnRessourceValue / 5.0f);
                break;
            default:
                returnRessourceValue = 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * Random.Range(1f, 2f);
                break;
        }
        
        structure.ressource = new Ressource(ressourceType, returnRessourceValue);
    }
    
    private void ChoseQuantityEpave(Structure structure, TypeRessource ressourceType) {
        int ramdomValue = Random.Range(0, 101);
        int returnRessourceValue;
        switch (ressourceType) {    
            case TypeRessource.Cuivre:
                returnRessourceValue = ramdomValue <= 90 ? Random.Range(0, 51) : 0;
                break;
            case TypeRessource.Argent:
                returnRessourceValue = ramdomValue <= 80 ? Random.Range(0, 51) : 0;
                break;
            case TypeRessource.Or:
                returnRessourceValue = ramdomValue <= 60 ? Random.Range(0, 26) : 0;
                break;
            case TypeRessource.Platine:
                returnRessourceValue = ramdomValue <= 40 ? Random.Range(0, 11) : 0;
                break;
            case TypeRessource.NoyauEnergie:
                returnRessourceValue = ramdomValue <= 3 ? 2 : 1;
                break;
            default:
                returnRessourceValue = 0;
                break;
        }
        
        structure.ressource = new Ressource(ressourceType, returnRessourceValue);
    }

    Vector3 GetRandomPositionInCell(Vector3 cellCenter, int cellSeed) {
        System.Random positionRandom = new System.Random(cellSeed);
        Vector3 randomOffset = new Vector3((float)positionRandom.NextDouble() * cellSize - cellSize / 2, 0, (float)positionRandom.NextDouble() * cellSize - cellSize / 2);
        return cellCenter + randomOffset;
    }

    void DestroyCell(Vector2Int cellCoord) {
        if (spawnedObjects.ContainsKey(cellCoord)) {
            foreach (GameObject obj in spawnedObjects[cellCoord]) {
                if (obj != null) Destroy(obj);
            }
            spawnedObjects.Remove(cellCoord);
        }

        //Debug.Log($"Cell destroyed at {cellCoord}");
    }

    Vector2Int GetCellCoordinates(Vector3 position) {
        return new Vector2Int(Mathf.FloorToInt(position.x / cellSize), Mathf.FloorToInt(position.z / cellSize));
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
