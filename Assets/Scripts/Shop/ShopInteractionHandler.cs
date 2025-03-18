using UnityEngine;

public class ShopInteractionHandler : MonoBehaviour
{
    private Outline outline;
    private bool isInShopRange = false;

    [SerializeField] private CircleDrawer circleDrawer; // Référence au CircleDrawer
    [SerializeField] private Transform player; // Référence au vaisseau (joueur)

    public bool IsInShopRange => isInShopRange; // Propriété publique pour accéder au booléen
    [SerializeField] private Shop shop;

    private void Start()
    {
        outline = GetComponent<Outline>();
    
    }

    private void Update()
    {
        if (circleDrawer != null && player != null)
        {
            // Vérifier si la distance entre le joueur et le centre du CircleDrawer est inférieure au radius
            float distance = Vector3.Distance(player.position, circleDrawer.transform.position);
            isInShopRange = distance <= circleDrawer.radius*10;
            
        }
    }

    private void OnMouseEnter()
    {
        // Activer l'outline uniquement si le joueur est dans le rayon
        if (isInShopRange && outline != null)
        {
            outline.enabled = true;
        }
    }

    private void OnMouseExit()
    {
        // Désactiver l'outline lorsque la souris quitte l'objet ou si le joueur n'est pas dans le rayon
        if (outline != null)
        {
            outline.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        // Vérifier si le joueur est dans le rayon et appeler ToggleShop()
        if (isInShopRange && shop != null)
        {
            shop.ToggleShop(); // Appeler la méthode ToggleShop
        }
    }
}