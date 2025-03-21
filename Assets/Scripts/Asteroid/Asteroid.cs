using UnityEngine;

public class Asteroid : Structure {
    private bool isCollecting;
    private float countdown;

    private void Awake() {
        int ramdomValue = Random.Range(0, 100 + 1);
        System.Array values = System.Enum.GetValues(typeof(TypeRessource));
        TypeRessource ressourceType = (TypeRessource)values.GetValue(Random.Range(0, values.Length));
        int returnRessourceValue;
        switch (ressourceType) {    
            case TypeRessource.Cuivre:
                returnRessourceValue = ramdomValue <= 50 ? Random.Range(1, 30 + 1) : 0;
                break;
            case TypeRessource.Argent:
                returnRessourceValue = ramdomValue <= 30 ? Random.Range(1, 15 + 1) : 0;
                break;
            case TypeRessource.Or:
                returnRessourceValue = ramdomValue <= 15 ? Random.Range(1, 5 + 1) : 0;
                break;
            case TypeRessource.Platine:
                returnRessourceValue = ramdomValue <= 5 ? Random.Range(1, 2 + 1) : 0;
                break;
            default:
                returnRessourceValue = 0;
                break;
        }
        Ressource.quantite = returnRessourceValue;
    }
}
