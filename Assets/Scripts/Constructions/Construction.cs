using System.Collections.Generic;
using UnityEngine;

public abstract class Construction : MonoBehaviour {
    [SerializeField] protected string nom;
    [SerializeField] protected string description;
    protected RarityConstruction Rarity;
    [SerializeField] protected TypeConstruction type;     
    protected int niveauMax = 5;
    protected int Niveau = 1;
    protected List<Ressource> CoutRessources;
    [SerializeField] protected float probability;
    [SerializeField] protected Sprite image;
    private Transform constructionTransform;
    
    protected virtual void Awake() {
        constructionTransform = transform.GetChild(0);
    }
    
    public List<Ressource> GetCoutRessources() {
        return CoutRessources;
    }
    
    public Sprite GetSprite() {
        return image;
    }
    
    public void SetChildOf(Transform parent) {
        transform.SetParent(parent);
        transform.localPosition = Vector3.zero;
        float taille = 1f + (Niveau - 1) * (2f - 1f) / (niveauMax - 1);
        constructionTransform.localPosition = new Vector3(0, -0.5f * (taille - 1f), 0);
        constructionTransform.localScale = new Vector3(taille, taille, taille);
    }

    public bool IsSameConstruction(Construction c) {
        if (c.GetType() == GetType() && c.Niveau == Niveau && c.Rarity == Rarity) {
            return false; //DEBUG!!! TEST !
        }
        return false;
    }

    public bool Upgrade() { //DEBUG!!! Modifier l'UI ?
        if (Niveau < niveauMax) {
            Niveau++;
            SetChildOf(transform.parent);
            PerformUpgrade();
            return true;
        }
        return false;
    }
    
    protected abstract void PerformUpgrade();
}

public enum RarityConstruction {
    Common,
    Rare,
    Epic,
    Legendary
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
