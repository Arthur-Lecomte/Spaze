using UnityEngine;
namespace Spaze {
    public class Structure : MonoBehaviour {
        public bool isAsteroid;
        public Ressource ressource;
        public GameObject emptyAsteroid;

        /// <summary>
        /// Définit la ressource associée à cette structure et vérifie la quantité.
        /// </summary>
        /// <param name="r">La ressource à définir.</param>
        public void SetRessource(Ressource r) {
            ressource = r;
            CheckQuantity();
        }

        /// <summary>
        /// Extrait une quantité spécifiée de ressource de cette structure.
        /// </summary>
        /// <param name="amount">La quantité à extraire.</param>
        /// <returns>True si la structure contient encore des ressources après l'extraction, sinon False.</returns>
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

        /// <summary>
        /// Vérifie la quantité de ressource restante et met à jour l'état de la structure si nécessaire.
        /// </summary>
        private void CheckQuantity() {
            if (ressource.quantite == 0) {
                if (isAsteroid) {
                    Destroy(transform.GetChild(0).gameObject);
                    GameObject go = Instantiate(emptyAsteroid, transform);
                    go.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                }
                tag = "Untagged"; // Retire son tag pour que l'extracteur ne le détecte plus
                gameObject.layer = 0; // Retire son layer pour que le laser de l'extracteur ne le détecte plus
            }
        }
    }
}