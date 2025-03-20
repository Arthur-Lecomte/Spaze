using System;

[Serializable]
public class Ressource {
    public TypeRessource type;
    public int quantite;
    
    public Ressource(TypeRessource type, int quantite) {
        this.type = type;
        this.quantite = quantite;
    }
}

public enum TypeRessource {
    Cuivre,
    Argent,
    Or,
    Platine,
    PoussiereRadioactive
}