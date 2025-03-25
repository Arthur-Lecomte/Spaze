using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour
{
    [SerializeField] public float radius = 1f; // Rayon du cercle
    [SerializeField] private int segments = 50; // Nombre de segments pour le cercle
    [SerializeField] private Color circleColor = Color.blue; // Couleur du cercle
    [SerializeField] private float lineWidth = 0.05f; // Largeur de la ligne

    private LineRenderer lineRenderer;

    private Shop shop;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = false;
        lineRenderer.loop = true;
        lineRenderer.positionCount = segments;

        // Assigner un matériau par défaut compatible avec les couleurs dynamiques
        Material defaultMaterial = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.material = defaultMaterial;

        // Appliquer la couleur
        lineRenderer.startColor = circleColor;
        lineRenderer.endColor = circleColor;

        // Appliquer la largeur initiale
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;

        // Dessiner le cercle initial
        DrawCircle();
        
        shop = transform.parent.GetComponentInChildren<Shop>();
    }

    private void DrawCircle()
    {
        float angleStep = 360f / segments;
        Vector3[] positions = new Vector3[segments];

        for (int i = 0; i < segments; i++)
        {
            float angle = Mathf.Deg2Rad * (i * angleStep);
            positions[i] = new Vector3(Mathf.Cos(angle) * radius, 0f, Mathf.Sin(angle) * radius); // Utiliser le plan XZ
        }

        lineRenderer.SetPositions(positions);
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            shop.SetIsInShopRange(true);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            shop.SetIsInShopRange(false);
        }
    }
}