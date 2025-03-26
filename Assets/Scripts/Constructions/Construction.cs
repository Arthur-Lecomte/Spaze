using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Construction : MonoBehaviour {
    private static ConstructionStatsManager stats;

    [SerializeField] protected string nom;
    [SerializeField] protected string description;
    [SerializeField] protected RarityConstruction rarity;
    [SerializeField] protected TypeConstruction type;
    private const int NiveauMax = 5;
    private int niveau = 1;
    [SerializeField] protected List<Ressource> coutRessources = new List<Ressource>();
    [SerializeField] protected int probability;
    [SerializeField] protected Sprite image;
    protected Transform ConstructionTransform;
    private GameObject[] pieces;
    
    public static Action<TypeUpgrade, float> onPercentChanged;

    public virtual void Initialisation(RarityConstruction rarityConstruction) {
        stats ??= new ConstructionStatsManager();
        ConstructionTransform = transform.GetChild(0);
        pieces = new GameObject[5];
        if (transform.childCount >= 2 && transform.GetChild(1).childCount == 5) {
            Transform allPieces = transform.GetChild(1);
            for (int i = 0; i < allPieces.childCount; i++) {
                pieces[i] = allPieces.GetChild((i+4)%5).gameObject;
            }
        }
        
        rarity = rarityConstruction;
        SetAllVariables();

        foreach (Ressource ressource in coutRessources) {
            ressource.quantite = (int)(ressource.quantite * GetRarityMultiplier());
        }
        
        onPercentChanged += OnPercentChanged;
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

    public int GetProbability() {
        return probability;
    }

    public string GetNom() {
        return nom;
    }

    public int GetNiveau() {
        return niveau;
    }

    public virtual void SetChildOf(Transform parent, bool onModule = true) {
        transform.SetParent(parent);
        
        if (onModule) {
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        
            float taille = 1f + (niveau - 1) * (2f - 1f) / (NiveauMax - 1);
            ConstructionTransform.localPosition = new Vector3(0, -0.5f * (taille - 1f), 0);
            ConstructionTransform.localScale = new Vector3(taille, taille, taille);
        
            for (int i = 0; i < pieces.Length; i++) {
                pieces[i].SetActive(i < niveau);
            }
        }
    }

    public bool IsSameConstruction(Construction c) {
        return c.type == type && c.niveau == niveau && c.rarity == rarity;
    }

    public bool Upgrade() {
        if (niveau >= NiveauMax) return false;
        
        niveau++;
        SetChildOf(transform.parent);
        SetAllVariables();
        PerformUpgrade();
        InventoryUI.Instance.ConstructionHaveUpdate(this);
        return true;
    }

    protected virtual void PerformUpgrade() {
    }

    // Méthode pour obtenir un multiplicateur basé sur la rareté
    private float GetRarityMultiplier() {
        switch (rarity) {
            case RarityConstruction.Common: return 1.0f;
            case RarityConstruction.Rare: return 1.25f;
            case RarityConstruction.Epic: return 1.5f;
            case RarityConstruction.Legendary: return 2f;
            default: return 1.0f;
        }
    }

    // Méthode pour obtenir la couleur en fonction de la rareté
    public Color GetRarityColor() {
        switch (rarity) {
            case RarityConstruction.Common: return Color.gray;
            case RarityConstruction.Rare: return Color.blue;
            case RarityConstruction.Epic: return new Color(0.5f, 0f, 0.5f);
            case RarityConstruction.Legendary: return Color.yellow;
            default: return Color.gray;
        }
    }

    public string GetRarityText() {
        switch (rarity) {
            case RarityConstruction.Common: return "Commun";
            case RarityConstruction.Rare: return "Rare";
            case RarityConstruction.Epic: return "Épique";
            case RarityConstruction.Legendary: return "Légendaire";
            default: return "Commun";
        }
    }
    
    protected virtual void OnPercentChanged(TypeUpgrade typeUpgrade, float percent) {
    }

    private void OnDestroy() {
        onPercentChanged -= OnPercentChanged;
    }

    private void SetAllVariables() {
        Dictionary<string, float> values = stats.GetDico(type, rarity, niveau-1);
        foreach (KeyValuePair<string, float> kvp in values) {
            FieldInfo field = GetType().GetField(kvp.Key, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) {
                field.SetValue(this, kvp.Value);
            } else {
                Debug.LogError("Field " + kvp.Key + " not found in " + GetType().Name);
            }
        }
    }

    public Dictionary<string, string> GetStats() {
        Dictionary<string, string> dico = new Dictionary<string, string> {
            { "Description", description }
        };
        Dictionary<string, float> values = stats.GetDico(type, rarity, niveau-1);
        foreach (KeyValuePair<string, float> kvp in values) {
            dico.Add(kvp.Key, kvp.Value.ToString("F2"));
        }

        return dico;
    }
}

public enum RarityConstruction {
    Common = 0,
    Rare = 1,
    Epic = 2,
    Legendary = 3,
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
