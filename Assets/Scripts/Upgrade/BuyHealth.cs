using System.Collections.Generic;
using SmallHedge.SoundManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyHealth : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private List<Ressource> coutRessources;

    /// <summary>
    /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
    /// </summary>
    private void Awake() {
        gameObject.name = "vie";
        coutRessources = new List<Ressource>();
    }

    /// <summary>
    /// Active ou désactive l'achat de santé en fonction de la santé manquante.
    /// </summary>
    /// <param name="value">True pour activer, False pour désactiver.</param>
    public void Activate(bool value) {
        int missingHealth = (int)Vaisseau.Instance.GetHealthForBeFull();
        gameObject.SetActive(value && missingHealth > 0);
        if (value) {
            coutRessources.Clear();
            coutRessources.Add(new Ressource(TypeRessource.Cuivre, (1 - missingHealth / 100) * 25));
            coutRessources.Add(new Ressource(TypeRessource.Argent, missingHealth));
            coutRessources.Add(new Ressource(TypeRessource.Or, missingHealth / 5 + 15));
            coutRessources.Add(new Ressource(TypeRessource.Platine, missingHealth / 100 * 25));
        }
    }

    /// <summary>
    /// Méthode appelée lorsque le pointeur entre dans l'élément.
    /// </summary>
    /// <param name="eventData">Les données de l'événement du pointeur.</param>
    public void OnPointerEnter(PointerEventData eventData) {
        BuyHoverUI.Instance.ShowHoverUI(name, coutRessources);
    }

    /// <summary>
    /// Tente d'acheter de la santé pour le vaisseau.
    /// </summary>
    public void WantBuy() {
        if (Inventory.Instance.HaveEnoughRessources(coutRessources)) {
            Inventory.Instance.RemoveRessources(coutRessources);
            Vaisseau.Instance.Regeneration();
            gameObject.SetActive(false);
            SoundManager.PlaySound(SoundType.BUY);
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
