using UnityEngine;

public class ShopInteractionHandler : MonoBehaviour {
    private Outline outline;
    private bool mouseOn;
    private bool isInShopRange;
    private bool isShopOpen;
    private CircleDrawer circleDrawer;

    private void Start() {
        outline = GetComponent<Outline>();
        circleDrawer = transform.parent.GetComponentInChildren<CircleDrawer>();
    }
    

    private void Update() {
        // Vérifier si la distance entre le joueur et le centre du CircleDrawer est inférieure au radius
        float distance = Vector3.Distance(Vaisseau.Instance.transform.position, circleDrawer.transform.position);
        isInShopRange = distance <= circleDrawer.radius;

        if(isInShopRange && mouseOn) {
            outline.enabled = true;
        } else {
            outline.enabled = false;
            if (!isInShopRange && ShopUI.Instance.IsShopOpen()) {
                ShopUI.Instance.ChangeShopOption(ShopOption.None);
            }
        }
    }

    private void OnMouseEnter() {
        // Activer l'outline uniquement si le joueur est dans le rayon
        if (isInShopRange) {
            mouseOn = true;
        }
    }

    private void OnMouseExit() {
        // Désactiver l'outline lorsque la souris quitte l'objet ou si le joueur n'est pas dans le rayon
        mouseOn = false;
    }

    private void OnMouseDown() {
        // Vérifier si le joueur est dans le rayon et appeler ToggleShop()
        if (isInShopRange) {
            ShopUI.Instance.ChangeShopOption(ShopOption.PurchaseConstruction);
        }
    }
}
