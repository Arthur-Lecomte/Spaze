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

    private void Start() {
        // Appeler la fonction pour sélectionner et afficher les constructions
        SelectionnerEtAfficherConstructions();
    }

        private void SelectionnerEtAfficherConstructions() {
         // Assigner une rareté aléatoire à chaque prefab (Avec 25% de chance pour chaque rareté)
        foreach (GameObject prefab in prefabsConstructions) {
            Construction construction = prefab.GetComponent<Construction>();
            if (construction != null) {
                construction.AssignRandomRarity();
            }
        }


        // Filtrer les prefabs pour ne garder que les tourelles
        GameObject[] tourellePrefabs = prefabsConstructions.Where(prefab => prefab.GetComponent<Turret>() != null).ToArray();
        // Sélectionner une tourelle aléatoire parmi les tourelles filtrées
        GameObject tourellePrefab = SelectPrefabWithRarityAndProbability(tourellePrefabs);

        // Sélectionner deux autres constructions aléatoires parmi toutes les constructions
        GameObject[] autresPrefabs = prefabsConstructions.Where(prefab => prefab != tourellePrefab).ToArray();
        

        GameObject selectedPrefab1 = SelectPrefabWithRarityAndProbability(autresPrefabs);
        GameObject selectedPrefab2 = SelectPrefabWithRarityAndProbability(autresPrefabs);
        
        // Afficher les trois constructions dans l'interface utilisateur
        AfficherConstruction(tourellePrefab);
        AfficherConstruction(selectedPrefab1);
        AfficherConstruction(selectedPrefab2);
    }

    private GameObject SelectPrefabWithRarityAndProbability(GameObject[] prefabs) {
        // Créer une liste pondérée en fonction de la rareté et de la probabilité
        List<GameObject> weightedList = new List<GameObject>();
        foreach (GameObject prefab in prefabs) {
            Construction construction = prefab.GetComponent<Construction>();
            if (construction != null) {
                int weight = GetWeight(construction.rarity, construction.probability);
                //Debug.Log($"Prefab: {prefab.name}, Rarity: {construction.rarity}, Probability: {construction.probability}, Weight: {weight}");
                for (int i = 0; i < weight; i++) {
                    weightedList.Add(prefab);
                }
            }
        }

        if (weightedList.Count == 0) {
            Debug.LogError("La liste pondérée est vide. Aucun prefab n'a été sélectionné.");
            return null;
        }


        // Sélectionner un prefab aléatoire dans la liste pondérée
        int randomIndex = Random.Range(0, weightedList.Count);
        return Instantiate(weightedList[randomIndex], zoneForPrefab, true);
    }

    private int GetWeight(RarityConstruction rarity, float probability) {
    int baseWeight = 1; // Poids de base pour les calculs
    int rarityWeight = (int)rarity; // Utiliser la valeur de l'énumération directement
    return Mathf.RoundToInt(baseWeight * probability * rarityWeight);
    }

    private void AfficherConstruction(GameObject prefab) {
        // Instancier le prefab de l'élément UI
        GameObject constructionItem = Instantiate(prefabConstructionItem, conteneurConstructions);

        // Configurer l'élément UI
        TextMeshProUGUI nom = constructionItem.transform.Find("Zone").Find("Name")?.GetComponent<TextMeshProUGUI>();
        Image sprite = constructionItem.transform.Find("Zone").Find("Sprite")?.GetComponent<Image>();
        TextMeshProUGUI rarity = constructionItem.transform.Find("Zone").Find("Rarity Zone")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI cuivre = constructionItem.transform.Find("Zone").Find("Cout Cuivre")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI argent = constructionItem.transform.Find("Zone").Find("Cout Argent")?.GetComponent<TextMeshProUGUI>();
        Button acheterButton = constructionItem.transform.Find("Zone").Find("AcheterButton")?.GetComponent<Button>();
        
        Construction construction = prefab.GetComponent<Construction>();
        nom.text = construction.nom;
        sprite.sprite = construction.GetImage();
        rarity.text = Enum.GetName(typeof(RarityConstruction), construction.rarity);
        cuivre.text = construction.CoutRessourcesDeBase[0].quantite.ToString();
        argent.text = construction.CoutRessourcesDeBase[1].quantite.ToString();
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
            //vaisseau.inventory.ShowConstructions();
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
}
