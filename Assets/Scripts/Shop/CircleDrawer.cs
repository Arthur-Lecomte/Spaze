using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class CircleDrawer : MonoBehaviour
{
    [SerializeField] public float radius = 1f; // Rayon du cercle
    [SerializeField] private int segments = 50; // Nombre de segments pour le cercle
    [SerializeField] private Color circleColor = Color.blue; // Couleur du cercle
    [SerializeField] private float lineWidth = 0.05f; // Largeur de la ligne

    private LineRenderer lineRenderer;
    private float previousRadius; // Pour détecter les changements de rayon
    private float previousLineWidth; // Pour détecter les changements de largeur

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

        // Assigner la couche "IgnoreOutline" uniquement à ce GameObject
        gameObject.layer = LayerMask.NameToLayer("IgnoreOutline");

        // Dessiner le cercle initial
        DrawCircle();

        // Initialiser les valeurs précédentes
        previousRadius = radius;
        previousLineWidth = lineWidth;
    }

    private void Update()
    {
        // Vérifier si le rayon a changé
        if (Mathf.Abs(previousRadius - radius) > Mathf.Epsilon)
        {
            DrawCircle(); // Redessiner le cercle
            previousRadius = radius; // Mettre à jour le rayon précédent
        }

        // Vérifier si la largeur a changé
        if (Mathf.Abs(previousLineWidth - lineWidth) > Mathf.Epsilon)
        {
            lineRenderer.startWidth = lineWidth;
            lineRenderer.endWidth = lineWidth;
            previousLineWidth = lineWidth; // Mettre à jour la largeur précédente
        }
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
}