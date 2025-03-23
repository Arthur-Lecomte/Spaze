using UnityEngine;

public class Asteroid : Structure {
    private void Awake() {
        int ramdomValue = Random.Range(0, 101);
        System.Array values = System.Enum.GetValues(typeof(TypeRessource));
        TypeRessource ressourceType = (TypeRessource)values.GetValue(Random.Range(0, values.Length));
        int returnRessourceValue;
        switch (ressourceType) {    
            case TypeRessource.Cuivre:
                returnRessourceValue = ramdomValue <= 90 ? Random.Range(0, 51) : 0;
                break;
            case TypeRessource.Argent:
                returnRessourceValue = ramdomValue <= 80 ? Random.Range(0, 51) : 0;
                break;
            case TypeRessource.Or:
                returnRessourceValue = ramdomValue <= 60 ? Random.Range(0, 26) : 0;
                break;
            case TypeRessource.Platine:
                returnRessourceValue = ramdomValue <= 40 ? Random.Range(0, 11) : 0;
                break;
            default:
                returnRessourceValue = 0;
                break;
        }
        ressource.quantite = returnRessourceValue;
    }
}
