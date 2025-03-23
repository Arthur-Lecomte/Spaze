using UnityEngine;
using UnityEngine.EventSystems;

public class ShopInteractionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
    private Outline outline;
    private bool mouseOn;
    private bool isInShopRange;

    private void Start() {
        outline = GetComponent<Outline>();
    }
    
    public void SetIsInShopRange(bool value) {
        isInShopRange = value;
        CheckOutline();
        
        if (!isInShopRange && ShopManager.Instance.IsShopOpen()) {
            ShopManager.Instance.ChangeShopOption(0);
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
            ShopManager.Instance.ChangeShopOption(-1);
        }
    }
}
