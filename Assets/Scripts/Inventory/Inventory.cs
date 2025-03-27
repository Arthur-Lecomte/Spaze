using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory Instance;

    private List<Ressource> ressources;
    private List<AddonModule> modules;
    private List<Construction> constructionsInventory;
    [SerializeField] private int size = 2;
    private Transform parentInInventory;

    /// <summary>
    /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
    /// </summary>
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        ressources = new List<Ressource>();
        foreach (TypeRessource type in System.Enum.GetValues(typeof(TypeRessource))) {
            ressources.Add(new Ressource(type, 0));
        }

        modules = new List<AddonModule>(GetComponentsInChildren<AddonModule>(true));
        constructionsInventory = new List<Construction>(size);
        for (int i = 0; i < size; i++) {
            constructionsInventory.Add(null);
        }

        GameObject newGameObject = new GameObject("InventoryForPrefab");
        parentInInventory = newGameObject.transform;
        parentInInventory.SetParent(transform);
        newGameObject.SetActive(false);
    }

    /// <summary>
    /// Méthode appelée au démarrage. Initialise l'interface utilisateur de l'inventaire.
    /// </summary>
    private void Start() {
        InventoryUI.Instance.ChangeNumberSlots(size);
    }

    /// <summary>
    /// Ajoute une quantité spécifiée de ressource à l'inventaire.
    /// </summary>
    /// <param name="type">Le type de ressource.</param>
    /// <param name="quantity">La quantité à ajouter.</param>
    public void AddRessource(TypeRessource type, int quantity) {
        Ressource ressource = ressources.FirstOrDefault(r => r.type == type);
        ressource.quantite += quantity;
        RessourcesUI.Instance.UpdateUI();
    }

    /// <summary>
    /// Vérifie si l'inventaire contient suffisamment de ressources pour couvrir le coût spécifié.
    /// </summary>
    /// <param name="cout">La liste des ressources nécessaires.</param>
    /// <returns>True si l'inventaire contient suffisamment de ressources, sinon False.</returns>
    public bool HaveEnoughRessources(List<Ressource> cout) {
        Dictionary<TypeRessource, int> dictionary = ressources.ToDictionary(ressource => ressource.type, ressource => ressource.quantite);
        foreach (Ressource ressource in cout) {
            if (!dictionary.ContainsKey(ressource.type) || dictionary[ressource.type] < ressource.quantite) {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Vérifie si l'inventaire contient suffisamment de la ressource spécifiée.
    /// </summary>
    /// <param name="cout">La ressource nécessaire.</param>
    /// <returns>True si l'inventaire contient suffisamment de la ressource, sinon False.</returns>
    public bool HaveEnoughRessource(Ressource cout) {
        Dictionary<TypeRessource, int> dictionary = ressources.ToDictionary(ressource => ressource.type, ressource => ressource.quantite);
        if (!dictionary.ContainsKey(cout.type) || dictionary[cout.type] < cout.quantite) {
            return false;
        }

        return true;
    }

    /// <summary>
    /// Retire les ressources spécifiées de l'inventaire.
    /// </summary>
    /// <param name="cout">La liste des ressources à retirer.</param>
    public void RemoveRessources(List<Ressource> cout) {
        foreach (Ressource ressource in cout) {
            RemoveRessource(ressource.type, ressource.quantite);
        }
    }

    /// <summary>
    /// Retire une quantité spécifiée de la ressource de l'inventaire.
    /// </summary>
    /// <param name="type">Le type de ressource.</param>
    /// <param name="quantity">La quantité à retirer.</param>
    public void RemoveRessource(TypeRessource type, int quantity) {
        Ressource ressource = ressources.FirstOrDefault(r => r.type == type);
        ressource.quantite -= quantity;
        RessourcesUI.Instance.UpdateUI();
    }

    /// <summary>
    /// Obtient la quantité de la ressource spécifiée dans l'inventaire.
    /// </summary>
    /// <param name="type">Le type de ressource.</param>
    /// <returns>La quantité de la ressource.</returns>
    public int GetRessource(TypeRessource type) {
        return ressources.FirstOrDefault(r => r.type == type).quantite;
    }

    /// <summary>
    /// Obtient la liste des ressources dans l'inventaire.
    /// </summary>
    /// <returns>La liste des ressources.</returns>
    public List<Ressource> GetRessources() {
        return ressources;
    }

    /// <summary>
    /// Augmente la taille de l'inventaire en ajoutant un nouvel emplacement.
    /// </summary>
    public void AddInventorySlotsSize() {
        size += 1;
        constructionsInventory.Add(null);
        InventoryUI.Instance.ChangeNumberSlots(size);
    }

    /// <summary>
    /// Définit la construction à l'emplacement spécifié de l'inventaire.
    /// </summary>
    /// <param name="index">L'index de l'emplacement.</param>
    /// <param name="construction">La construction à ajouter.</param>
    public void SetInventorySlot(int index, Construction construction) {
        constructionsInventory[index] = construction;
        if (construction) {
            construction.SetChildOf(parentInInventory, false);
        }
        InventoryUI.Instance.SetInventorySlot(index, constructionsInventory[index]);
    }

    /// <summary>
    /// Retire la construction de l'emplacement spécifié de l'inventaire.
    /// </summary>
    /// <param name="index">L'index de l'emplacement.</param>
    /// <returns>La construction retirée.</returns>
    public Construction RemoveInventorySlot(int index) {
        Construction construction = constructionsInventory[index];
        constructionsInventory[index] = null;
        InventoryUI.Instance.SetInventorySlot(index, null);
        return construction;
    }

    /// <summary>
    /// Ajoute une construction au vaisseau ou à l'inventaire.
    /// </summary>
    /// <param name="newConstruction">La nouvelle construction à ajouter.</param>
    /// <returns>True si la construction a été ajoutée avec succès, sinon False.</returns>
    public bool AddConstruction(Construction newConstruction) {
        AddonModule libre = null;

        // On vérifie si on peut améliorer une construction posée sur le vaisseau
        foreach (AddonModule module in modules) {
            if (module.IsActivate()) {
                Construction construction = module.GetConstruction();
                if (construction && construction.IsSameConstruction(newConstruction)) {
                    if (module.GetConstruction().Upgrade()) {
                        Destroy(newConstruction.gameObject);
                        return true;
                    }
                }

                // On garde en mémoire un module libre au cas où on ne peut rien améliorer
                if (!libre && module.IsEmpty()) {
                    libre = module;
                }
            }
        }

        // On vérifie si on peut améliorer une construction présente dans l'inventaire
        foreach (Construction construction in constructionsInventory) {
            if (construction && construction.IsSameConstruction(newConstruction)) {
                if (construction.Upgrade()) {
                    Destroy(newConstruction.gameObject);
                    return true;
                }
            }
        }

        // Si on a un module libre, on lui ajoute la construction
        if (libre) {
            libre.onAddConstruction?.Invoke(newConstruction);
            return true;
        }

        // Si on a de la place dans l'inventaire, on ajoute la construction
        for (int i = 0; i < constructionsInventory.Count; i++) {
            if (!constructionsInventory[i]) {
                SetInventorySlot(i, newConstruction);
                return true;
            }
        }

        // On ne peut rien faire de cette construction, on annule l'achat
        return false;
    }
}
