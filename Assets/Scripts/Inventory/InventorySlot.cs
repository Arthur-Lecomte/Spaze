using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour {
    private GameObject constructionObject;
    private Image image;
    
    [SerializeField] private int index;
    private AddonModule module;
    
    private void Awake() {
        if (index == -1) {
            transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;
            
            module = transform.parent.parent.GetComponent<AddonModule>();
            Initialisation(-1);
        }
    }
    
    private void Start() {
        if (index == -1) {
            InventoryUI.Instance.AddInventorySlots(this);
            module.onAddConstruction.AddListener(SetGameObject);
            SetActive(false);
        }
    }
    
    public void Initialisation(int i) {
        index = i;

        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        AddEventTrigger(trigger, EventTriggerType.PointerDown, (data) => InventoryUI.Instance.OnPointerDown((PointerEventData)data, this));
        AddEventTrigger(trigger, EventTriggerType.Drag, (data) => InventoryUI.Instance.OnDrag((PointerEventData)data));
        AddEventTrigger(trigger, EventTriggerType.PointerUp, (data) => InventoryUI.Instance.OnPointerUp((PointerEventData)data));
        
        image = transform.GetChild(0).GetComponent<Image>();
    }
    
    private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> action) {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((data) => action(data));
        trigger.triggers.Add(entry);
    }
    
    public void SetGameObject(Construction construction) {
        if (module) {
            module.SetConstruction(construction);
        }
        if(construction) {
            constructionObject = construction.gameObject;
            image.sprite = construction.GetSprite();
        } else {
            constructionObject = null;
        }
        image.enabled = construction;
    }
    
    public GameObject GetGameObject() {
        return constructionObject;
    }
    
    public Sprite GetSprite() {
        return image.sprite;
    }
    
    public int GetIndex() {
        return index;
    }
    
    public void Exchange(InventorySlot inventorySlot) {
        Construction construction1;
        Construction construction2;
        
        if (index == -1) {
            construction1 = module.GetConstruction();
        } else {
            construction1 = Inventory.Instance.RemoveInventorySlot(index);
        }
        
        if (inventorySlot.index == -1) {
            construction2 = inventorySlot.module.GetConstruction();
            inventorySlot.SetGameObject(construction1);
        } else {
            construction2 = Inventory.Instance.RemoveInventorySlot(inventorySlot.index);
            Inventory.Instance.SetInventorySlot(inventorySlot.index, construction1);
        }
        
        if (index == -1) {
            SetGameObject(construction2);
        } else {
            Inventory.Instance.SetInventorySlot(index, construction2);
        }
    }

    public void SetActive(bool value) {
        if (module) {
            gameObject.SetActive(value && module.IsActivate());
        }
    }
}
