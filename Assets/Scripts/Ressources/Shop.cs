using UnityEngine;

public class Shop : MonoBehaviour {
    private ShopOption currentOption;
    
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
        if(!Inventory.Instance.HaveEnoughRessources(construction.GetCoutRessources())) {
            // Si on peut ajouter la construction au vaisseau
            if (Inventory.Instance.AddConstruction(construction)) {
                // Retirer les ressources nécessaires
                Inventory.Instance.RemoveRessources(construction.GetCoutRessources());
            }
        }
    }
    
    public void ChangeOption(ShopOption option) {
        if (currentOption == option) return;
        //DEBUG!!! à remplir par Baptiste
        // Désactiver le panel des constructions achetables (à Baptiste de remplir)
        UpgradeManager.Instance.DisplayUpgrade(false);
        InventoryUI.Instance.DisplayInventory(false);
        
        currentOption = option;
        switch(option) {
            case ShopOption.PurchaseConstruction:
                // Afficher le panel des constructions achetables (à Baptiste de remplir)
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
