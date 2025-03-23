using UnityEngine;

public class Structure : MonoBehaviour {
    public Ressource ressource;

    public bool Extract() {
        Debug.Log("Extracting");
        int amountToExtract = 10;
        int number = Mathf.Min(ressource.quantite, amountToExtract);
        ressource.quantite -= number;
        Inventory.Instance.AddRessource(ressource.type, number);
        
        if (ressource.quantite <= 0) {
            //DEBUG!!! changer skin pour asteroid et mettre petite lumière quand plein et l'enlever quand plus rien pour épave
            tag = "Untagged"; //Retire son tag pour que l'extracteur ne le détecte plus
            return false;
        }

        return true;
    }
}
