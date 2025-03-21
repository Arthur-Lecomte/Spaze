using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopUI : MonoBehaviour {
    public static ShopUI Instance;
    private ShopOption currentOption;
    private bool freeReset = true;

    [SerializeField] private GameObject[] prefabsConstructions;
    
    [SerializeField] private Transform conteneurConstructions;
    [SerializeField] private GameObject prefabConstructionItem;
    [SerializeField] private Transform zoneForPrefab;
    [SerializeField] private GameObject shopPanel;

    private Dictionary<GameObject, bool> panelStates = new Dictionary<GameObject, bool>(); // État ouvert/fermé
    private Dictionary<GameObject, bool> panelTransitions = new Dictionary<GameObject, bool>(); // Transition en cours

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        DisplayShop(false);
    }
    
    public void DisplayShop(bool value) {
        if (value && freeReset) {
            ResetConstructions();
            freeReset = false;
        }
        
        shopPanel.SetActive(value);
    }
    
    public void FreeReset() {
        freeReset = true;
        
        if (currentOption == ShopOption.PurchaseConstruction) {
            DisplayShop(true);
        }
    }

    private void SelectionnerEtAfficherConstructions() {
        // Liste temporaire pour stocker les instances des prefabs
        List<GameObject> instancesConstructions = new List<GameObject>();

        // Méthode locale pour recréer les instances
        List<GameObject> RecreateInstances() {
            List<GameObject> newInstances = new List<GameObject>();
            foreach (GameObject prefab in prefabsConstructions) {
                GameObject instance = Instantiate(prefab, zoneForPrefab, true);
                newInstances.Add(instance);

                // Assigner une rareté aléatoire à chaque instance
                Construction construction = instance.GetComponent<Construction>();
                if (construction != null) {
                    construction.AssignRandomRarity();
                }
            }
            return newInstances;
        }

        // Créer les premières instances
        instancesConstructions = RecreateInstances();

        // Filtrer les instances pour ne garder que les tourelles
        GameObject[] tourelleInstances = instancesConstructions
            .Where(instance => instance.GetComponent<Turret>() != null)
            .ToArray();

        // Sélectionner une tourelle aléatoire parmi les tourelles filtrées
        GameObject selectedTourelle = SelectPrefabWithRarityAndProbability(tourelleInstances);

        // Recréer les instances après la sélection
        instancesConstructions = RecreateInstances();

        // Sélectionner deux autres constructions aléatoires parmi les nouvelles instances
        GameObject[] autresInstances = instancesConstructions
            .Where(instance => instance != selectedTourelle)
            .ToArray();

        GameObject selectedInstance1 = SelectPrefabWithRarityAndProbability(autresInstances);

        // Recréer les instances une dernière fois pour garantir l'unicité
        instancesConstructions = RecreateInstances();
        autresInstances = instancesConstructions
            .Where(instance => instance != selectedTourelle && instance != selectedInstance1)
            .ToArray();

        GameObject selectedInstance2 = SelectPrefabWithRarityAndProbability(autresInstances);

        // Récupérer les composants Construction des objets instanciés
        Construction constructionTourelle = selectedTourelle.GetComponent<Construction>();
        Construction construction1 = selectedInstance1.GetComponent<Construction>();
        Construction construction2 = selectedInstance2.GetComponent<Construction>();

        constructionTourelle.AdjustStatsByRarity();
        construction1.AdjustStatsByRarity();
        construction2.AdjustStatsByRarity();

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
        constructionItem.GetComponent<SubPanelHandler>().Initialisation(construction, index);
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

    public void DimOtherConstructions(GameObject activeConstruction) {
        foreach (Transform child in conteneurConstructions) {
            if (child.gameObject != activeConstruction) {
                CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
                if (canvasGroup == null) {
                    canvasGroup = child.gameObject.AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha = 0.02f; // Make less visible
            }
        }

        // Move the active construction to the bottom of the hierarchy
        activeConstruction.transform.SetAsLastSibling();
    }

    public void RestoreConstructionsVisibility() {
        foreach (Transform child in conteneurConstructions) {
            CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
            if (canvasGroup != null) {
                canvasGroup.alpha = 1f; // Restore visibility
            }
        }
    }
    
    public bool IsShopOpen() {
        return currentOption != ShopOption.None;
    }
    
    public void CloseShop() {
        ChangeShopOption(ShopOption.None);
    }

    public void ChangeShopOption(ShopOption option) {
        if (currentOption == option) return;
        
        DisplayShop(false);
        UpgradeManager.Instance.DisplayUpgrade(false);
        InventoryUI.Instance.DisplayInventory(false);

        currentOption = option;
        switch (option) {
            case ShopOption.PurchaseConstruction:
                DisplayShop(true);
                break;
            case ShopOption.UpgradeShip:
                UpgradeManager.Instance.DisplayUpgrade(true);
                break;
            case ShopOption.ManageInventory:
                InventoryUI.Instance.DisplayInventory(true);
                break;
        }
    }
}

public enum ShopOption {
    None,
    PurchaseConstruction,
    UpgradeShip,
    ManageInventory
}
