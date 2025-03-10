using System.Collections.Generic;
using UnityEngine;

public class Vaisseau : MonoBehaviour
{
    private Dictionary<Ressource.TypeRessource, Ressource> ressourcesDict;
    private List<Construction> constructionsDebloques;

    public delegate void ChangementInventaire();
    public event ChangementInventaire OnInventaireModifie;

    void Start()
    {
        InitialiserInventaire();
        InitialiserConstructionsDebloquees();
        ObtenirConstructionsDebloquees();

    }

    // Initialiser le dictionnaire avec toutes les ressources possibles 
    private void InitialiserInventaire()
    {
        ressourcesDict = new Dictionary<Ressource.TypeRessource, Ressource>();
        foreach (Ressource.TypeRessource type in System.Enum.GetValues(typeof(Ressource.TypeRessource)))
        {
            Ressource ressource = GameManager.Instance.ObtenirRessource(type);
            if (ressource != null)
            {
                // Créer une nouvelle instance de Ressource pour le Vaisseau
                Ressource nouvelleRessource = new Ressource(type, ressource.Quantite);
                ressourcesDict.Add(type, nouvelleRessource);
            }
            else
            {
                Debug.LogError($"Ressource de type {type} non trouvée dans le GameManager.");
            }
        }
    }

    // Initialiser la liste des constructions à débloquer
    private void InitialiserConstructionsDebloquees()
    {
        constructionsDebloques = new List<Construction>();
        foreach (var construction in GameManager.Instance.ObtenirConstructions())
        {
            if (construction.EstDebloque)
            {
                constructionsDebloques.Add(construction);
            }
        }
    }

    // Ajoute une quantité de ressource spécifiée
    public void AjouterRessource(Ressource.TypeRessource type, int quantite)
    {
        if (ressourcesDict.TryGetValue(type, out Ressource ressource))
        {
            ressource.Quantite += quantite;
            OnInventaireModifie?.Invoke();
        }
    }

    public void AjouterRessourceCuivre(int quantite)
    {
        AjouterRessource(Ressource.TypeRessource.Cuivre, quantite); // Exemple d'ajout de 10 unités de cuivre
    }

    // Retire une quantité de ressource spécifiée
    public void RetirerRessource(Ressource.TypeRessource type, int quantite)
    {
        if (ressourcesDict.TryGetValue(type, out Ressource ressource))
        {
            ressource.Quantite -= quantite;
            OnInventaireModifie?.Invoke();
        }
    }

    // Obtient la quantité d'une ressource spécifique
    public int ObtenirQuantite(Ressource.TypeRessource type)
    {
        if (ressourcesDict.TryGetValue(type, out Ressource ressource))
        {
            return ressource.Quantite;
        }
        else
        {
            Debug.LogError($"Ressource de type {type} non trouvée.");
            return 0;
        }
    }

    // Méthode publique pour accéder à la liste des constructions débloquées
    public List<Construction> ObtenirConstructionsDebloquees()
    {
        foreach (var construction in constructionsDebloques)
        {
            Debug.Log(construction.ToString());
        }
        
        return constructionsDebloques;
    }

    public void AfficherConstructionsDebloquees()
    {
        ObtenirConstructionsDebloquees();
    }
    

    // Méthode pour ajouter une construction au vaisseau
    public void AjouterConstruction(Construction construction)
    {
        constructionsDebloques.Add(construction);
        Debug.Log($"Construction {construction.Nom} ajoutée au vaisseau.");
    }
}