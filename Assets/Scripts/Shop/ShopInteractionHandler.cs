using UnityEngine;
using UnityEngine.EventSystems;

public class ShopInteractionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    private Outline outline;
    private bool mouseOn;
    private bool isInShopRange;
    private CircleDrawer circleDrawer;

    private void Start() {
        outline = GetComponent<Outline>();
        circleDrawer = transform.parent.GetComponentInChildren<CircleDrawer>();
    }
    
    public void SetIsInShopRange(bool value) {
        isInShopRange = value;
        CheckOutline();
        
        if (!isInShopRange && ShopManager.Instance.IsShopOpen()) {
            ShopManager.Instance.CloseShop();
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        mouseOn = true;
        CheckOutline();
    }

    public void OnPointerExit(PointerEventData eventData) {
        mouseOn = false;
        CheckOutline();
    }
    
    private void CheckOutline() {
        outline.enabled = isInShopRange && mouseOn && !ShopManager.Instance.IsShopOpen();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (isInShopRange && !ShopManager.Instance.IsShopOpen()) {
            ShopManager.Instance.OpenShop();
        }
    }
}
