using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour {
    public static ShopManager Instance;
    private ShopOption currentOption;
    private bool freeReset = true;

    [SerializeField] private GameObject[] prefabsTurrets;
    [SerializeField] private GameObject[] prefabsConstructions;

    [SerializeField] private Transform conteneurConstructions;
    [SerializeField] private GameObject prefabConstructionItem;
    [SerializeField] private Transform zoneForPrefab;
    [SerializeField] private GameObject shopPanel;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        DisplayShop(false);
    }

    private void DisplayShop(bool value) {
        if (value && freeReset) {
            ResetConstructions();
            freeReset = false;
        }

        if (value) {
            RestoreConstructionsVisibility();
            foreach (Transform child in conteneurConstructions) {
                child.GetComponent<ShopCardUI>().HideSubPanel();
            }
        }

        shopPanel.SetActive(value);
    }
    
    public void FreeReset() {
        freeReset = true;

        if (currentOption == ShopOption.PurchaseConstruction) {
            DisplayShop(true);
        }
    }
    
    public void ResetConstructions() {
        // Supprimer toutes les constructions affichées
        foreach (Transform child in conteneurConstructions) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in zoneForPrefab) {
            Destroy(child.gameObject);
        }
        
        ChooseThreeConstructions();
    }

    private void ChooseThreeConstructions() {
        SelectRandomConstruction(prefabsTurrets, -1);
        SelectRandomConstruction(prefabsConstructions, 0);
        SelectRandomConstruction(prefabsConstructions, 1);
    }

    private void SelectRandomConstruction(GameObject[] gameObjects, int index) {
        Construction[] constructions = new Construction[gameObjects.Length];
        int totalWeight = 0;
        for (int i = 0; i < gameObjects.Length; i++) {
            constructions[i] = gameObjects[i].GetComponent<Construction>();
            totalWeight += constructions[i].GetProbability();
        }

        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        for (int i = 0; i < gameObjects.Length; i++) {
            cumulativeWeight += constructions[i].GetProbability();
            if (randomValue < cumulativeWeight) {
                ChooseRarity(gameObjects[i], index);
                return;
            }
        }
        
        throw new InvalidOperationException("Erreur au niveau de la liste pondérée");
    }

    private void ChooseRarity(GameObject prefab, int index) {
        Array values = Enum.GetValues(typeof(RarityConstruction));
        int[] weights = values.Cast<RarityConstruction>().Select(r => (int)r).ToArray();
        int totalWeight = weights.Sum();
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (RarityConstruction rarity in values) {
            cumulativeWeight += (int)rarity;
            if (randomValue < cumulativeWeight) {
                CreateObject(prefab, rarity, index);
                return;
            }
        }

        throw new InvalidOperationException("Erreur au niveau du choix de la rareté");
    }
    
    private void CreateObject(GameObject prefab, RarityConstruction rarity, int index) {
        GameObject go = Instantiate(prefab, zoneForPrefab);
        Construction construction = go.GetComponent<Construction>();
        construction.SetRarity(rarity);
        AfficherConstruction(construction, index);
    }

    private void AfficherConstruction(Construction construction, int index) {
        // Instancier le prefab de l'élément UI
        GameObject constructionItem = Instantiate(prefabConstructionItem, conteneurConstructions);
        constructionItem.GetComponent<ShopCardUI>().Initialisation(construction, index);
    }

    public void DimOtherConstructions(GameObject activeConstruction) {
        foreach (Transform child in conteneurConstructions) {
            if (child.gameObject != activeConstruction) {
                child.GetComponent<ShopCardUI>().HideSubPanel();
                child.GetComponent<CanvasGroup>().alpha = 0.25f; // Make less visible
            }
        }

        // Move the active construction to the bottom of the hierarchy
        activeConstruction.transform.SetAsLastSibling();
    }

    public void RestoreConstructionsVisibility() {
        foreach (Transform child in conteneurConstructions) {
            CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
            if (canvasGroup) {
                canvasGroup.alpha = 1f; // Restore visibility
            }
        }
    }

    public bool IsShopOpen() {
        return currentOption != ShopOption.None;
    }

    public void OpenShop() {
        ChangeShopOption(ShopOption.PurchaseConstruction);
    }

    public void CloseShop() {
        ChangeShopOption(ShopOption.None);
    }

    public void ChangeShopOption(ShopOption option) {
        if (currentOption == option) return;

        BuyHoverUI.Instance.HideHoverUI();
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
