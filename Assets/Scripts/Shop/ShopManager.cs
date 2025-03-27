using System;
using System.Collections.Generic;
using System.Linq;
using SmallHedge.SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopManager : MonoBehaviour {
    public static ShopManager Instance;
    
    private ShopOption currentOption;
    private ShopOption memoryOption;
    private Shop currentShop;
    private bool forceUpdate;

    [SerializeField] private GameObject[] prefabsTurrets;
    [SerializeField] private GameObject[] prefabsConstructions;
    
    [SerializeField] private GameObject[] buttons;
    
    [SerializeField] private Transform conteneurConstructions;
    private readonly List<ShopCardUI> shopCards = new List<ShopCardUI>();
    [SerializeField] private GameObject prefabConstructionItem;
    [SerializeField] private Transform zoneForPrefab;
    [SerializeField] private GameObject shopPanel;
    
    private Dictionary<Shop, Construction[]> shopConstructions = new Dictionary<Shop, Construction[]>();
    
    private Dictionary<RarityConstruction, int> rarityWeight = new Dictionary<RarityConstruction, int> {
        {RarityConstruction.Common, 50},
        {RarityConstruction.Rare, 30},
        {RarityConstruction.Epic, 15},
        {RarityConstruction.Legendary, 5}
    };

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    private void Start() {
        //Force l'interface à se désactiver
        currentOption = ShopOption.PurchaseConstruction;
        memoryOption = ShopOption.PurchaseConstruction;
        ChangeShopOption(0, null);
        
        for(int index = 0; index < 3; index++) {
            GameObject constructionItem = Instantiate(prefabConstructionItem, conteneurConstructions);
            shopCards.Add(constructionItem.GetComponent<ShopCardUI>());
            shopCards[index].Create(index);
        }
    }

    private void DisplayShop(bool value, Shop shop) {
        if (value) {
            if (shop != currentShop || forceUpdate) {
                if (!shopConstructions.TryGetValue(shop, out Construction[] constructions)) {
                    constructions = ChooseThreeConstructions();
                    shopConstructions[shop] = constructions;
                }
                
                for (int index = 0; index < shopCards.Count; index++) {
                    if (constructions[index]) {
                        shopCards[index].Initialisation(constructions[index]);
                        shopCards[index].gameObject.SetActive(true);
                    } else {
                        shopCards[index].gameObject.SetActive(false);
                    }
                }
            }
            
            foreach (ShopCardUI child in shopCards) {
                child.HideStoppedCoroutine();
            }
            RestoreConstructionsVisibility(shop);
        }

        shopPanel.SetActive(value);
    }

    public void IsBuy(int index) {
        shopConstructions[currentShop][index] = null;
    }
    
    public void NewWave() {
        shopConstructions.Clear();
        foreach (Transform child in zoneForPrefab) {
            Destroy(child.gameObject);
        }

        forceUpdate = true;
        if (currentOption == ShopOption.PurchaseConstruction) {
            DisplayShop(true, currentShop);
        }
    }

    private Construction[] ChooseThreeConstructions() {
        Construction[] constructions = new Construction[3];
        
        constructions[0] = SelectRandomConstruction(prefabsTurrets);
        constructions[1] = SelectRandomConstruction(prefabsConstructions);
        constructions[2] = SelectRandomConstruction(prefabsConstructions);

        return constructions;
    }

    private Construction SelectRandomConstruction(GameObject[] gameObjects) {
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
                return ChooseRarity(gameObjects[i]);
            }
        }
        
        throw new InvalidOperationException("Erreur au niveau de la liste pondérée");
    }

    private Construction ChooseRarity(GameObject prefab) {
        int totalWeight = rarityWeight.Values.Sum();
        int randomValue = Random.Range(0, totalWeight);
        int cumulativeWeight = 0;

        foreach (var rarity in rarityWeight) {
            cumulativeWeight += rarity.Value;
            if (randomValue < cumulativeWeight) {
                return CreateObject(prefab, rarity.Key);
            }
        }

        throw new InvalidOperationException("Erreur au niveau du choix de la rareté");
    }
    
    private Construction CreateObject(GameObject prefab, RarityConstruction rarity) {
        GameObject go = Instantiate(prefab, zoneForPrefab);
        Construction construction = go.GetComponent<Construction>();
        construction.Initialisation(rarity);
        return construction;
    }

    public void DimOtherConstructions(ShopCardUI activeConstruction) {
        for (int index = 0; index < shopCards.Count; index++) {
            if (shopConstructions[currentShop][index] && shopCards[index] != activeConstruction) {
                shopCards[index].SetCanvasGroup(0.25f);
                shopCards[index].HideSubPanel();
            }
        }

        // Move the active construction to the bottom of the hierarchy
        activeConstruction.transform.SetAsLastSibling();
    }

    public void RestoreConstructionsVisibility(Shop shop) {
        shop ??= currentShop; // Si aucun shop n'est donné, utilisé le shop actuel
        
        for (int index = 0; index < shopCards.Count; index++) {
            if (shopConstructions[shop][index]) {
                shopCards[index].SetCanvasGroup(1f);
            }
        }
    }

    public bool IsShopOpen() {
        return currentOption != ShopOption.None;
    }

    public void ChangeShopOptionButton(int option) {
        ChangeShopOption(option, currentShop);
        SoundManager.PlaySound(SoundType.CLICK);
    }

    public void ChangeShopOption(int option, Shop shop) {
        if (option == 0 && currentShop != shop) return;

        ShopOption shopOption;
        if (currentShop != shop) {
            shopOption = ShopOption.PurchaseConstruction;
        } else {
            shopOption = option == -1 ? memoryOption : (ShopOption)option;
            if (currentOption == shopOption) return;
        }
        
        currentOption = shopOption;
        memoryOption = option != 0 ? shopOption : memoryOption;

        gameObject.SetActive(option != 0);
        ChangeAffichageButton(buttons[(int)shopOption]);
        DisplayShop(false, currentShop);
        UpgradeManager.Instance.DisplayUpgrade(false);
        InventoryUI.Instance.DisplayInventory(false);
        
        switch (shopOption) {
            case ShopOption.PurchaseConstruction:
                DisplayShop(true, shop);
                break;
            case ShopOption.UpgradeShip:
                UpgradeManager.Instance.DisplayUpgrade(true);
                break;
            case ShopOption.ManageInventory:
                InventoryUI.Instance.DisplayInventory(true);
                break;
        }

        currentShop = shop;
    }
    
    private void ChangeAffichageButton(GameObject button) {
        Color visible = new Color(1f, 1f, 1f, 1f);
        Color transparent = new Color(1f, 1f, 1f, 0.25f);
        foreach(GameObject b in buttons) {
            Color color = b == button ? transparent : visible;
            b.GetComponent<Image>().color = color;
            b.GetComponentInChildren<TextMeshProUGUI>().color = color;
        }
    }
}

public enum ShopOption {
    None = 0,
    PurchaseConstruction = 1,
    UpgradeShip = 2,
    ManageInventory = 3
}
