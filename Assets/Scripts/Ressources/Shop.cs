using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [SerializeField] private GameObject[] prefabsConstructions;

    [SerializeField] private Vaisseau vaisseau;
    [SerializeField] private Transform conteneurConstructions;
    [SerializeField] private GameObject prefabConstructionItem;

    private void Start() {
        // Appeler la fonction pour sélectionner et afficher les constructions
        SelectionnerEtAfficherConstructions();
    }

        private void SelectionnerEtAfficherConstructions() {
        // Filtrer les prefabs pour ne garder que les tourelles
        GameObject[] tourellePrefabs = prefabsConstructions.Where(prefab => prefab.GetComponent<Turret>() != null).ToArray();
        // Sélectionner une tourelle aléatoire parmi les tourelles filtrées
        int randomTourelleIndex = Random.Range(0, tourellePrefabs.Length);
        GameObject tourellePrefab = tourellePrefabs[randomTourelleIndex];

        // Sélectionner deux autres constructions aléatoires parmi toutes les constructions
        GameObject[] autresPrefabs = prefabsConstructions.Where(prefab => prefab != tourellePrefab).ToArray();
        

        int randomIndex1 = Random.Range(0, autresPrefabs.Length);
        int randomIndex2;
        do {
            randomIndex2 = Random.Range(0, autresPrefabs.Length);
        } while (randomIndex2 == randomIndex1);

        GameObject selectedPrefab1 = autresPrefabs[randomIndex1];
        GameObject selectedPrefab2 = autresPrefabs[randomIndex2];

        // Afficher les trois constructions dans l'interface utilisateur
        AfficherConstruction(tourellePrefab);
        AfficherConstruction(selectedPrefab1);
        AfficherConstruction(selectedPrefab2);
    }

    private void AfficherConstruction(GameObject prefab) {
        // Instancier le prefab de l'élément UI
        GameObject constructionItem = Instantiate(prefabConstructionItem, conteneurConstructions);

        // Configurer l'élément UI
        TextMeshProUGUI constructionText = constructionItem.transform.Find("ConstructionText")?.GetComponent<TextMeshProUGUI>();
        Button acheterButton = constructionItem.transform.Find("AcheterButton")?.GetComponent<Button>();
        Construction construction = prefab.GetComponent<Construction>();
        constructionText.text = construction.ToString();
        acheterButton.onClick.AddListener(() => BuyConstruction(construction));
        
    }


    // Méthode pour acheter une construction
    private bool BuyConstruction(Construction construction) {
        // Vérifiez si le joueur à suffisamment de ressources pour acheter la construction
        bool canBuy = true;
        foreach((TypeRessource type, int quantity) in construction.GetCoutRessources()) {
            if(!vaisseau.inventory.HaveEnoughRessource(type, quantity)) {
                canBuy = false;
                break;
            }
        }

        if(canBuy) {
            // Retirer les ressources nécessaires
            foreach((TypeRessource type, int quantity) in construction.GetCoutRessources()) {
                vaisseau.inventory.RemoveRessource(type, quantity);
            }

            // Ajouter la construction au vaisseau
            vaisseau.inventory.AddConstruction(construction);
            //TEMPORAIRE
            vaisseau.inventory.ShowConstructions();
        }
        return canBuy;
    }
}
