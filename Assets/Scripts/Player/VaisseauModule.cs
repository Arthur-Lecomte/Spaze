using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VaisseauModule : MonoBehaviour {
    private Vaisseau vaisseau;

    private Dictionary<TypeRessource, int> coutRessources;
    [SerializeField] private bool isActivate;
    private EventTrigger eventTrigger;

    private Collider objectCollider;
    private Renderer objectRenderer;
    private Color color;
    private Construction construction;
    
    private GameObject previewInstance;
    private Transform parentPreview;
    private Vector3 localPositionPreview;
    private float alphaPreview;

    void Awake() {
        objectCollider = GetComponent<Collider>();
        objectRenderer = GetComponent<Renderer>();
        color = objectRenderer.material.color;

        if(!isActivate) {
            objectCollider.enabled = false;
            objectRenderer.enabled = false;
            color.a = 0.25f;
            objectRenderer.material.color = color;
        }

        vaisseau = transform.parent.GetComponent<Vaisseau>();
        //DEBUG!!! Initialiser les ressources nécessaires pour construire le module
    }

    public bool IsActivate() {
        return isActivate;
    }

    public void SetConstruction(Construction c) {
        construction = c;
        if(construction) {
            Transform constructionTransform = construction.transform;
            constructionTransform.SetParent(transform);
            constructionTransform.localPosition = new Vector3(0, 1, 0);
            EnablePointerHandlers();
        } else {
            DisablePointerHandlers();
        }
    }

    private void EnablePointerHandlers() {
        if(eventTrigger == null) {
            eventTrigger = gameObject.AddComponent<EventTrigger>();
            InventoryUI.Instance.AddEventTrigger(eventTrigger, EventTriggerType.PointerDown, (data) => InventoryUI.Instance.OnPointerDownModule(this, Input.mousePosition));
            InventoryUI.Instance.AddEventTrigger(eventTrigger, EventTriggerType.Drag, (data) => InventoryUI.Instance.OnDrag((PointerEventData)data));
            InventoryUI.Instance.AddEventTrigger(eventTrigger, EventTriggerType.PointerUp, (data) => InventoryUI.Instance.OnPointerUp((PointerEventData)data));
        }
    }

    private void DisablePointerHandlers() {
        if(eventTrigger != null) {
            Destroy(eventTrigger);
            eventTrigger = null;
        }
    }

    public Construction GetConstruction() {
        return construction;
    }

    public bool IsEmpty() {
        return construction == null;
    }

    private void OnMouseEnter() {
        if(!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            color.a = 0.5f;
            objectRenderer.material.color = color;
        } else if(InventoryUI.Instance.IsDragging()) {
            StartConstructionPreview();
        }
    }
    
    public void StartConstructionPreview() {
        previewInstance = InventoryUI.Instance.StartDraggedOnModule();
        
        Renderer rendererPreview = previewInstance.GetComponent<Renderer>();
        if (rendererPreview != null) {
            Color previewColor = rendererPreview.material.color;
            alphaPreview = previewColor.a;
            previewColor.a = previewInstance.transform.parent == transform ? 1f : 0.75f;
            rendererPreview.material.color = previewColor;
        }
        
        parentPreview = previewInstance.transform.parent;
        localPositionPreview = previewInstance.transform.localPosition;
        
        if(construction && previewInstance.transform.parent != transform) {
            VaisseauModule module = InventoryUI.Instance.GetDraggedModule();
            if(module) {
                construction.transform.SetParent(module.transform);
                construction.transform.localPosition = new Vector3(0, 1, 0);
            } else {
                construction.gameObject.SetActive(false);
            }
        }

        previewInstance.transform.SetParent(transform);
        previewInstance.transform.localPosition = new Vector3(0, 1, 0);
    }
    
    private void OnMouseDown() {
        if(!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            if(vaisseau.inventory.HaveEnoughRessources(coutRessources)) {
                BuildManager.Instance.CurrentConstruction(construction);

                Activate();
            }
        } else if(BuildManager.Instance.InBuildMode()) {
            BuildManager.Instance.CurrentConstruction(construction);
        }
    }

    private void OnMouseExit() {
        if(!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            color.a = 0.25f;
            objectRenderer.material.color = color;
        } else if(InventoryUI.Instance.IsDragging()) {
            EndConstructionPreview();
        }
    }
    
    public void EndConstructionPreview() {
        InventoryUI.Instance.EndDraggedOnModule();
        
        Renderer rendererPreview = previewInstance.GetComponent<Renderer>();
        if(rendererPreview != null) {
            Color previewColor = rendererPreview.material.color;
            previewColor.a = alphaPreview;
            rendererPreview.material.color = previewColor;
        }

        previewInstance.transform.SetParent(parentPreview);
        previewInstance.transform.localPosition = localPositionPreview;
        
        if(construction) {
            construction.transform.SetParent(transform);
            construction.transform.localPosition = new Vector3(0, 1, 0);
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
        color.a = 1f;
        objectRenderer.material.color = color;
        objectRenderer.enabled = true;
        objectCollider.enabled = true;
    }

    public void TakeDamage(int damage) {
        //DEBUG!!! Renvoie les dégâts au joueur ou les absorbe (à voir avec l'équipe) [WaitFor Vaisseau]
    }
}