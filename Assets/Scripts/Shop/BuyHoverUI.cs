using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyHoverUI : MonoBehaviour {
    public static BuyHoverUI Instance;
    
    [SerializeField] private GameObject prefabElement;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        HideHoverUI();
    }

    public void ShowHoverUI(string objectName, List<Ressource> price) {
        UpdateUI(transform, objectName, price);
        gameObject.SetActive(true);
    }

    public void HideHoverUI() {
        gameObject.SetActive(false);
    }
    
    public void ShowConstructionUI(Transform parent, string objectName, List<Ressource> price) {
        UpdateUI(parent, objectName, price);
    }

    private void UpdateUI(Transform parent, string objectName, List<Ressource> price) {
        TextMeshProUGUI nameText = parent.Find("Name").GetComponent<TextMeshProUGUI>();
        nameText.text = parent == transform ? "Acheter " +objectName + " ?" : objectName;

        Transform priceContainer = parent.Find("PriceContainer");
        foreach (Transform child in priceContainer) {
            Destroy(child.gameObject);
        }
        foreach (Ressource ressource in price) {
            GameObject element = Instantiate(prefabElement, priceContainer);

            element.GetComponentInChildren<Image>().sprite = ressource.GetSprite();
            
            TextMeshProUGUI quantity = element.GetComponentInChildren<TextMeshProUGUI>();
            quantity.text = ressource.quantite.ToString();
            quantity.color = Inventory.Instance.HaveEnoughRessource(ressource) ? Color.white : Color.red;
        }
    }
}
