using System.Collections.Generic;
using UnityEngine;

public abstract class Construction : MonoBehaviour {
    [SerializeField] protected string nom;
    [SerializeField] protected string description;
    [SerializeField] protected RarityConstruction rarity;
    [SerializeField] protected int niveauMax;
    protected int Niveau;
    protected Dictionary<TypeRessource, int> CoutRessources;
    [SerializeField] protected float probability;
    [SerializeField] protected Sprite image;
    
    public Dictionary<TypeRessource, int> GetCoutRessources() {
        return CoutRessources;
    }
    
    public Sprite GetImage() {
        return image;
    }

    public bool IsSameConstruction(Construction c) {
        return false; // Vérifier la class, le niveau et la rareté
    }
    
    public abstract void Upgrade();
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
    Extraction, //Augmente la quantité de ressources récoltées
    Radar, //Augmente la portée du radar (ennemis et/ou ressources)
}
/* Notes à voir avec l'équipe:
- Choisir le mode d'attaque des armes (ennemi le plus proche, le plus faible, avec le plus de PV...)
- Pouvoir sélectionner des ennemis pour les ciblés en priorité
*/
