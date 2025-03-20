using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class Construction : MonoBehaviour {
    [SerializeField] protected string nom;
    [SerializeField] protected string description;
    [SerializeField] protected RarityConstruction rarity;
    [SerializeField] protected TypeConstruction type;
    protected int NiveauMax = 5;
    protected int Niveau = 1;
    [SerializeField] protected List<Ressource> coutRessources = new List<Ressource>();
    [SerializeField] protected float probability;
    [SerializeField] protected Sprite image;
    private Transform constructionTransform;

    protected virtual void Awake() {
        constructionTransform = transform.GetChild(0);
    }

    public List<Ressource> GetCoutRessources() {
        return coutRessources;
    }

    public Sprite GetSprite() {
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
        float taille = 1f + (Niveau - 1) * (2f - 1f) / (NiveauMax - 1);
        constructionTransform.localPosition = new Vector3(0, -0.5f * (taille - 1f), 0);
        constructionTransform.localScale = new Vector3(taille, taille, taille);
    }

    public bool IsSameConstruction(Construction c) {
        if (c.type == type && c.Niveau == Niveau && c.rarity == rarity) {
            return true;
        }

        return false;
    }

    public bool Upgrade() {
        //DEBUG!!! Modifier l'UI ?
        if (Niveau < NiveauMax) {
            Niveau++;
            SetChildOf(transform.parent);
            PerformUpgrade();
            return true;
        }

        return false;
    }

    protected abstract void PerformUpgrade();

    public void AssignRandomRarity() {
        int roll = Random.Range(0, 100);
        if (roll < 25) {
            rarity = RarityConstruction.Common;
        } else if (roll < 50) {
            rarity = RarityConstruction.Rare;
        } else if (roll < 75) {
            rarity = RarityConstruction.Epic;
        } else {
            rarity = RarityConstruction.Legendary;
        }
    }

    public virtual void AdjustStatsByRarity() {
        // Ajuster les coûts en fonction de la rareté
        for (int i = 0; i < coutRessources.Count; i++) {
            coutRessources[i] = new Ressource(coutRessources[i].type,
                Mathf.RoundToInt(coutRessources[i].quantite * GetRarityMultiplier()));
        }
    }

    // Méthode pour obtenir un multiplicateur basé sur la rareté
    protected float GetRarityMultiplier() {
        switch (rarity) {
            case RarityConstruction.Common: return 1.0f;
            case RarityConstruction.Rare: return 1.5f;
            case RarityConstruction.Epic: return 2.0f;
            case RarityConstruction.Legendary: return 3.0f;
            default: return 1.0f;
        }
    }

    public virtual Dictionary<string, string> GetStats() {
        return new Dictionary<string, string> {
            { "Description", description },
            { "Level", $"{Niveau} / {NiveauMax}" }
        };
    }
}

public enum RarityConstruction {
    Common = 50,
    Rare = 30,
    Epic = 15,
    Legendary = 5,
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