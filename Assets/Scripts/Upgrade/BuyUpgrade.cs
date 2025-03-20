using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyUpgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private List<Ressource> coutRessources;
    private int canBuy = 12;

    private void Awake() {
        gameObject.name = "énergie";
    }

    public void OnPointerEnter(PointerEventData eventData) {
        BuyHoverUI.Instance.ShowHoverUI(name, coutRessources);
    }

    public void WantBuy() {
        if (Inventory.Instance.HaveEnoughRessources(coutRessources)) {
            Inventory.Instance.RemoveRessources(coutRessources);
            UpgradeManager.Instance.AddPoints();
            canBuy -= 1;
            if (canBuy == 0) {
                gameObject.SetActive(false);
                BuyHoverUI.Instance.HideHoverUI();
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        BuyHoverUI.Instance.HideHoverUI();
    }
}
