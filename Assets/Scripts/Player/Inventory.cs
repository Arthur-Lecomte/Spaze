using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {
    private Dictionary<TypeRessource, int> ressources;
    private List<Construction> constructionsInventory;
    [SerializeField] private int size;
    private List<Composant> composants;

    private void Awake() {
        ressources = new Dictionary<TypeRessource, int>();
        foreach(TypeRessource type in System.Enum.GetValues(typeof(TypeRessource))) {
            ressources.Add(type, 0);
        }
        
        composants = new List<Composant>(GetComponentsInChildren<Composant>(true));
    }

    public void AddRessource(TypeRessource type, int quantity) {
        ressources[type] += quantity;
    }
    
    public bool HaveEnoughRessource(TypeRessource type, int quantity) {
        return ressources[type] >= quantity;
    }

    // Retire une quantité de ressource spécifiée
    public void RemoveRessource(TypeRessource type, int quantity) {
        ressources[type] -= quantity;
    }
    
    public int GetRessource(TypeRessource type) {
        return ressources[type];
    }

    // Méthode pour ajouter une construction au vaisseau
    public bool AddConstruction(Construction newConstruction) {
        Composant libre = null;
        
        // On vérifie si on peut améliorer une construction posée sur le vaisseau
        foreach(Composant composant in composants) {
            if(composant.GetConstruction().IsSameConstruction(newConstruction)) {
                composant.GetConstruction().Upgrade();
                return true;
            }
            
            // On garde en mémoire un composant libre au cas où on ne peut rien améliorer
            if(!composant.IsEmpty()) {
                libre = composant;
            }
        }
        
        // On vérifie si on peut améliorer une construction présente dans l'inventaire
        foreach(Construction construction in constructionsInventory) {
            if(construction.IsSameConstruction(construction)) {
                construction.Upgrade();
                return true;
            }
        }

        // Si on a un composant libre, on lui ajoute la construction
        if(libre) {
            libre.SetConstruction(newConstruction);
            return true;
        }
        
        // Si on a de la place dans l'inventaire, on ajoute la construction
        if(constructionsInventory.Count < size) {
            constructionsInventory.Add(newConstruction);
            return true;
        }
        
        // On ne peut rien faire de cette construction, on annule l'achat
        return false;
    }
}