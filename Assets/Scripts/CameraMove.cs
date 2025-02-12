using UnityEngine;

public class CameraMove : MonoBehaviour {
    public Transform player; // Référence au joueur
    public float zoomSpeed = 10f; // Vitesse de zoom
    public float minY = 10f; // Hauteur minimale de la caméra
    public float maxY = 100f; // Hauteur maximale de la caméra

    void Update() {
        // Suivre le joueur
        if(player) {
            Vector3 newPosition = player.position;
            newPosition.y = transform.position.y;
            transform.position = newPosition;
        }

        // Zoomer / Dézoomer avec la molette de la souris
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0.0f) {
            float newY = transform.position.y - scroll * zoomSpeed;
            newY = Mathf.Clamp(newY, minY, maxY); // Limiter la hauteur de la caméra
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
