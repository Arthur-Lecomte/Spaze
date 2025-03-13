using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Construction : MonoBehaviour {
    [SerializeField] public string nom;
    [SerializeField] protected string description;
    [SerializeField] public RarityConstruction rarity;
    [SerializeField] protected int niveauMax;
    protected int Niveau;
    [SerializeField] public List<RessourceCout> CoutRessourcesDeBase = new List<RessourceCout>();
    [SerializeField] public float probability;
    [SerializeField] protected Sprite image;

    public Dictionary<TypeRessource, int> GetCoutRessources() {
        return CoutRessourcesDeBase.ToDictionary(cout => cout.typeRessource, cout => cout.quantite);
    }

    public Sprite GetImage() {
        return image;
    }

    public bool IsSameConstruction(Construction c) {
        return false; // Vérifier la class, le niveau et la rareté
    }

    public abstract void Upgrade();

    public void AssignRandomRarity() {
        int roll = Random.Range(0, 100);
        if(roll < 25) {
            rarity = RarityConstruction.Common;
        } else if(roll < 50) {
            rarity = RarityConstruction.Rare;
        } else if(roll < 75) {
            rarity = RarityConstruction.Epic;
        } else {
            rarity = RarityConstruction.Legendary;
        }
    }
}

public enum RarityConstruction {
    Common = 50,
    Rare = 30,
    Epic = 15,
    Legendary = 5,
}

[Serializable]
public struct RessourceCout {
    public TypeRessource typeRessource;
    public int quantite;
}

public enum TypeConstruction {
    Sniper, //Longue porté à cible unique
    Assault, //Dégâts de zone à moyenne portée
    Shotgun, //Puissants dégâts de zone à courte portée
    Laser, //Puissants dégâts en ligne droite (très long rechargement)
    Shield, //Augmente le bouclier du joueur
    RegenerationShield, //Augmente la régénération du bouclier du joueur
    Speed, //Augmente la vitesse de déplacement du joueur
    Slower, //Ralenti les ennemis proches
    Extraction, //Augmente la quantité de ressources récoltées
    Radar, //Augmente la portée du radar (ennemis et/ou ressources)
}
/* Notes à voir avec l'équipe:
- Choisir le mode d'attaque des armes (ennemi le plus proche, le plus faible, avec le plus de PV...)
- Pouvoir sélectionner des ennemis pour les ciblés en priorité
*/
