using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RessourcesUI : MonoBehaviour {
    [SerializeField] private Transform conteneurRessources;
    [SerializeField] private GameObject prefabElementRessource;

    // Dictionnaire pour stocker les références aux éléments UI de chaque ressource
    private Dictionary<TypeRessource, TextMeshProUGUI> texteRessources = new();

    public static RessourcesUI Instance { get; private set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    void Start() {
        CreateElementsUI();
        UpdateUI();
    }
    
    public void AddRessource(int quantity) {
        Inventory.Instance.AddRessource(TypeRessource.Cuivre, quantity);
        Inventory.Instance.AddRessource(TypeRessource.Argent, quantity);
        Inventory.Instance.AddRessource(TypeRessource.Or, quantity);
        Inventory.Instance.AddRessource(TypeRessource.Platine, quantity);
        Inventory.Instance.AddRessource(TypeRessource.NoyauEnergie, quantity);
    }

    private void CreateElementsUI() {
        // Créer un élément UI pour chaque type de ressource
        foreach(TypeRessource type in System.Enum.GetValues(typeof(TypeRessource))) {
            GameObject elementRessource = Instantiate(prefabElementRessource, conteneurRessources);
            TextMeshProUGUI texteElement = elementRessource.GetComponentInChildren<TextMeshProUGUI>();
            elementRessource.GetComponentInChildren<Image>().sprite = Ressource.GetRessourceSprite(type);
            texteRessources.Add(type, texteElement);
        }
    }

    public void UpdateUI() {
        // Mettre à jour le texte de quantité pour chaque ressource
        foreach(var res in texteRessources) {
            TypeRessource type = res.Key;
            TextMeshProUGUI texteElement = res.Value;
            int quantity = Inventory.Instance.GetRessource(type);
            texteElement.text = quantity.ToString();

            // colorer les ressources dont la quantité est 0
            if(quantity == 0) {
                texteElement.color = Color.red;
            } else {
                texteElement.color = Color.white;
            }
        }
    }
}
