using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryAddSlot : MonoBehaviour {
    private Image image;
    [SerializeField] private List<Ressource> coutRessources;
    
    private EventTrigger trigger;

    public void Initialisation() {
        image = transform.GetChild(0).GetComponent<Image>();

        trigger = gameObject.AddComponent<EventTrigger>();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        //DEBUG!!! afficher over avec prix
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (Inventory.Instance.HaveEnoughRessources(coutRessources)) {
            //DEBUG!!! Demander une verification
            //DEBUG!!! enlever over avec prix si acheté
            
            Inventory.Instance.RemoveRessources(coutRessources);

            Inventory.Instance.AddInventorySlotsSize();
            IsBuy(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        //DEBUG!!! enlever over avec prix
    }
    
    public void IsBuy(bool value) {
        if (value) {
            image.enabled = false;
            trigger.triggers.Clear();
        } else {
            image.enabled = true;
            AddEventTrigger(EventTriggerType.PointerEnter, (data) => OnPointerEnter((PointerEventData)data));
            AddEventTrigger(EventTriggerType.PointerDown, (data) => OnPointerDown((PointerEventData)data));
            AddEventTrigger(EventTriggerType.PointerExit, (data) => OnPointerExit((PointerEventData)data));
        }
    }
    
    private void AddEventTrigger(EventTriggerType eventType, System.Action<BaseEventData> action) {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((data) => action(data));
        trigger.triggers.Add(entry);
    }
}
