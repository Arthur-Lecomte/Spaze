using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RessourcesUI : MonoBehaviour {
    public static RessourcesUI Instance;

    [SerializeField] private Transform conteneurRessources;
    [SerializeField] private GameObject prefabElementRessource;

    // Dictionnaire pour stocker les références aux éléments UI de chaque ressource
    private Dictionary<TypeRessource, TextMeshProUGUI> texteRessources = new();

    public static event Action OnUIUpdated;

    /// <summary>
    /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
    /// </summary>
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Méthode appelée au démarrage. Crée les éléments UI et met à jour l'interface utilisateur.
    /// </summary>
    void Start() {
        CreateElementsUI();
        UpdateUI();
    }

    /// <summary>
    /// Ajoute une quantité spécifiée de ressource du type donné à l'inventaire et met à jour l'interface utilisateur.
    /// </summary>
    /// <param name="ressourceType">Le type de la ressource.</param>
    /// <param name="quantity">La quantité à ajouter.</param>
    public void AddRessourceByType(TypeRessource ressourceType, int quantity) {
        Inventory.Instance.AddRessource(ressourceType, quantity);
        UpdateUI();
    }

    /// <summary>
    /// Ajoute une quantité spécifiée de chaque type de ressource à l'inventaire.
    /// </summary>
    /// <param name="quantity">La quantité à ajouter pour chaque type de ressource.</param>
    public void AddRessource(int quantity) {
        Inventory.Instance.AddRessource(TypeRessource.Cuivre, quantity);
        Inventory.Instance.AddRessource(TypeRessource.Argent, quantity);
        Inventory.Instance.AddRessource(TypeRessource.Or, quantity);
        Inventory.Instance.AddRessource(TypeRessource.Platine, quantity);
        Inventory.Instance.AddRessource(TypeRessource.NoyauEnergie, quantity);
    }

    /// <summary>
    /// Crée les éléments UI pour chaque type de ressource.
    /// </summary>
    private void CreateElementsUI() {
        // Créer un élément UI pour chaque type de ressource
        foreach (TypeRessource type in System.Enum.GetValues(typeof(TypeRessource))) {
            GameObject elementRessource = Instantiate(prefabElementRessource, conteneurRessources);
            TextMeshProUGUI texteElement = elementRessource.GetComponentInChildren<TextMeshProUGUI>();
            elementRessource.GetComponentInChildren<Image>().sprite = Ressource.GetRessourceSprite(type);
            texteRessources.Add(type, texteElement);
        }
    }

    /// <summary>
    /// Met à jour l'interface utilisateur pour refléter les quantités actuelles de ressources dans l'inventaire.
    /// </summary>
    public void UpdateUI() {
        // Mettre à jour le texte de quantité pour chaque ressource
        foreach (var res in texteRessources) {
            TypeRessource type = res.Key;
            TextMeshProUGUI texteElement = res.Value;
            int quantity = Inventory.Instance.GetRessource(type);
            texteElement.text = quantity.ToString();

            // colorer les ressources dont la quantité est 0
            texteElement.color = quantity == 0 ? Color.red : Color.white;
        }
        BuyHoverUI.Instance.UpdateColorUI();
        OnUIUpdated?.Invoke();
    }
}
