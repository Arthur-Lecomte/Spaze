using UnityEngine;

public class Structure : MonoBehaviour {
    public Ressource Ressource;

    public void Extract() {
        Debug.Log("Extracting");
        int amountToExtract = 10;
        int number = Mathf.Min(Ressource.quantite, amountToExtract);
        Ressource.quantite -= number;
        Inventory.Instance.AddRessource(Ressource.type, number);
    }
}
