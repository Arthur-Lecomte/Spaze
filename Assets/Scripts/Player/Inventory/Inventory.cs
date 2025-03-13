using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    public static Inventory Instance; //DEBUG!!! Pour des tests rapides. À enlever et corriger plus tard

    private Dictionary<TypeRessource, int> ressources;
    private List<VaisseauModule> modules;
    private List<Construction> constructionsInventory;
    [SerializeField] private int size = 2;
    [SerializeField] private Transform parentInInventory;

    public GameObject prefabTest;
    public GameObject prefabTest2;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        ressources = new Dictionary<TypeRessource, int>();
        foreach(TypeRessource type in System.Enum.GetValues(typeof(TypeRessource))) {
            ressources.Add(type, 0);
        }

        modules = new List<VaisseauModule>(GetComponentsInChildren<VaisseauModule>(true));
        constructionsInventory = new List<Construction>(size);
        for(int i = 0; i < size; i++) {
            constructionsInventory.Add(null);
        }
    }

    private void Start() {
        InventoryUI.Instance.ChangeNumberSlots(size);
        /*
        GameObject go = Instantiate(prefabTest);
        AddConstruction(go.GetComponent<Construction>());

        go = Instantiate(prefabTest2);
        AddConstruction(go.GetComponent<Construction>());
        */
    }

    public void AddRessource(TypeRessource type, int quantity) {
        ressources[type] += quantity;
        RessourcesUI.Instance.UpdateUI();
    }

    public bool HaveEnoughRessources(Dictionary<TypeRessource, int> allRessources) {
        foreach((TypeRessource type, int quantity) in allRessources) {
            if(!HaveEnoughRessource(type, quantity)) {
                return false;
            }
        }
        return true;
    }

    public bool HaveEnoughRessource(TypeRessource type, int quantity) {
        return ressources[type] >= quantity;
    }

    // Retire une quantité de ressource spécifiée
    public void RemoveRessource(TypeRessource type, int quantity) {
        ressources[type] -= quantity;
        RessourcesUI.Instance.UpdateUI();
    }

    public int GetRessource(TypeRessource type) {
        return ressources[type];
    }

    public void AddInventorySlotsSize() {
        size += 1;
        constructionsInventory.Add(null);
        InventoryUI.Instance.ChangeNumberSlots(size);
    }

    public void SetInventorySlots(int index, Construction construction) {
        constructionsInventory[index] = construction;
        if(construction) {
            construction.transform.SetParent(parentInInventory);
        }
        InventoryUI.Instance.UpdateSlotImage(index, constructionsInventory[index]);
    }

    public Construction GetInventorySlots(int index) {
        return constructionsInventory[index];
    }

    public void MoveInventorySlot(int index, int newIndex) {
        (constructionsInventory[index], constructionsInventory[newIndex]) = (constructionsInventory[newIndex], constructionsInventory[index]);
        InventoryUI.Instance.UpdateSlotImage(index, constructionsInventory[index]);
        InventoryUI.Instance.UpdateSlotImage(newIndex, constructionsInventory[newIndex]);
    }

    public Construction RemoveInventorySlots(int index) {
        Construction construction = constructionsInventory[index];
        constructionsInventory[index] = null;
        InventoryUI.Instance.UpdateSlotImage(index, null);
        return construction;
    }

    // Méthode pour ajouter une construction au vaisseau
    public bool AddConstruction(Construction newConstruction) {
        VaisseauModule libre = null;

        // On vérifie si on peut améliorer une construction posée sur le vaisseau
        foreach(VaisseauModule module in modules) {
            if(module.IsActivate()) {
                Construction construction = module.GetConstruction();
                if(construction && construction.IsSameConstruction(newConstruction)) {
                    module.GetConstruction().Upgrade();
                    return true;
                }

                // On garde en mémoire un module libre au cas où on ne peut rien améliorer
                if(!libre && module.IsEmpty()) {
                    libre = module;
                }
            }
        }

        // On vérifie si on peut améliorer une construction présente dans l'inventaire
        foreach(Construction construction in constructionsInventory) {
            if(construction && construction.IsSameConstruction(construction)) {
                construction.Upgrade();
                return true;
            }
        }

        // Si on a un module libre, on lui ajoute la construction
        if(libre) {
            libre.SetConstruction(newConstruction);
            return true;
        }

        // Si on a de la place dans l'inventaire, on ajoute la construction
        for(int i = 0; i < constructionsInventory.Count; i++) {
            if(!constructionsInventory[i]) {
                constructionsInventory[i] = newConstruction;
                newConstruction.transform.SetParent(parentInInventory);
                InventoryUI.Instance.UpdateSlotImage(i, newConstruction);
                return true;
            }
        }

        // On ne peut rien faire de cette construction, on annule l'achat
        return false;
    }
}
