using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationStructure : MonoBehaviour {
    public static GenerationStructure Instance { get; private set; }

    [Header("Ressource Data")]
    [SerializeField] private AsteroidData asteroidData;
    [SerializeField] private WreckData wreckData;
    [SerializeField] private GameObject shopPrefab;
    [SerializeField] private GameObject asteroidVidePrefab;

    [Header("Generation Settings")]
    [SerializeField] private int cellSize = 50; // Taille des cellules de la grille
    [SerializeField] private int spawnRadius = 3; // Nombre de cellules autour du joueur à charger
    [SerializeField] private int seed = 0; // Seed aléatoire pour générer les structures

    private Dictionary<Vector2Int, GameObject> loadedCells = new();
    private Dictionary<Vector2Int, CellState> cellStates = new();

    private Transform asteroidParent;
    private Transform wreckParent;
    private Transform shopParent;

    private System.Random random;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        if (seed == 0) {
            seed = Random.Range(0, 100000);
        }
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
                Vector2Int cellCoord = new(playerCell.x + x, playerCell.y + y);
                newLoadedCells.Add(cellCoord);

                if (!loadedCells.ContainsKey(cellCoord)) {
                    StartCoroutine(GenerateCell(cellCoord));
                }
            }
        }

        // Supprimer les anciennes cellules qui ne sont plus dans la zone de spawn
        HashSet<Vector2Int> cellsToRemove = new HashSet<Vector2Int>(loadedCells.Keys);
        cellsToRemove.ExceptWith(newLoadedCells);

        foreach (Vector2Int cell in cellsToRemove) {
            DestroyCell(cell);
        }

        // Mettre à jour la liste des cellules chargées
        foreach (var cell in cellsToRemove) {
            loadedCells.Remove(cell);
        }
    }

    IEnumerator GenerateCell(Vector2Int cellCoord) {
        if (cellCoord == Vector2Int.zero) yield break; // Ne pas générer de structure à la position (0, 0)

        if (cellStates.TryGetValue(cellCoord, out var cellState)) {
            // Restaurer l'état de la structure dans la cellule
            GameObject prefab;
            if (cellState.IsAsteroid && cellState.Ressource.quantite == 0) {
                prefab = asteroidVidePrefab;
            } else {
                prefab = cellState.IsAsteroid ? asteroidData.variants.Find(v => v.ressourceType == cellState.Ressource.type).prefab : wreckData.variants.Find(v => v.ressourceType == cellState.Ressource.type).prefab;
            }
            GameObject obj = Instantiate(prefab, cellState.Position, Quaternion.identity, cellState.IsAsteroid ? asteroidParent : wreckParent);
            obj.transform.localScale = cellState.Scale;
            Structure structure = obj.GetComponent<Structure>();
            structure.SetRessource(cellState.Ressource);
            if (cellState.IsMined || cellState.IsScavenged) {
                Destroy(obj);
            }
            loadedCells[cellCoord] = obj;
        } else {
            int cellSeed = seed + cellCoord.x * 73856093 + cellCoord.y * 19349663; // Seed unique par cellule
            random = new System.Random(cellSeed);

            float asteroidChance = (float)random.NextDouble();
            float wreckChance = (float)random.NextDouble();
            float shopChance = (float)random.NextDouble();

            Vector3 cellCenter = new(cellCoord.x * cellSize, 0, cellCoord.y * cellSize);
            GameObject obj = null;

            // Limiter à une seule structure par cellule
            if (asteroidChance < 0.7f) // 70% de chance d'apparition d'un astéroïde
            {
                obj = TryInstantiateVariant(asteroidData.variants, cellCenter, asteroidParent, cellSeed);
            } else if (wreckChance < 0.34f) // 10% de chance pour une épave
            {
                obj = TryInstantiateVariant(wreckData.variants, cellCenter, wreckParent, cellSeed);
            } else if (shopChance < 0.26f) // 2% de chance pour un magasin
            {
                obj = TryInstantiateObject(shopPrefab, cellCenter, shopParent, cellSeed);
            }

            if (obj != null) {
                loadedCells[cellCoord] = obj;
            }
        }

        yield return null;
    }

    GameObject TryInstantiateVariant(List<VariantData> variants, Vector3 cellCenter, Transform parent, int cellSeed) {
        float randomValue = (float)random.NextDouble();
        float cumulativeProbability = 0f;

        foreach (var variant in variants) {
            cumulativeProbability += variant.probability;
            if (randomValue < cumulativeProbability) {
                return TryInstantiateObject(variant.prefab, cellCenter, parent, cellSeed, variant.ressourceType);
            }
        }
        return null;
    }

    GameObject TryInstantiateObject(GameObject prefab, Vector3 cellCenter, Transform parent, int cellSeed) {
        Vector3 spawnPosition = GetRandomPositionInCell(cellCenter, cellSeed);
        return Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
    }

    GameObject TryInstantiateObject(GameObject prefab, Vector3 cellCenter, Transform parent, int cellSeed, TypeRessource ressourceType) {
        Vector3 spawnPosition = GetRandomPositionInCell(cellCenter, cellSeed);
        GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity, parent);

        // Assigner une ressource à la structure si applicable
        if (obj.TryGetComponent<Structure>(out var structure)) {
            if (structure.isAsteroid) {
                ChoseQuantityAsteroid(structure, ressourceType, cellSeed);
            } else {
                ChoseQuantityEpave(structure, ressourceType, cellSeed);
            }
        }
        return obj;
    }

    public void SaveCellState(GameObject obj) {
        Vector2Int cellCoord = GetCellCoordinates(obj.transform.position);
        // Sauvegarder l'état de la structure
        if (obj.TryGetComponent<Structure>(out var structure)) {
            cellStates[cellCoord] = new CellState {
                Position = obj.transform.position,
                Scale = obj.transform.localScale,
                IsAsteroid = structure.isAsteroid,
                Ressource = structure.ressource,
                IsMined = structure.isAsteroid && structure.ressource.quantite == 0,
                IsScavenged = !structure.isAsteroid && structure.ressource.quantite == 0
            };
        }
    }

    private void ChoseQuantityAsteroid(Structure structure, TypeRessource ressourceType, int cellSeed) {
        System.Random random = new(cellSeed);
        int randomValue = random.Next(0, 101);
        Transform structureTransform = structure.transform;
        int returnRessourceValue;
        switch (ressourceType) {
            case TypeRessource.Cuivre:
                returnRessourceValue = randomValue <= 90 ? random.Next(0, 51) : 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * Mathf.Max(0.4f, returnRessourceValue / 25.0f);
                break;
            case TypeRessource.Argent:
                returnRessourceValue = randomValue <= 80 ? random.Next(0, 51) : 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * Mathf.Max(0.6f, returnRessourceValue / 25.0f);
                break;
            case TypeRessource.Or:
                returnRessourceValue = randomValue <= 60 ? random.Next(0, 26) : 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * Mathf.Max(0.7f, returnRessourceValue / 12.5f);
                break;
            case TypeRessource.Platine:
                returnRessourceValue = randomValue <= 40 ? random.Next(0, 11) : 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * Mathf.Max(0.8f, returnRessourceValue / 5.0f);
                break;
            default:
                returnRessourceValue = 0;
                structureTransform.localScale = new Vector3(1, 1, 1) * random.Next(1, 2);
                break;
        }

        structure.SetRessource(new Ressource(ressourceType, returnRessourceValue));
    }

    private void ChoseQuantityEpave(Structure structure, TypeRessource ressourceType, int cellSeed) {
        System.Random random = new(cellSeed);
        int randomValue = random.Next(0, 101);
        var returnRessourceValue = ressourceType switch {
            TypeRessource.Cuivre => randomValue <= 90 ? random.Next(0, 51) : 0,
            TypeRessource.Argent => randomValue <= 80 ? random.Next(0, 51) : 0,
            TypeRessource.Or => randomValue <= 60 ? random.Next(0, 26) : 0,
            TypeRessource.Platine => randomValue <= 40 ? random.Next(0, 11) : 0,
            TypeRessource.NoyauEnergie => randomValue <= 3 ? 2 : 1,
            _ => 0,
        };
        structure.ressource = new Ressource(ressourceType, returnRessourceValue);
    }

    Vector3 GetRandomPositionInCell(Vector3 cellCenter, int cellSeed) {
        System.Random positionRandom = new System.Random(cellSeed);
        Vector3 randomOffset = new((float)positionRandom.NextDouble() * cellSize - cellSize / 2, 0, (float)positionRandom.NextDouble() * cellSize - cellSize / 2);
        return cellCenter + randomOffset;
    }

    void DestroyCell(Vector2Int cellCoord) {
        if (loadedCells.TryGetValue(cellCoord, out var obj)) {
            if (obj != null) {
                Destroy(obj);
            }
        }
    }

    Vector2Int GetCellCoordinates(Vector3 position) {
        return new Vector2Int(Mathf.FloorToInt(position.x / cellSize), Mathf.FloorToInt(position.z / cellSize));
    }

    void OnDrawGizmos() {
        if (loadedCells == null) return;

        // Dessiner les cellules chargées
        Gizmos.color = Color.green;
        foreach (Vector2Int cell in loadedCells.Keys) {
            Vector3 cellCenter = new Vector3(cell.x * cellSize, 0, cell.y * cellSize);
            Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 1, cellSize));
        }

        // Dessiner les cellules sauvegardées
        Gizmos.color = Color.blue;
        foreach (Vector2Int cell in cellStates.Keys) {
            if (!loadedCells.ContainsKey(cell)) {
                Vector3 cellCenter = new Vector3(cell.x * cellSize, 0, cell.y * cellSize);
                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 1, cellSize));
            }
        }
    }
}
