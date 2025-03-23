using UnityEngine;
using UnityEngine.UI; // Nécessaire pour accéder au composant Image

public class ScrollUV : MonoBehaviour
{
    [SerializeField] public float parallax = 2f;

    void Update()
    {
        // Récupérer le composant Image
        Image image = GetComponent<Image>();
        if (image != null && image.material != null)
        {
            // Modifier l'offset de la texture
            Material material = image.material;
            Vector2 offset = material.mainTextureOffset;
            offset.x += Time.deltaTime / 10f / parallax;
            material.mainTextureOffset = offset;
        }
        else
        {
            Debug.LogWarning("Aucun composant Image ou matériau trouvé sur cet objet !");
        }
    }
}