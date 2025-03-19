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

        // Charger les cellules autour du joueur
        for (int x = -spawnRadius; x <= spawnRadius; x++)
        {
            for (int y = -spawnRadius; y <= spawnRadius; y++)
            {
                Vector2Int cellCoord = new Vector2Int(playerCell.x + x, playerCell.y + y);

                if (!loadedCells.Contains(cellCoord))
                {
                    GenerateCell(cellCoord);
                    loadedCells.Add(cellCoord);
                }
            }
        }
    }

    void GenerateCell(Vector2Int cellCoord)
    {
        Random.InitState(seed + cellCoord.x * 73856093 + cellCoord.y * 19349663); // Seed unique par cellule

        float asteroidChance = Random.value;
        float wreckChance = Random.value;
        float shopChance = Random.value;

        Vector3 cellCenter = new Vector3(cellCoord.x * cellSize, 0, cellCoord.y * cellSize);

        if (asteroidChance < 0.5f) // 50% de chance d'apparition d'un astéroïde
        {
            Instantiate(asteroidPrefab, cellCenter + Random.insideUnitSphere * (cellSize / 2), Quaternion.identity);
        }
        if (wreckChance < 0.2f) // 20% de chance pour une épave
        {
            Instantiate(wreckPrefab, cellCenter + Random.insideUnitSphere * (cellSize / 2), Quaternion.identity);
        }
        if (shopChance < 0.1f) // 10% de chance pour un magasin
        {
            Instantiate(shopPrefab, cellCenter + Random.insideUnitSphere * (cellSize / 2), Quaternion.identity);
        }
    }

    Vector2Int GetCellCoordinates(Vector3 position)
    {
        return new Vector2Int(Mathf.FloorToInt(position.x / cellSize), Mathf.FloorToInt(position.z / cellSize));
    }
}
