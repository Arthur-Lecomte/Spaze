using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryAddSlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler {
    private Image image;
    [SerializeField] private List<Ressource> coutRessources;
    private bool isBuy;
    

    public void Initialisation() {
        gameObject.name = "place d'inventaire";
        image = transform.GetChild(0).GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isBuy) {
            BuyHoverUI.Instance.ShowHoverUI(name, coutRessources);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!isBuy && Inventory.Instance.HaveEnoughRessources(coutRessources)) {
            BuyHoverUI.Instance.HideHoverUI();
            
            Inventory.Instance.RemoveRessources(coutRessources);

            Inventory.Instance.AddInventorySlotsSize();
            IsBuy(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!isBuy) {
            BuyHoverUI.Instance.HideHoverUI();
        }
    }
    
    public void IsBuy(bool value) {
        isBuy = value;
        image.enabled = !value;
    }
}
