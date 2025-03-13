using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour {
    [SerializeField] private GameObject[] prefabsConstructions;

    [SerializeField] private Vaisseau vaisseau;
    [SerializeField] private Transform conteneurConstructions;
    [SerializeField] private GameObject prefabConstructionItem;

    private void Start() {
        
             // Configurer l'élément UI
            GameObject constructionItem = Instantiate(prefabConstructionItem, conteneurConstructions);
            TextMeshProUGUI constructionText = constructionItem.transform.Find("ConstructionText")?.GetComponent<TextMeshProUGUI>();
            Button acheterButton = constructionItem.transform.Find("AcheterButton")?.GetComponent<Button>();

            int randomIndex = Random.Range(0, prefabsConstructions.Length);
            GameObject selectedPrefab = prefabsConstructions[randomIndex];
            Construction construction = selectedPrefab.GetComponent<Construction>();
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
            Debug.Log("Construction achetée");
            vaisseau.inventory.ShowConstructions();
        }
        return canBuy;
    }
}
