using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddonModule : Module, ICanTakeDamage, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler {
    [SerializeField] private List<Ressource> coutRessources;
    private bool isActivate;

    private Collider objectCollider;
    private Renderer objectRenderer;
    private Construction construction;
    private Image constructionImage;

    private GameObject previewInstance;
    private Transform parentPreview;
    private Vector3 localPositionPreview;
    private float alphaPreview;
    
    public UnityEvent<Construction> onAddConstruction;
    public UnityEvent<bool> onBuyModule;

    protected override void Awake() {
        base.Awake();
        objectCollider = GetComponent<Collider>();
        objectRenderer = GetComponent<Renderer>();
        ChangeAlpha(objectRenderer, 0.25f);
    }

    private void Start() {
        objectCollider.enabled = false;
        objectRenderer.enabled = false;
    }

    public bool IsActivate() {
        return isActivate;
    }

    public void SetConstruction(Construction c) {
        construction = c;
        if (construction) {
            construction.SetChildOf(transform);
        }
    }

    public Construction GetConstruction() {
        return construction;
    }

    public bool IsEmpty() {
        return construction == null;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            ChangeAlpha(objectRenderer, 0.5f);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            if (Inventory.Instance.HaveEnoughRessources(coutRessources)) {
                BuildManager.Instance.CurrentConstruction(construction);
                
                isActivate = true;
                ChangeAlpha(objectRenderer, 1f);
                objectRenderer.enabled = true;
                objectCollider.enabled = true;
                
                CanActivateNeighbors();
                foreach (Module module in Neighbors) {
                    module.ToggleBuildMode(true);
                }
                
                onBuyModule.Invoke(BuildManager.Instance.InBuildMode());
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!isActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            ChangeAlpha(objectRenderer, 0.25f);
        }
    }

    public override void ToggleBuildMode(bool value) {
        //Debug.Log(name + " " + value + " " + isActivate + " " + CanBeActivate);
        if (!isActivate && CanBeActivate) {
            //DEBUG!!! Vérifier si proche d'un shop [WaitforShopManager]
            objectCollider.enabled = value;
            objectRenderer.enabled = value;
        }
    }

    private void ChangeAlpha(Renderer rendererToChange, float alpha) {
        for (int i = 0; i < rendererToChange.materials.Length; i++) {
            Material material = rendererToChange.materials[i];

            Color color = material.color;
            color.a = alpha;
            rendererToChange.materials[i].color = color;
        }
    }

    public void TakeDamage(float damage) {
        //DEBUG!!! Renvoie les dégâts au joueur ou les absorbe (à voir avec l'équipe) [WaitFor Vaisseau]
    }
}
