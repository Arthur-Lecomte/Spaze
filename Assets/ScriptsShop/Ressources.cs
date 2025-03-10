using System;
using UnityEngine;



[Serializable]
public class Ressource
{
    public enum TypeRessource
    {
        Cuivre,
        Argent,
        Or,
        Platine,
        PoussiereRadioactive
    }

    [SerializeField] private TypeRessource type;
    [SerializeField] private int quantite;

    //Autres propriétés (mining speed etc...)

    // Constructeur
    public Ressource(TypeRessource type, int quantite)
    {
        this.type = type;
        this.quantite = quantite;
    }

    
    
    public int Quantite 
    { 
        get => quantite; 
        set => quantite = Mathf.Max(0, value); // Empêche les quantités négatives
    }

    public override string ToString()
    {
        return $"{type}: {quantite} unités";
    }
}