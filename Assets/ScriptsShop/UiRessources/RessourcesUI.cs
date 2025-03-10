using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RessourcesUI : MonoBehaviour
{
    [SerializeField] private Vaisseau vaisseau;
    [SerializeField] private Transform conteneurRessources;
    [SerializeField] private GameObject prefabElementRessource;
    
    // Dictionnaire pour stocker les références aux éléments UI de chaque ressource
    private Dictionary<Ressource.TypeRessource, TextMeshProUGUI> texteRessources = 
        new Dictionary<Ressource.TypeRessource, TextMeshProUGUI>();
    
    void Start()
    {
        
        
        
        vaisseau.OnInventaireModifie += MettreAJourUI;
        CreerElementsUI();
        MettreAJourUI();
    }
    
private void CreerElementsUI()
{
    // Supprimer les enfants existants si nécessaire
    foreach (Transform enfant in conteneurRessources)
    {
        Destroy(enfant.gameObject);
    }

    // Créer un élément UI pour chaque type de ressource
    foreach (Ressource.TypeRessource type in System.Enum.GetValues(typeof(Ressource.TypeRessource)))
    {
        GameObject elementRessource = Instantiate(prefabElementRessource, conteneurRessources);

        // Configurer l'élément UI
        TextMeshProUGUI texteElement = elementRessource.transform.Find("TexteElement").GetComponent<TextMeshProUGUI>();

        // Définir le titre initial
        texteElement.text = type.ToString() + ": 0";

        // Enregistrer la référence pour les mises à jour
        texteRessources.Add(type, texteElement);
    }
}

public void MettreAJourUI()
{
    Debug.Log("Mise à jour de l'interface utilisateur des ressources");
    // Mettre à jour le texte de quantité pour chaque ressource
    foreach (var res in texteRessources)
    {
        Ressource.TypeRessource type = res.Key;
        TextMeshProUGUI texteElement = res.Value;

        int quantite = vaisseau.ObtenirQuantite(type);
        texteElement.text = type.ToString() + ": " + quantite.ToString();

        // Optionnel : colorer les ressources dont la quantité est 0
        if (quantite == 0)
        {
            texteElement.color = Color.red;
        }
        else
        {
            texteElement.color = Color.white;
        }
    }
}

}