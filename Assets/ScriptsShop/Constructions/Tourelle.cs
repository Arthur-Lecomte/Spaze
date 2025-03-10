using System;

[Serializable]
public class Tourelle : Construction
{

    private string nom;
    private int coutRessources;
    private bool estDebloque;
    private int degats;
    private float attackSpeed;
    private int niveau;
    private int niveauMax;

    // Constructeur
    public Tourelle(string nom, int coutRessources, int degats, float attackSpeed, int niveau, int niveauMax, bool estDebloque = false)

    {
        this.nom = nom;
        this.coutRessources = coutRessources;
        this.degats = degats;
        this.attackSpeed = attackSpeed;
        this.niveau = niveau;
        this.niveauMax = niveauMax;
        this.estDebloque = estDebloque;
    }

    // Propriétés spécifiques à la tourelle
    public string Nom => nom;
    public int CoutRessources => coutRessources;

    public int Degats => degats;
    public float AttackSpeed => attackSpeed;
    public int Niveau => niveau;
    public int NiveauMax => niveauMax;
    public bool EstDebloque
    {
        get => estDebloque;
        set => estDebloque = value;
    }

    public override string ToString()
    {
        return $"Nom: {nom}, Coût: {coutRessources}, Dégâts: {degats}, Vitesse d'attaque: {attackSpeed}, Niveau: {niveau}/{niveauMax}, Débloqué: {estDebloque}";
    }
}