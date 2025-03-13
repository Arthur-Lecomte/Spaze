using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour {
    public static InventoryUI Instance;

    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private GameObject dragImagePrefab;
    private RectTransform rectTransform;

    private List<GameObject> constructionsList;
    private int numberSlotsMemory;

    [SerializeField] private Camera mainCamera;
    private GameObject dragImage;
    private Image dragImageComponent;
    private int draggedSlotIndex = -1;
    private VaisseauModule draggedConstruction;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        rectTransform = GetComponent<RectTransform>();
        constructionsList = new List<GameObject>();

        dragImage = Instantiate(dragImagePrefab, transform);
        dragImageComponent = dragImage.GetComponent<Image>();
        dragImage.SetActive(false);
    }

    public void ChangeNumberSlots(int numberSlots) {
        int columns = (numberSlots + 1) / 2;
        rectTransform.sizeDelta = new Vector2(15 + 105 * columns, 250);
        rectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x / 2 + 25, 150);

        for(int i = numberSlotsMemory + 1; i < numberSlots + 1; i++) {
            int column = (i + 1) / 2;
            GameObject slot = Instantiate(slotPrefab, rectTransform);
            constructionsList.Add(slot);
            RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
            slotRectTransform.anchoredPosition = new Vector3(-45 + 105 * column, -65 + 105 * (i % 2), 0);

            EventTrigger trigger = slot.AddComponent<EventTrigger>();
            int index = i - 1;
            AddEventTrigger(trigger, EventTriggerType.PointerDown, (data) => OnPointerDown((PointerEventData)data, slot, index));
            AddEventTrigger(trigger, EventTriggerType.Drag, (data) => OnDrag((PointerEventData)data));
            AddEventTrigger(trigger, EventTriggerType.PointerUp, (data) => OnPointerUp((PointerEventData)data));
        }
        numberSlotsMemory = numberSlots;
    }

    public void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> action) {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
        entry.callback.AddListener((data) => action(data));
        trigger.triggers.Add(entry);
    }

    public void UpdateSlotImage(int index, Construction newConstruction) {
        Image imageComponent = constructionsList[index].transform.Find("ImageConstruction").GetComponent<Image>();
        if(newConstruction) {
            imageComponent.sprite = newConstruction.GetImage();
        }
        imageComponent.enabled = newConstruction;
    }

    private void OnPointerDown(PointerEventData eventData, GameObject slot, int index) {
        BuildManager.Instance.CurrentConstruction(Inventory.Instance.GetInventorySlots(index));
        if(Inventory.Instance.GetInventorySlots(index) == null) return;

        draggedSlotIndex = index;
        Image imageComponent = slot.transform.Find("ImageConstruction").GetComponent<Image>();
        dragImageComponent.sprite = imageComponent.sprite;
        dragImageComponent.color = new Color(1, 1, 1, 0.5f);
        dragImage.transform.position = eventData.position;
        dragImage.transform.SetAsLastSibling();
        dragImage.SetActive(true);
    }

    public void OnPointerDownModule(VaisseauModule module, Vector2 position) {
        draggedSlotIndex = -2;
        draggedConstruction = module;
        dragImageComponent.sprite = module.GetConstruction().GetImage();
        dragImageComponent.color = new Color(1, 1, 1, 0.5f);
        dragImage.transform.position = position;
        dragImage.transform.SetAsLastSibling();
        dragImage.SetActive(true);
    }

    public void OnDrag(PointerEventData eventData) {
        if(dragImage.activeSelf) {
            dragImage.transform.position = eventData.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData) {
        if(dragImage.activeSelf) {
            dragImage.SetActive(false);

            PointerEventData pointerEventData = new PointerEventData(EventSystem.current) {
                position = eventData.position
            };

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointerEventData, raycastResults);

            bool handled = false;
            foreach(RaycastResult result in raycastResults) {
                GameObject slot = result.gameObject;
                if(constructionsList.Contains(slot)) {
                    int targetIndex = constructionsList.IndexOf(slot);
                    if(targetIndex != draggedSlotIndex) {
                        if(draggedSlotIndex == -2) {
                            Construction construction = draggedConstruction.GetConstruction();
                            draggedConstruction.SetConstruction(Inventory.Instance.GetInventorySlots(targetIndex));
                            Inventory.Instance.SetInventorySlots(targetIndex, construction);
                        } else {
                            Inventory.Instance.MoveInventorySlot(draggedSlotIndex, targetIndex);
                        }
                    }
                    handled = true;
                    break;
                }
            }

            if(!handled) {
                Ray ray = mainCamera.ScreenPointToRay(eventData.position);
                if(Physics.Raycast(ray, out RaycastHit hit)) {
                    VaisseauModule module = hit.collider.GetComponent<VaisseauModule>();
                    if(module != null) {
                        if(draggedSlotIndex == -2) {
                            Construction construction = draggedConstruction.GetConstruction();
                            draggedConstruction.SetConstruction(module.GetConstruction());
                            module.SetConstruction(construction);
                        } else {
                            Construction construction = Inventory.Instance.RemoveInventorySlots(draggedSlotIndex);
                            Inventory.Instance.SetInventorySlots(draggedSlotIndex, module.GetConstruction());
                            module.SetConstruction(construction);
                        }
                    }
                }
            }
        }
        draggedSlotIndex = -1;
        draggedConstruction = null;
    }
}
