using System.Collections.Generic;
using UnityEngine;

public class Vaisseau : MonoBehaviour
{
    
    //Ressources
    [SerializeField] private List<Ressource> inventaire = new List<Ressource>();
    private Dictionary<Ressource.TypeRessource, Ressource> ressourcesDict = new Dictionary<Ressource.TypeRessource, Ressource>();
    
    public delegate void ChangementInventaire();
    public event ChangementInventaire OnInventaireModifie;

    void Awake()
    {
        InitialiserInventaire();
    }

    // Initialiser le dictionnaire avec toutes les ressources possibles 
    private void InitialiserInventaire()
    {
        
        foreach (Ressource.TypeRessource type in System.Enum.GetValues(typeof(Ressource.TypeRessource)))
        {
            Ressource nouvelleRessource = new Ressource(type, 100);
            inventaire.Add(nouvelleRessource);
            ressourcesDict.Add(type, nouvelleRessource);
        }
    }

    // Ajoute une quantité de ressource spécifiée
    public void AjouterRessource(Ressource.TypeRessource type, int quantite)
    {
        if (quantite <= 0) return;
        
        Ressource ressource = ressourcesDict[type];
        

        ressource.Quantite += quantite;
        Debug.Log($"Ajout de {quantite} {type}. Nouveau total : {ressource.Quantite}");
        
        // Déclencher l'événement de modification
        OnInventaireModifie?.Invoke();
    }

    // Retire une quantité de ressource spécifiée
    public bool RetirerRessource(Ressource.TypeRessource type, int quantite)
    {
        if (quantite <= 0) return false;
        
        Ressource ressource = ressourcesDict[type];
        
        // Vérifier si on a assez de ressources
        if (ressource.Quantite < quantite)
        {
            Debug.LogWarning($"Impossible de retirer {quantite} {type}. Quantité disponible : {ressource.Quantite}");
            return false;
        }
        
        // Retirer la quantité
        ressource.Quantite -= quantite;
        Debug.Log($"Retrait de {quantite} {type}. Nouveau total : {ressource.Quantite}");
        
        // Déclencher l'événement de modification
        OnInventaireModifie?.Invoke();
        
        return true;
    }

    // Obtient la quantité d'une ressource spécifique
    public int ObtenirQuantite(Ressource.TypeRessource type)
    {
        return ressourcesDict[type].Quantite;
    }

   
}