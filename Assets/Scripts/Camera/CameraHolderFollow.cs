using UnityEngine;

public class CameraHolderFollow : MonoBehaviour {
    /// <summary>
    /// Appelé après toutes les mises à jour de frame. Suit la position du joueur.
    /// </summary>
    private void LateUpdate() {
        // Suit le joueur
        transform.position = Vaisseau.Instance.transform.position;
    }
}
