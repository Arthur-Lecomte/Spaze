using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Construction : MonoBehaviour {
    [SerializeField] protected string nom;
    [SerializeField] protected string description;
    protected RarityConstruction rarity;
    [SerializeField] protected TypeConstruction type;     
    [SerializeField] protected int niveauMax;
    protected int Niveau;
    [SerializeField] public List<RessourceCout> CoutRessourcesDeBase = new List<RessourceCout>();
    [SerializeField] protected float probability;
    [SerializeField] protected Sprite image;
    private Transform constructionTransform;

    public Dictionary<TypeRessource, int> GetCoutRessources() {
        return CoutRessourcesDeBase.ToDictionary(cout => cout.typeRessource, cout => cout.quantite);
    }
    
    private void Awake() {
        constructionTransform = transform.GetChild(0);
    }
    
    public Sprite GetImage() {
        return image;
    }
    
    public RarityConstruction GetRarity() {
        return rarity;
    }

    public float GetProbability() {
        return probability;
    }

    public string GetNom() {
        return nom;
    }
    
    
    public void SetChildOf(Transform parent) {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        float taille = 1f + (Niveau - 1) * (2f - 1f) / (niveauMax - 1);
        constructionTransform.localPosition = new Vector3(0, -0.5f * (taille - 1f), 0);
        constructionTransform.localScale = new Vector3(taille, taille, taille);
    }

    public bool IsSameConstruction(Construction c) {
        if (c.GetType() == GetType() && c.Niveau == Niveau && c.rarity == rarity) {
            return true;
        }
        return false;
    }

    public bool Upgrade() {
        if (Niveau < niveauMax) {
            Niveau++;
            PerformUpgrade();
            return true;
        }
        return false;
    }


    
    public abstract void PerformUpgrade();

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
    Extracteur, //Augmente la quantité de ressources récoltées
    Radar, //Augmente la portée du radar (ennemis et/ou ressources)
}
/* Notes à voir avec l'équipe:
- Choisir le mode d'attaque des armes (ennemi le plus proche, le plus faible, avec le plus de PV...)
- Pouvoir sélectionner des ennemis pour les ciblés en priorité
*/
