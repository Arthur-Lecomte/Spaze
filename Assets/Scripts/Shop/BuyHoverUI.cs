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
        UpdateHoverUI(objectName, price);
        gameObject.SetActive(true);
    }

    public void HideHoverUI() {
        gameObject.SetActive(false);
    }

    private void UpdateHoverUI(string objectName, List<Ressource> price) {
        TextMeshProUGUI nameText = transform.Find("Name").GetComponent<TextMeshProUGUI>();
        nameText.text = "Acheter " +objectName + " ?";

        Transform priceContainer = transform.Find("PriceContainer");
        foreach (Transform child in priceContainer) {
            Destroy(child.gameObject);
        }

        foreach (Ressource ressource in price) {
            GameObject element = Instantiate(prefabElement, priceContainer);

            element.GetComponentInChildren<Image>().sprite = ressource.GetSprite();
            element.GetComponentInChildren<TextMeshProUGUI>().text = ressource.quantite.ToString();
        }
    }
}
