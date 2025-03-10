using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public abstract class Construction
{
    public string Nom { get; protected set; }
    public Dictionary<Ressource.TypeRessource, int> CoutRessources { get; protected set; }
    public bool EstDebloque { get; set; }

    // Constructeur
    protected Construction(string nom, Dictionary<Ressource.TypeRessource, int> coutRessources, bool estDebloque = false)
    {
        Nom = nom;
        CoutRessources = coutRessources;
        EstDebloque = estDebloque;
    }

     public override string ToString()
    {
        string couts = string.Join("\n", CoutRessources.Select(kv => $"{kv.Key}: {kv.Value}"));
        return $"Nom: {Nom}\nCoût:\n{couts}\nDébloqué: {EstDebloque}";
    }
}