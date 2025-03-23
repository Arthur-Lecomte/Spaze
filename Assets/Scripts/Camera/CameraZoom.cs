using Unity.VisualScripting;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Camera cam; // La caméra à modifier
    [SerializeField] private Camera camShield;
    [SerializeField] private float zoomSpeed = 10f; // Vitesse du zoom
    [SerializeField] private float minZoom = 5f;  // Zoom maximum (le plus proche)
    [SerializeField]private GameObject background;
    public float maxZoom = 50f; // Zoom minimum (le plus loin)

    private void Update()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel"); // Récupère la molette
        if (scrollInput != 0)
        {
            cam.orthographicSize -= scrollInput * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

            camShield.orthographicSize = cam.orthographicSize;

            float scaleFactor = Mathf.Lerp(0.4f, 4.3f, (cam.orthographicSize - minZoom) / (maxZoom - minZoom));
            background.transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
        }
    }
}
