using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public abstract class Construction : MonoBehaviour {
    private static ConstructionStatsManager stats;

    [SerializeField] protected string nom;
    [SerializeField] protected string description;
    [SerializeField] protected RarityConstruction rarity;
    [SerializeField] protected TypeConstruction type;
    protected int NiveauMax = 5;
    protected int Niveau = 1;
    [SerializeField] protected List<Ressource> coutRessources = new List<Ressource>();
    [SerializeField] protected int probability;
    [SerializeField] protected Sprite image;
    private Transform constructionTransform;

    public virtual void Initialisation(RarityConstruction rarityConstruction) {
        stats = new ConstructionStatsManager();
        constructionTransform = transform.GetChild(0);
        rarity = rarityConstruction;
        SetAllVariables();

        foreach (Ressource ressource in coutRessources) {
            ressource.quantite = (int)(ressource.quantite * GetRarityMultiplier());
        }
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
        return Niveau;
    }

    public void SetChildOf(Transform parent) {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
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
        if (Niveau < NiveauMax) {
            Niveau++;
            SetChildOf(transform.parent);
            SetAllVariables();
            PerformUpgrade();
            InventoryUI.Instance.ConstructionHaveUpdate(this);
            return true;
        }

        return false;
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

    private void SetAllVariables() {
        Dictionary<string, float> values = stats.GetDico(type, rarity, Niveau);
        foreach (KeyValuePair<string, float> kvp in values) {
            FieldInfo field = GetType().GetField(kvp.Key, BindingFlags.NonPublic | BindingFlags.Instance);
            if (field != null) {
                field.SetValue(this, kvp.Value);
            }
        }
    }

    public virtual Dictionary<string, string> GetStats() {
        Dictionary<string, string> dico = new Dictionary<string, string> {
            { "Description", description }
        };
        Dictionary<string, float> values = stats.GetDico(type, rarity, Niveau);
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
