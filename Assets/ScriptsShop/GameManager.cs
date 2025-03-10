using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Ressources
    [SerializeField] private List<Ressource> ressources = new List<Ressource>();
    private Dictionary<Ressource.TypeRessource, Ressource> ressourcesDict = new Dictionary<Ressource.TypeRessource, Ressource>();

    // Constructions
    [SerializeField] private List<Construction> constructions = new List<Construction>();

    void Awake()
    {
        
        if (Instance == null)
        {
            Instance = this;
            InitialiserRessourcesVaisseau();
            InitialiserConstructions();
        }

    
    }

    // Initialiser le dictionnaire avec toutes les ressources possibles 
    //A passer de préférence via prefab plus tard
    private void InitialiserRessourcesVaisseau()
    {
        foreach (Ressource.TypeRessource type in System.Enum.GetValues(typeof(Ressource.TypeRessource)))
        {
            Ressource nouvelleRessource = new Ressource(type, 100);
            ressources.Add(nouvelleRessource);
            ressourcesDict.Add(type, nouvelleRessource);
        }
    }

    // Initialiser la liste des constructions
    private void InitialiserConstructions()
    {
        constructions.Add(new Tourelle("Tourelle de défense légère", 200, 50, 1.0f, 1, 5, true));
        constructions.Add(new Tourelle("Tourelle de défense lourde", 400, 100, 2.0f, 1, 5, false));
        // Ajoutez d'autres constructions ici (via prefab de préférence plus tard) 
    }

    // Méthodes pour accéder aux ressources et constructions
    public Ressource ObtenirRessource(Ressource.TypeRessource type)
    {
        if (ressourcesDict.TryGetValue(type, out Ressource ressource))
        {
            return ressource;
        }
        else
        {
            Debug.LogError($"Ressource de type {type} non trouvée.");
            return null;
        }
    }

    public List<Construction> ObtenirConstructions()
    {
        return constructions;
    }
}