using System.Collections.Generic;
using SmallHedge.SoundManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryAddSlot : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler {
    private Image image;
    [SerializeField] private List<Ressource> coutRessources;
    private bool isBuy;

    /// <summary>
    /// Initialise le slot d'inventaire.
    /// </summary>
    public void Initialisation() {
        gameObject.name = "place d'inventaire";
        image = transform.GetChild(0).GetComponent<Image>();
    }

    /// <summary>
    /// Méthode appelée lorsque le pointeur entre dans le slot d'inventaire.
    /// </summary>
    /// <param name="eventData">Les données de l'événement du pointeur.</param>
    public void OnPointerEnter(PointerEventData eventData) {
        if (!isBuy) {
            BuyHoverUI.Instance.ShowHoverUI(name, coutRessources);
        }
    }

    /// <summary>
    /// Méthode appelée lorsque le pointeur clique sur le slot d'inventaire.
    /// </summary>
    /// <param name="eventData">Les données de l'événement du pointeur.</param>
    public void OnPointerDown(PointerEventData eventData) {
        if (!isBuy && Inventory.Instance.HaveEnoughRessources(coutRessources)) {
            BuyHoverUI.Instance.HideHoverUI();

            Inventory.Instance.RemoveRessources(coutRessources);
            SoundManager.PlaySound(SoundType.BUY);
            Inventory.Instance.AddInventorySlotsSize();
            IsBuy(true);
        }
    }

    /// <summary>
    /// Méthode appelée lorsque le pointeur sort du slot d'inventaire.
    /// </summary>
    /// <param name="eventData">Les données de l'événement du pointeur.</param>
    public void OnPointerExit(PointerEventData eventData) {
        if (!isBuy) {
            BuyHoverUI.Instance.HideHoverUI();
        }
    }

    /// <summary>
    /// Définit l'état d'achat du slot d'inventaire.
    /// </summary>
    /// <param name="value">True si le slot est acheté, sinon False.</param>
    public void IsBuy(bool value) {
        isBuy = value;
        image.enabled = !value;
    }
}
