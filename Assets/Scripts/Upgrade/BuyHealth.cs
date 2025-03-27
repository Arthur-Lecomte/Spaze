using System.Collections.Generic;
using SmallHedge.SoundManager;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuyHealth : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private List<Ressource> coutRessources;

    private void Awake() {
        gameObject.name = "vie";
        coutRessources = new List<Ressource>();
    }
    
    public void Activate(bool value) {
        int missingHealth = (int)Vaisseau.Instance.GetHealthForBeFull();
        gameObject.SetActive(value && missingHealth > 0);
        if (value) {
            coutRessources.Clear();
            coutRessources.Add(new Ressource(TypeRessource.Cuivre, (1 -missingHealth/100) * 25));
            coutRessources.Add(new Ressource(TypeRessource.Argent, missingHealth));
            coutRessources.Add(new Ressource(TypeRessource.Or, missingHealth/5 + 15));
            coutRessources.Add(new Ressource(TypeRessource.Platine, missingHealth/100 * 25));
        }
        
    }

    public void OnPointerEnter(PointerEventData eventData) {
        BuyHoverUI.Instance.ShowHoverUI(name, coutRessources);
    }

    public void WantBuy() {
        if (Inventory.Instance.HaveEnoughRessources(coutRessources)) {
            Inventory.Instance.RemoveRessources(coutRessources);
            Vaisseau.Instance.Regeneration();
            gameObject.SetActive(false);
            SoundManager.PlaySound(SoundType.BUY);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        BuyHoverUI.Instance.HideHoverUI();
    }
}
