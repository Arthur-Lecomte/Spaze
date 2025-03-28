using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Spaze {
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

        /// <summary>
        /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
        /// </summary>
        void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Méthode appelée au démarrage. Initialise les parents des structures et le générateur aléatoire.
        /// </summary>
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

        /// <summary>
        /// Méthode appelée à chaque frame pour mettre à jour les cellules chargées.
        /// </summary>
        void Update() {
            UpdateLoadedCells();
        }

        /// <summary>
        /// Met à jour les cellules chargées autour du joueur.
        /// </summary>
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

        /// <summary>
        /// Génère une cellule à la position spécifiée.
        /// </summary>
        /// <param name="cellCoord">Les coordonnées de la cellule à générer.</param>
        /// <returns>Un IEnumerator pour la coroutine.</returns>
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
                if (cellState.IsScavenged) {
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
                    obj = TryInstantiateVariant(asteroidData.variants, cellCenter, asteroidParent, cellSeed);
                else if (wreckChance < 0.34f)  // 10% de chance pour une épave
                    obj = TryInstantiateVariant(wreckData.variants, cellCenter, wreckParent, cellSeed);
                else if (shopChance < 0.05f)  // 2% de chance pour un magasin
                    obj = TryInstantiateObject(shopPrefab, cellCenter, shopParent, cellSeed);


                if (obj != null)
                    loadedCells[cellCoord] = obj;

            }

            yield return null;
        }

        /// <summary>
        /// Tente d'instancier une variante de structure à la position spécifiée.
        /// </summary>
        /// <param name="variants">La liste des variantes de structure.</param>
        /// <param name="cellCenter">Le centre de la cellule.</param>
        /// <param name="parent">Le parent de la structure.</param>
        /// <param name="cellSeed">La seed de la cellule.</param>
        /// <returns>Le GameObject instancié ou null si aucune variante n'a été instanciée.</returns>
        GameObject TryInstantiateVariant(List<VariantData> variants, Vector3 cellCenter, Transform parent, int cellSeed) {
            float randomValue = (float)random.NextDouble();
            float cumulativeProbability = 0f;

            foreach (var variant in variants) {
                cumulativeProbability += variant.probability;
                if (randomValue < cumulativeProbability)
                    return TryInstantiateObject(variant.prefab, cellCenter, parent, cellSeed, variant.ressourceType);
            }
            return null;
        }

        /// <summary>
        /// Tente d'instancier un objet à la position spécifiée.
        /// </summary>
        /// <param name="prefab">Le prefab de l'objet à instancier.</param>
        /// <param name="cellCenter">Le centre de la cellule.</param>
        /// <param name="parent">Le parent de l'objet.</param>
        /// <param name="cellSeed">La seed de la cellule.</param>
        /// <returns>Le GameObject instancié.</returns>
        GameObject TryInstantiateObject(GameObject prefab, Vector3 cellCenter, Transform parent, int cellSeed) {
            // Appliquer un décalage aléatoire
            Vector3 spawnPosition = cellCenter + GetRandomOffset(cellSeed);
            return Instantiate(prefab, spawnPosition, Quaternion.identity, parent);
        }

        /// <summary>
        /// Tente d'instancier un objet à la position spécifiée avec un type de ressource.
        /// </summary>
        /// <param name="prefab">Le prefab de l'objet à instancier.</param>
        /// <param name="cellCenter">Le centre de la cellule.</param>
        /// <param name="parent">Le parent de l'objet.</param>
        /// <param name="cellSeed">La seed de la cellule.</param>
        /// <param name="ressourceType">Le type de ressource de l'objet.</param>
        /// <returns>Le GameObject instancié.</returns>
        GameObject TryInstantiateObject(GameObject prefab, Vector3 cellCenter, Transform parent, int cellSeed, TypeRessource ressourceType) {
            // Appliquer un décalage aléatoire
            Vector3 spawnPosition = cellCenter + GetRandomOffset(cellSeed);
            GameObject obj = Instantiate(prefab, spawnPosition, Quaternion.identity, parent);

            // Assigner une ressource à la structure si applicable
            if (obj.TryGetComponent<Structure>(out var structure)) {
                if (structure.isAsteroid) {
                    ChooseQuantityAsteroid(structure, ressourceType, cellSeed);
                } else {
                    ChooseQuantityEpave(structure, ressourceType, cellSeed);
                }
            }
            return obj;
        }

        /// <summary>
        /// Obtient un décalage aléatoire pour la position de l'objet.
        /// </summary>
        /// <param name="cellSeed">La seed de la cellule.</param>
        /// <returns>Un Vector3 représentant le décalage aléatoire.</returns>
        Vector3 GetRandomOffset(int cellSeed) {
            System.Random random = new System.Random(cellSeed);
            float maxOffset = cellSize / 3f;
            float offsetX = (float)(random.NextDouble() * 2 - 1) * maxOffset;
            float offsetZ = (float)(random.NextDouble() * 2 - 1) * maxOffset;
            return new Vector3(offsetX, 0, offsetZ);
        }

        /// <summary>
        /// Sauvegarde l'état de la cellule contenant l'objet spécifié.
        /// </summary>
        /// <param name="obj">L'objet dont l'état doit être sauvegardé.</param>
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

        /// <summary>
        /// Choisit la quantité de ressource pour un astéroïde.
        /// </summary>
        /// <param name="structure">La structure de l'astéroïde.</param>
        /// <param name="ressourceType">Le type de ressource de l'astéroïde.</param>
        /// <param name="cellSeed">La seed de la cellule.</param>
        private void ChooseQuantityAsteroid(Structure structure, TypeRessource ressourceType, int cellSeed) {
            System.Random random = new(cellSeed);
            Transform structureTransform = structure.transform;
            int returnRessourceValue = random.Next(0, 100);
            structureTransform.localScale = new Vector3(1, 1, 1) * (1.5f + returnRessourceValue / 50f);

            structure.SetRessource(new Ressource(ressourceType, returnRessourceValue));
        }

        /// <summary>
        /// Choisit la quantité de ressource pour une épave.
        /// </summary>
        /// <param name="structure">La structure de l'épave.</param>
        /// <param name="ressourceType">Le type de ressource de l'épave.</param>
        /// <param name="cellSeed">La seed de la cellule.</param>
        private void ChooseQuantityEpave(Structure structure, TypeRessource ressourceType, int cellSeed) {
            System.Random random = new(cellSeed);
            var returnRessourceValue = ressourceType switch {
                TypeRessource.Cuivre => random.Next(0, 51),
                TypeRessource.Argent => random.Next(0, 51),
                TypeRessource.Or => random.Next(0, 26),
                TypeRessource.Platine => random.Next(0, 11),
                TypeRessource.NoyauEnergie => random.Next(1, 2),
                _ => 0,
            };
            structure.ressource = new Ressource(ressourceType, returnRessourceValue);
        }

        /// <summary>
        /// Détruit la cellule à la position spécifiée.
        /// </summary>
        /// <param name="cellCoord">Les coordonnées de la cellule à détruire.</param>
        void DestroyCell(Vector2Int cellCoord) {
            if (loadedCells.TryGetValue(cellCoord, out var obj)) {
                if (obj != null) {
                    Destroy(obj);
                }
            }
        }

        /// <summary>
        /// Obtient les coordonnées de la cellule contenant la position spécifiée.
        /// </summary>
        /// <param name="position">La position à convertir en coordonnées de cellule.</param>
        /// <returns>Un Vector2Int représentant les coordonnées de la cellule.</returns>
        Vector2Int GetCellCoordinates(Vector3 position) {
            // Calculer le point central de la cellule la plus proche
            float halfCellSize = cellSize / 2f;
            float closestX = Mathf.Round(position.x / cellSize) * cellSize;
            float closestZ = Mathf.Round(position.z / cellSize) * cellSize;

            // Utiliser les coordonnées du point central pour calculer le Vector2Int
            return new Vector2Int(Mathf.FloorToInt((closestX + halfCellSize) / cellSize), Mathf.FloorToInt((closestZ + halfCellSize) / cellSize));
        }

        /// <summary>
        /// Dessine des gizmos pour visualiser les cellules chargées et sauvegardées dans l'éditeur.
        /// </summary>
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
                Vector3 cellCenter = new Vector3(cell.x * cellSize, 0, cell.y * cellSize);
                Gizmos.DrawWireCube(cellCenter, new Vector3(cellSize, 1, cellSize));
            }
        }
    }
}