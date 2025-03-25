using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AddonModule : Module, ICanTakeDamage, IPointerEnterHandler, IPointerDownHandler, IPointerExitHandler {
    [SerializeField] private List<Ressource> coutRessources;
    private bool isActivate;

    private Collider objectCollider;
    private Renderer objectRenderer;
    private List<Color> colors;
    private Image constructionImage;
    private GameObject previewInstance;
    private Transform parentPreview;
    private Vector3 localPositionPreview;
    private float alphaPreview;

    public UnityEvent<Construction> onAddConstruction;

    protected override void Awake() {
        base.Awake();
        objectCollider = GetComponent<Collider>();
        objectRenderer = GetComponent<Renderer>();

        colors = new List<Color>();
        foreach (Material material in objectRenderer.materials) {
            colors.Add(material.color);
        }

        gameObject.name = "module";
        ChangeAlpha(0.25f);
    }

    private void Start() {
        objectCollider.enabled = false;
        objectCollider.isTrigger = true;
        objectRenderer.enabled = false;
    }

    public bool IsActivate() {
        return isActivate;
    }

    public override void DisplayModule(bool value) {
        if (!isActivate && CanBeActivate) {
            objectCollider.enabled = value;
            objectRenderer.enabled = value;
        }
    }

    public void SetConstruction(Construction c) {
        Construction = c;
        if (Construction) {
            Construction.SetChildOf(transform);
        }
    }

    public Construction GetConstruction() {
        return Construction;
    }

    public bool IsEmpty() {
        return Construction == null;
    }

    public void OnPointerEnter(PointerEventData eventData) {
        if (!isActivate) {
            BuyHoverUI.Instance.ShowHoverUI(name, coutRessources);
            ChangeAlpha(0.5f);
        } else if (Construction) {
            ConstructionInformationsUI.Instance.ShowHoverUI(Construction);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!isActivate) {
            if (Inventory.Instance.HaveEnoughRessources(coutRessources)) {
                BuyHoverUI.Instance.HideHoverUI();
                Inventory.Instance.RemoveRessources(coutRessources);

                isActivate = true;
                ChangeAlpha(1f);
                objectRenderer.enabled = true;
                objectCollider.isTrigger = false;
                objectCollider.enabled = true;

                CanActivateNeighbors();
                foreach (Module module in Neighbors) {
                    module.DisplayModule(true);
                }
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!isActivate) {
            BuyHoverUI.Instance.HideHoverUI();
            ChangeAlpha(0.25f);
        } else if (Construction) {
            ConstructionInformationsUI.Instance.HideHoverUI();
        }
    }

    private void ChangeAlpha(float alpha) {
        for (int i = 0; i < colors.Count; i++) {
            Color color = colors[i];
            color.a = alpha;
            objectRenderer.materials[i].color = color;
        }
    }

    public void TakeDamage(float damage) {
        Vaisseau.Instance.TakeDamage(damage);
    }
    
    public bool AmIPlayer() {
        return true;
    }
}
