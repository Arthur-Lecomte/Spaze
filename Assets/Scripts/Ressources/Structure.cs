using UnityEngine;

public class Structure : MonoBehaviour {
    public bool isAsteroid;
    public Ressource ressource;
    public GameObject emptyAsteroid;

    public void SetRessource(Ressource r) {
        ressource = r;
        CheckQuantity();
    }

    public bool Extract(float amount) {
        int number = Mathf.Min(ressource.quantite, (int)amount);
        ressource.quantite -= number;
        Inventory.Instance.AddRessource(ressource.type, number);

        GenerationStructure.Instance.SaveCellState(gameObject);

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
            gameObject.layer = 0; //Retire son layer pour que le laser de l'extracteur ne le détecte plus
        }
    }
}
