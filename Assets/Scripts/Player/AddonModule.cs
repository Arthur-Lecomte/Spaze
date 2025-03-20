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
    private Construction construction;
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

        ChangeAlpha(0.25f);
    }

    private void Start() {
        objectCollider.enabled = false;
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
            //DEBUG!!! afficher over avec prix
            ChangeAlpha(0.5f);
        }
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (!isActivate) {
            if (Inventory.Instance.HaveEnoughRessources(coutRessources)) {
                //DEBUG!!! Demander une verification
                //DEBUG!!! enlever over avec prix si acheté
                
                Inventory.Instance.RemoveRessources(coutRessources);

                isActivate = true;
                ChangeAlpha(1f);
                objectRenderer.enabled = true;
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
            //DEBUG!!! enlever over avec prix
            ChangeAlpha(0.25f);
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
    
    public GameObject WhoAmI() {
        return Vaisseau.Instance.gameObject;
    }
}
