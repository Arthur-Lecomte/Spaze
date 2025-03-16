using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VaisseauModule : MonoBehaviour, ICanTakeDamage, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler {
    [SerializeField] private List<Ressource> coutRessources;
    [SerializeField] private bool isActivate;
    private EventTrigger eventTrigger;

    private Collider objectCollider;
    private Renderer objectRenderer;
    private Construction construction;
    
    private GameObject previewInstance;
    private Transform parentPreview;
    private Vector3 localPositionPreview;
    private float alphaPreview;

    private void Awake() {
        objectCollider = GetComponent<Collider>();
        objectRenderer = GetComponent<Renderer>();

        if(!isActivate) {
            objectCollider.enabled = false;
            objectRenderer.enabled = false;
            ChangeAlpha(objectRenderer, 0.25f);
        }
    }

    private void Start() {
        eventTrigger = gameObject.AddComponent<EventTrigger>();
        InventoryUI.Instance.AddEventTrigger(eventTrigger, EventTriggerType.PointerDown, (data) => InventoryUI.Instance.OnPointerDownModule(this, Input.mousePosition));
        InventoryUI.Instance.AddEventTrigger(eventTrigger, EventTriggerType.Drag, (data) => InventoryUI.Instance.OnDrag((PointerEventData)data));
        InventoryUI.Instance.AddEventTrigger(eventTrigger, EventTriggerType.PointerUp, (data) => InventoryUI.Instance.OnPointerUp((PointerEventData)data));
        eventTrigger.enabled = false;
    }

    public bool IsActivate() {
        return isActivate;
    }

    public void SetConstruction(Construction c) {
        construction = c;
        if(construction) {
            construction.SetChildOf(transform);
        }
        eventTrigger.enabled = construction;
    }

    public Construction GetConstruction() {
        return construction;
    }

    public bool IsEmpty() {
        return construction == null;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if(!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            ChangeAlpha(objectRenderer, 0.5f);
        } else if(InventoryUI.Instance.IsDragging()) {
            StartConstructionPreview();
        }
    }
    
    public void StartConstructionPreview() {
        previewInstance = InventoryUI.Instance.StartDraggedOnModule();
        
        ChangeAlpha(previewInstance.GetComponent<Renderer>(), previewInstance.transform.parent == transform ? 1f : 0.75f);
        
        parentPreview = previewInstance.transform.parent;
        localPositionPreview = previewInstance.transform.localPosition;
        
        if(construction && previewInstance.transform.parent != transform) {
            VaisseauModule module = InventoryUI.Instance.GetDraggedModule();
            if(module) {
                construction.SetChildOf(transform);
            } else {
                construction.gameObject.SetActive(false);
            }
        }

        previewInstance.transform.SetParent(transform);
        previewInstance.transform.localPosition = new Vector3(0, 1, 0);
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        if(!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            if(Inventory.Instance.HaveEnoughRessources(coutRessources)) {
                BuildManager.Instance.CurrentConstruction(construction);
                Activate();
                Inventory.Instance.AddInventorySlotsSize();
            }
        } else if(BuildManager.Instance.InBuildMode()) {
            BuildManager.Instance.CurrentConstruction(construction);
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if(!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            ChangeAlpha(objectRenderer, 0.25f);
        } else if(InventoryUI.Instance.IsDragging()) {
            EndConstructionPreview();
        }
    }
    
    public void EndConstructionPreview() {
        InventoryUI.Instance.EndDraggedOnModule();
        
        ChangeAlpha(previewInstance.GetComponent<Renderer>(), alphaPreview);

        previewInstance.transform.SetParent(parentPreview);
        previewInstance.transform.localPosition = localPositionPreview;
        
        if(construction) {
            construction.SetChildOf(transform);
            construction.gameObject.SetActive(true);
        }
    }

    public void ToggleBuildMode(bool value) {
        if(!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            ToggleModulePreview(value);
        }
    }

    private void ToggleModulePreview(bool isPreview) {
        objectCollider.enabled = isPreview;
        objectRenderer.enabled = isPreview;
    }

    private void Activate() {
        isActivate = true;
        ChangeAlpha(objectRenderer, 1f);
        objectRenderer.enabled = true;
        objectCollider.enabled = true;
    }
    
    private void ChangeAlpha(Renderer rendererToChange, float alpha) {
        foreach (Material material in rendererToChange.materials) {
            Color color = material.color;
            color.a = alpha;
            rendererToChange.material.color = color;
        }
    }

    public void TakeDamage(int damage) {
        //DEBUG!!! Renvoie les dégâts au joueur ou les absorbe (à voir avec l'équipe) [WaitFor Vaisseau]
    }
}