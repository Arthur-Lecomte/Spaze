using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    // Ressources
    //[SerializeField] private List<Ressource> ressources = new List<Ressource>();
    private Dictionary<Ressource.TypeRessource, Ressource> ressourcesDict = new Dictionary<Ressource.TypeRessource, Ressource>();

    // Constructions
    [SerializeField] private List<Construction> constructions = new List<Construction>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitialiserRessources();
            InitialiserConstructions();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Initialiser le dictionnaire avec toutes les ressources possibles 
    private void InitialiserRessources()
    {
        foreach (Ressource.TypeRessource type in System.Enum.GetValues(typeof(Ressource.TypeRessource)))
        {
            Ressource nouvelleRessource = new Ressource(type, 100);
            //ressources.Add(nouvelleRessource);
            ressourcesDict.Add(type, nouvelleRessource);
        }
    }

    // Initialiser la liste des constructions
    private void InitialiserConstructions()
    {
        constructions.Add(new Tourelle("Tourelle de défense légère", new Dictionary<Ressource.TypeRessource, int> { { Ressource.TypeRessource.Cuivre, 200 } }, 50, 1.0f, 1, 5, true));
        constructions.Add(new Tourelle("Tourelle de défense lourde", new Dictionary<Ressource.TypeRessource, int> { { Ressource.TypeRessource.Argent, 300 } }, 100, 2.0f, 1, 5, false));
        //constructions.Add(new Bouclier("Bouclier énergétique", new Dictionary<Ressource.TypeRessource, int> { { Ressource.TypeRessource.Or, 300 } }, 100, 5.0f));
        //constructions.Add(new Extracteur("Extracteur de ressources", new Dictionary<Ressource.TypeRessource, int> { { Ressource.TypeRessource.Argent, 150 } }, 10, 2.0f));
        //constructions.Add(new Soutien("Module de soutien", new Dictionary<Ressource.TypeRessource, int> { { Ressource.TypeRessource.Platine, 250 } }, 20, 10.0f));
        
        // Ajoutez d'autres constructions ici
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