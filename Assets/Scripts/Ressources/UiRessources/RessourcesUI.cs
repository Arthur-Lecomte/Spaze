using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RessourcesUI : MonoBehaviour {
    public static RessourcesUI Instance;
    
    [SerializeField] private Vaisseau vaisseau;
    [SerializeField] private Transform conteneurRessources;
    [SerializeField] private GameObject prefabElementRessource;

    // Dictionnaire pour stocker les références aux éléments UI de chaque ressource
    private Dictionary<TypeRessource, TextMeshProUGUI> texteRessources = new Dictionary<TypeRessource, TextMeshProUGUI>();

    void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    void Start() {
        CreateElementsUI();
        UpdateUI();
    }
    
    public void AddRessourceCuivre(int quantity) {
        Inventory.Instance.AddRessource(TypeRessource.Cuivre, quantity);
        UpdateUI();
    }

    private void CreateElementsUI() {
        // Créer un élément UI pour chaque type de ressource
        foreach(TypeRessource type in System.Enum.GetValues(typeof(TypeRessource))) {
            GameObject elementRessource = Instantiate(prefabElementRessource, conteneurRessources);
            TextMeshProUGUI texteElement = elementRessource.transform.Find("TexteElement").GetComponent<TextMeshProUGUI>();
            texteElement.text = type + ": 0";
            texteRessources.Add(type, texteElement);
        }
    }

    public void UpdateUI() {
        // Mettre à jour le texte de quantité pour chaque ressource
        foreach(var res in texteRessources) {
            TypeRessource type = res.Key;
            TextMeshProUGUI texteElement = res.Value;
            int quantity = Inventory.Instance.GetRessource(type);
            texteElement.text = type + ": " + quantity;

            // colorer les ressources dont la quantité est 0
            if(quantity == 0) {
                texteElement.color = Color.red;
            } else {
                texteElement.color = Color.white;
            }
        }
    }
}
