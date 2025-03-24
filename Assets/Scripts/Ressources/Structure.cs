using UnityEngine;

public class Structure : MonoBehaviour {
    public bool isAsteroid;
    public Ressource ressource;
    public GameObject emptyAsteroid;

    public void SetRessource(Ressource r) {
        ressource = r;
        CheckQuantity();
    }

    public bool Extract() {
        int amountToExtract = 10;
        int number = Mathf.Min(ressource.quantite, amountToExtract);
        ressource.quantite -= number;
        Inventory.Instance.AddRessource(ressource.type, number);

        if (ressource.quantite <= 0) {
            CheckQuantity();
            return false;
        }
        return true;
    }

    private void CheckQuantity() {
        if (ressource.quantite == 0) {
            if (isAsteroid) {
                Destroy(transform.GetChild(0).gameObject);
                GameObject go = Instantiate(emptyAsteroid, transform);
                go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            }
            tag = "Untagged"; //Retire son tag pour que l'extracteur ne le détecte plus
        }
    }
}
