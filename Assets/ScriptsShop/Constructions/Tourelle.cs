using System;
using System.Collections.Generic;

[Serializable]
public class Tourelle : Construction
{
    private int degats;
    private float attackSpeed;
    private int niveau;
    private int niveauMax;

    // Constructeur
    public Tourelle(string nom, Dictionary<Ressource.TypeRessource, int> coutRessources, int degats, float attackSpeed, int niveau, int niveauMax, bool estDebloque = false)
        : base(nom, coutRessources, estDebloque)
    {
        this.degats = degats;
        this.attackSpeed = attackSpeed;
        this.niveau = niveau;
        this.niveauMax = niveauMax;
    }

    // Propriétés spécifiques à la tourelle
    public int Degats => degats;
    public float AttackSpeed => attackSpeed;
    public int Niveau => niveau;
    public int NiveauMax => niveauMax;

    public override string ToString()
    {
        return base.ToString() + $"\nDégâts: {degats}\nVitesse d'attaque: {attackSpeed}\nNiveau: {niveau}/{niveauMax}";
    }
}