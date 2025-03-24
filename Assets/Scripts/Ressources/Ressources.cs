using System;
using UnityEngine;

[Serializable]
public class Ressource {
    public TypeRessource type;
    private Sprite icon;
    public int quantite;
    
    public Ressource(TypeRessource type, int quantite) {
        this.type = type;
        this.quantite = quantite;
    }

    public Sprite GetSprite() {
        if (!icon) {
            icon = Resources.Load<Sprite>($"IconRessource/{type.ToString()}");
        }
        return icon;
    }
    
    public static Sprite GetRessourceSprite(TypeRessource type) {
        return Resources.Load<Sprite>($"IconRessource/{type.ToString()}");
    }
}

public enum TypeRessource {
    Cuivre,
    Argent,
    Or,
    Platine,
    NoyauEnergie
}