using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Construction constructionObject;
    private Image image;
    private TextMeshProUGUI textLevel;
    private Image imageColorRarity;
    
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
        
        image = transform.GetChild(1).GetComponent<Image>();
        textLevel = transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        imageColorRarity = transform.GetChild(0).GetComponent<Image>();
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
            constructionObject = construction;
            image.sprite = construction.GetSprite();
            imageColorRarity.color = construction.GetRarityColor();
        } else {
            constructionObject = null;
        }
        SetCorrectNiveau();
        image.enabled = construction;
        imageColorRarity.enabled = construction;
    }
    
    public void SetCorrectNiveau() {
        textLevel.text = constructionObject ? constructionObject.GetNiveau() + "/5" : "";
    }
    
    public Construction GetConstruction() {
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
    
    public void OnPointerEnter(PointerEventData eventData) {
        if (constructionObject) {
            ConstructionInformationsUI.Instance.ShowHoverUI(constructionObject);
        }
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        if (constructionObject) {
            ConstructionInformationsUI.Instance.HideHoverUI();
        }
    }
}
