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

    public Transform player;
    private HashSet<Vector2Int> loadedCells = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, List<GameObject>> spawnedObjects = new Dictionary<Vector2Int, List<GameObject>>();

    void Start()
    {
        seed = Random.Range(0, 100000); // Génération d'une seed unique
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
        List<Vector2Int> cellsToRemove = new List<Vector2Int>();

        foreach (Vector2Int cell in loadedCells)
        {
            if (!newLoadedCells.Contains(cell))
            {
                DestroyCell(cell);
                cellsToRemove.Add(cell);
            }
        }

        // Mettre à jour la liste des cellules chargées
        foreach (Vector2Int cell in cellsToRemove)
        {
            loadedCells.Remove(cell);
        }

        loadedCells = newLoadedCells;
    }

    void GenerateCell(Vector2Int cellCoord)
    {
        Random.InitState(seed + cellCoord.x * 73856093 + cellCoord.y * 19349663); // Seed unique par cellule

        float asteroidChance = Random.value;
        float wreckChance = Random.value;
        float shopChance = Random.value;

        Vector3 cellCenter = new Vector3(cellCoord.x * cellSize, 0, cellCoord.y * cellSize);
        List<GameObject> objectsInCell = new List<GameObject>();

        if (asteroidChance < 0.5f) // 50% de chance d'apparition d'un astéroïde
        {
            objectsInCell.Add(Instantiate(asteroidPrefab, cellCenter + Random.insideUnitSphere * (cellSize / 2), Quaternion.identity));
        }
        if (wreckChance < 0.2f) // 20% de chance pour une épave
        {
            objectsInCell.Add(Instantiate(wreckPrefab, cellCenter + Random.insideUnitSphere * (cellSize / 2), Quaternion.identity));
        }
        if (shopChance < 0.1f) // 10% de chance pour un magasin
        {
            objectsInCell.Add(Instantiate(shopPrefab, cellCenter + Random.insideUnitSphere * (cellSize / 2), Quaternion.identity));
        }

        spawnedObjects[cellCoord] = objectsInCell;
        loadedCells.Add(cellCoord);
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
}
