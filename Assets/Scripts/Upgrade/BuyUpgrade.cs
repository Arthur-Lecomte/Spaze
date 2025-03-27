using System.Collections.Generic;
using SmallHedge.SoundManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyUpgrade : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    [SerializeField] private List<Ressource> coutRessources;
    private int canBuy = 12;

    /// <summary>
    /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
    /// </summary>
    private void Awake() {
        gameObject.name = "énergie";
    }

    /// <summary>
    /// Méthode appelée lorsque le pointeur entre dans l'élément.
    /// </summary>
    /// <param name="eventData">Les données de l'événement du pointeur.</param>
    public void OnPointerEnter(PointerEventData eventData) {
        BuyHoverUI.Instance.ShowHoverUI(name, coutRessources);
    }

    /// <summary>
    /// Tente d'acheter une amélioration.
    /// </summary>
    public void WantBuy() {
        if (Inventory.Instance.HaveEnoughRessources(coutRessources)) {
            Inventory.Instance.RemoveRessources(coutRessources);
            UpgradeManager.Instance.AddPoints();
            BuyHoverUI.Instance.ShowHoverUI(name, coutRessources); // On réactualise l'UI
            SoundManager.PlaySound(SoundType.BUY);

            canBuy -= 1;
            if (canBuy == 0) {
                gameObject.SetActive(false);
                BuyHoverUI.Instance.HideHoverUI();
            }
        }
    }

    /// <summary>
    /// Méthode appelée lorsque le pointeur sort de l'élément.
    /// </summary>
    /// <param name="eventData">Les données de l'événement du pointeur.</param>
    public void OnPointerExit(PointerEventData eventData) {
        BuyHoverUI.Instance.HideHoverUI();
    }
}
