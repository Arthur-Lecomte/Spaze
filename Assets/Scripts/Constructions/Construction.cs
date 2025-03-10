using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class Construction : MonoBehaviour {
    [SerializeField] protected string nom;
    [SerializeField] protected string description;
    [SerializeField] protected string rarity;
    [SerializeField] protected int niveau;
    [SerializeField] protected int niveauMax;
    protected Dictionary<TypeRessource, int> CoutRessources;
    
    public Dictionary<TypeRessource, int> GetCoutRessources() {
        return CoutRessources;
    }

    public override string ToString() {
        string couts = string.Join("\n", CoutRessources.Select(kv => $"{kv.Key}: {kv.Value}"));
        return $"Nom: {nom}\nCoût:\n{couts}";
    }

    public bool IsSameConstruction(Construction c) {
        return false; // Vérifier la class, le niveau et la rareté
    }
    public abstract void Upgrade();
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
