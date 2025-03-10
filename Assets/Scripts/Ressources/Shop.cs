using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour {
    [SerializeField] private GameObject[] prefabsConstructions;

    [SerializeField] private Vaisseau vaisseau;
    [SerializeField] private Transform conteneurConstructions;
    [SerializeField] private GameObject prefabConstructionItem;

    private void Start() {
        /* DEBUG!!! A garder pour plus tard

            GameObject constructionItem = Instantiate(prefabConstructionItem, conteneurConstructions);

            // Configurer l'élément UI
            TextMeshProUGUI constructionText = constructionItem.transform.Find("ConstructionText")?.GetComponent<TextMeshProUGUI>();
            Button acheterButton = constructionItem.transform.Find("AcheterButton")?.GetComponent<Button>();

            constructionText.text = construction.ToString();
            acheterButton.onClick.AddListener(() => AcheterConstruction(construction));

        */
    }

    // Méthode pour acheter une construction
    private void BuyConstruction(Construction construction) {
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
        }
    }
}
