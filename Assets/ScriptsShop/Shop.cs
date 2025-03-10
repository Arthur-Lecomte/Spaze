using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour
{
    [SerializeField] private Vaisseau vaisseau;
    [SerializeField] private Transform conteneurConstructions;
    [SerializeField] private GameObject prefabConstructionItem;

    private List<Construction> constructionsDisponibles;

    void Start()
    {
        InitialiserShop();
    }

    // Initialiser le shop avec les constructions disponibles
    private void InitialiserShop()
    {

        constructionsDisponibles = GameManager.Instance.ObtenirConstructions();

        foreach (var construction in constructionsDisponibles)
        {
            GameObject constructionItem = Instantiate(prefabConstructionItem, conteneurConstructions);

    

            // Configurer l'élément UI
            TextMeshProUGUI constructionText = constructionItem.transform.Find("ConstructionText")?.GetComponent<TextMeshProUGUI>();
            Button acheterButton = constructionItem.transform.Find("AcheterButton")?.GetComponent<Button>();

 
            constructionText.text = construction.ToString();
            acheterButton.onClick.AddListener(() => AcheterConstruction(construction));
        }
    }

    // Méthode pour acheter une construction
    private void AcheterConstruction(Construction construction)
    {
        // Vérifiez si le joueur a suffisamment de ressources pour acheter la construction
        bool peutAcheter = true;
        foreach (var cout in construction.CoutRessources)
        {
            if (vaisseau.ObtenirQuantite(cout.Key) < cout.Value)
            {
                peutAcheter = false;
                break;
            }
        }

        if (peutAcheter)
        {
            // Retirer les ressources nécessaires
            foreach (var cout in construction.CoutRessources)
            {
                vaisseau.RetirerRessource(cout.Key, cout.Value);
            }

            // Ajouter la construction au vaisseau
            vaisseau.AjouterConstruction(construction);

            Debug.Log($"Construction {construction.Nom} achetée !");
        }
        else
        {
            Debug.Log("Pas assez de ressources pour acheter cette construction.");
        }
    }
}