using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuyHoverUI : MonoBehaviour {
    public static BuyHoverUI Instance;

    [SerializeField] private GameObject prefabElement;
    private Dictionary<Ressource, TextMeshProUGUI> quantityTexts;

    /// <summary>
    /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
    /// </summary>
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        quantityTexts = new Dictionary<Ressource, TextMeshProUGUI>();
        HideHoverUI();
    }

    /// <summary>
    /// Affiche l'interface de survol pour l'achat d'un objet.
    /// </summary>
    /// <param name="objectName">Le nom de l'objet à acheter.</param>
    /// <param name="price">La liste des ressources nécessaires pour l'achat.</param>
    public void ShowHoverUI(string objectName, List<Ressource> price) {
        quantityTexts = UpdateUI(transform, objectName, price);
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Masque l'interface de survol.
    /// </summary>
    public void HideHoverUI() {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Affiche l'interface de construction pour un objet.
    /// </summary>
    /// <param name="parent">Le parent de l'interface de construction.</param>
    /// <param name="objectName">Le nom de l'objet à construire.</param>
    /// <param name="price">La liste des ressources nécessaires pour la construction.</param>
    /// <returns>Un dictionnaire des ressources et des textes de quantité associés.</returns>
    public Dictionary<Ressource, TextMeshProUGUI> ShowConstructionUI(Transform parent, string objectName, List<Ressource> price) {
        return UpdateUI(parent, objectName, price);
    }

    /// <summary>
    /// Met à jour l'interface utilisateur avec les informations de l'objet et des ressources nécessaires.
    /// </summary>
    /// <param name="parent">Le parent de l'interface utilisateur.</param>
    /// <param name="objectName">Le nom de l'objet.</param>
    /// <param name="price">La liste des ressources nécessaires.</param>
    /// <returns>Un dictionnaire des ressources et des textes de quantité associés.</returns>
    private Dictionary<Ressource, TextMeshProUGUI> UpdateUI(Transform parent, string objectName, List<Ressource> price) {
        TextMeshProUGUI nameText = parent.Find("Name").GetComponent<TextMeshProUGUI>();
        nameText.text = parent == transform ? "Acheter " + objectName + " ?" : objectName;

        Transform priceContainer = parent.Find("PriceContainer");
        foreach (Transform child in priceContainer) {
            Destroy(child.gameObject);
        }
        Dictionary<Ressource, TextMeshProUGUI> texts = new Dictionary<Ressource, TextMeshProUGUI>();
        foreach (Ressource ressource in price) {
            GameObject element = Instantiate(prefabElement, priceContainer);

            element.GetComponentInChildren<Image>().sprite = ressource.GetSprite();

            TextMeshProUGUI quantity = element.GetComponentInChildren<TextMeshProUGUI>();
            quantity.text = ressource.quantite.ToString();
            texts.Add(ressource, quantity);
        }
        UpdateColorUI(texts);
        return texts;
    }

    /// <summary>
    /// Met à jour les couleurs de l'interface utilisateur en fonction des ressources disponibles.
    /// </summary>
    /// <param name="texts">Le dictionnaire des ressources et des textes de quantité associés.</param>
    public void UpdateColorUI(Dictionary<Ressource, TextMeshProUGUI> texts = null) {
        texts ??= quantityTexts;

        foreach (KeyValuePair<Ressource, TextMeshProUGUI> quantity in texts) {
            quantity.Value.color = Inventory.Instance.HaveEnoughRessource(quantity.Key) ? Color.white : Color.red;
        }
    }
}
