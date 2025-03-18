using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour {
    [SerializeField] private GameObject[] prefabsConstructions;

    [SerializeField] private Vaisseau vaisseau;
    [SerializeField] private Transform conteneurConstructions;
    [SerializeField] private GameObject prefabConstructionItem;
    [SerializeField] private Transform zoneForPrefab;
    [SerializeField] private GameObject shopPanel; 

    private bool isShopActive=true;

    private void Start() {
        // Appeler la fonction pour sélectionner et afficher les constructions
        SelectionnerEtAfficherConstructions();
        ToggleShop();
    }

    public void ToggleShop() {
        isShopActive = !isShopActive;
        shopPanel.SetActive(isShopActive); 
    }

    private void SelectionnerEtAfficherConstructions() {
    // Liste temporaire pour stocker les instances des prefabs
    List<GameObject> instancesConstructions = new List<GameObject>();

    // Instancier tous les prefabs et les ajouter à la liste
    foreach (GameObject prefab in prefabsConstructions) {
        GameObject instance = Instantiate(prefab, zoneForPrefab, true);
        instancesConstructions.Add(instance);

        // Assigner une rareté aléatoire à chaque instance
        Construction construction = instance.GetComponent<Construction>();
        if (construction != null) {
            construction.AssignRandomRarity();
        }
    }

    // Filtrer les instances pour ne garder que les tourelles
    GameObject[] tourelleInstances = instancesConstructions
        .Where(instance => instance.GetComponent<Turret>() != null)
        .ToArray();

    // Sélectionner une tourelle aléatoire parmi les tourelles filtrées
    GameObject selectedTourelle = SelectPrefabWithRarityAndProbability(tourelleInstances);

    // Sélectionner deux autres constructions aléatoires parmi toutes les instances
    GameObject[] autresInstances = instancesConstructions
        .Where(instance => instance != selectedTourelle)
        .ToArray();

    GameObject selectedInstance1 = SelectPrefabWithRarityAndProbability(autresInstances);
    GameObject selectedInstance2 = SelectPrefabWithRarityAndProbability(autresInstances);

    // Récupérer les composants Construction des objets instanciés
    Construction constructionTourelle = selectedTourelle.GetComponent<Construction>();
    Construction construction1 = selectedInstance1.GetComponent<Construction>();
    Construction construction2 = selectedInstance2.GetComponent<Construction>();

    constructionTourelle.AdjustStatsByRarity();
    if (selectedInstance1 == selectedInstance2) {
        construction1.AdjustStatsByRarity();
    } else {
        construction1.AdjustStatsByRarity();
        construction2.AdjustStatsByRarity();
    }


    // Afficher les trois constructions dans l'interface utilisateur
    AfficherConstruction(constructionTourelle, -1);
    AfficherConstruction(construction1, 0);
    AfficherConstruction(construction2, 1);
    }

    private GameObject SelectPrefabWithRarityAndProbability(GameObject[] instances) {
    // Créer une liste pondérée en fonction de la rareté et de la probabilité
    List<GameObject> weightedList = new List<GameObject>();
    foreach (GameObject instance in instances) {
        Construction construction = instance.GetComponent<Construction>();
        if (construction != null) {
            int weight = GetWeight(construction.GetRarity(), construction.GetProbability());
            //Debug.Log($"Instance: {instance.name}, Rarity: {construction.GetRarity()}, Probability: {construction.GetProbability()}, Weight: {weight}");
            for (int i = 0; i < weight; i++) {
                weightedList.Add(instance);
            }
        }
    }

    if (weightedList.Count == 0) {
        Debug.LogError("La liste pondérée est vide. Aucun prefab n'a été sélectionné.");
        return null;
    }

    // Sélectionner un prefab aléatoire dans la liste pondérée
    int randomIndex = Random.Range(0, weightedList.Count);
    return weightedList[randomIndex];
    }

    private int GetWeight(RarityConstruction rarity, float probability) {
        int baseWeight = 1; // Poids de base pour les calculs
        int rarityWeight = (int)rarity; // Utiliser la valeur de l'énumération directement
        return Mathf.RoundToInt(baseWeight * probability * rarityWeight);
    }

    private void AfficherConstruction(Construction construction, int index) {
        // Instancier le prefab de l'élément UI
        GameObject constructionItem = Instantiate(prefabConstructionItem, conteneurConstructions);
        constructionItem.transform.localPosition = new Vector3(index * 600, 0, 0);

        // Configurer l'élément UI
        TextMeshProUGUI nom = constructionItem.transform.Find("Zone").Find("Name")?.GetComponent<TextMeshProUGUI>();
        Image sprite = constructionItem.transform.Find("Zone").Find("Sprite")?.GetComponent<Image>();
        TextMeshProUGUI rarity = constructionItem.transform.Find("Zone").Find("Rarity Zone")?.GetComponent<TextMeshProUGUI>();

        // Liste des champs de texte pour les ressources
        TextMeshProUGUI[] resourceTexts = new TextMeshProUGUI[] {
            constructionItem.transform.Find("Zone").Find("Cout Cuivre")?.GetComponent<TextMeshProUGUI>(),
            constructionItem.transform.Find("Zone").Find("Cout Argent")?.GetComponent<TextMeshProUGUI>(),
            constructionItem.transform.Find("Zone").Find("Cout Or")?.GetComponent<TextMeshProUGUI>(),
            constructionItem.transform.Find("Zone").Find("Cout Platine")?.GetComponent<TextMeshProUGUI>(),
            constructionItem.transform.Find("Zone").Find("Cout Poussiere Radioactive")?.GetComponent<TextMeshProUGUI>()
        };

        if (construction != null) {
            nom.text = construction.GetNom();
            sprite.sprite = construction.GetImage();
            rarity.text = Enum.GetName(typeof(RarityConstruction), construction.GetRarity());
            rarity.color = GetColorForRarity(construction.GetRarity());

            // Mettre à jour les coûts des ressources
            for (int i = 0; i < resourceTexts.Length; i++) {
                if (i < construction.CoutRessourcesDeBase.Count) {
                    resourceTexts[i].text = construction.CoutRessourcesDeBase[i].quantite.ToString();
                } else {
                    resourceTexts[i].text = "0"; // Valeur par défaut si aucune ressource
                }
            }

            // Configurer le bouton d'achat
            Button acheterButton = constructionItem.transform.Find("Zone").Find("AcheterButton")?.GetComponent<Button>();
            acheterButton.onClick.AddListener(() => BuyConstruction(construction, constructionItem));
        } else {
            Debug.LogError("AfficherConstruction: Construction component is missing on the prefab.");
        }
    }


    // Méthode pour acheter une construction
    private bool BuyConstruction(Construction construction, GameObject constructionItem) {
        // Vérifiez si le joueur à suffisamment de ressources pour acheter la construction
        bool canBuy = true;
        foreach((TypeRessource type, int quantity) in construction.GetCoutRessources()) {
            if(!vaisseau.inventory.HaveEnoughRessource(type, quantity)) {
                canBuy = false;
                break;
            }
        }

        if(canBuy && vaisseau.inventory.AddConstruction(construction)) {
            // Retirer les ressources nécessaires
            foreach((TypeRessource type, int quantity) in construction.GetCoutRessources()) {
                vaisseau.inventory.RemoveRessource(type, quantity);
            }
            
            constructionItem.SetActive(false);
        }
        return canBuy;
    }

    public void ResetConstructions() {
        // Supprimer toutes les constructions affichées
        foreach (Transform child in conteneurConstructions) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in zoneForPrefab) {
            Destroy(child.gameObject);
        }
        // Réinitialiser les constructions
        SelectionnerEtAfficherConstructions();
    }

    // Méthode pour obtenir la couleur en fonction de la rareté
    private Color GetColorForRarity(RarityConstruction rarity) {
        switch (rarity) {
            case RarityConstruction.Common:
                return Color.white;
            case RarityConstruction.Rare:
                return Color.blue; 
            case RarityConstruction.Epic:
                return new Color(0.5f, 0f, 0.5f); // Violet pour "Epic"
            case RarityConstruction.Legendary:
                return Color.yellow; 
            default:
                return Color.gray; 
        }
    }
}
