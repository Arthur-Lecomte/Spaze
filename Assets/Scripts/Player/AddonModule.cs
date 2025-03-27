using System.Collections.Generic;
using SmallHedge.SoundManager;
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

    /// <summary>
    /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
    /// </summary>
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

    /// <summary>
    /// Méthode appelée au démarrage. Initialise l'état du module.
    /// </summary>
    private void Start() {
        objectCollider.enabled = false;
        objectCollider.isTrigger = true;
        objectRenderer.enabled = false;
    }

    /// <summary>
    /// Vérifie si le module est activé.
    /// </summary>
    /// <returns>True si le module est activé, sinon False.</returns>
    public bool IsActivate() {
        return isActivate;
    }

    /// <summary>
    /// Affiche ou masque le module.
    /// </summary>
    /// <param name="value">True pour afficher, False pour masquer.</param>
    public override void DisplayModule(bool value) {
        if (!isActivate && CanBeActivate) {
            objectCollider.enabled = value;
            objectRenderer.enabled = value;
        }
    }

    /// <summary>
    /// Définit la construction pour ce module.
    /// </summary>
    /// <param name="c">La construction à définir.</param>
    public void SetConstruction(Construction c) {
        Construction = c;
        if (Construction) {
            Construction.SetChildOf(transform);
        }
    }

    /// <summary>
    /// Obtient la construction associée à ce module.
    /// </summary>
    /// <returns>La construction associée.</returns>
    public Construction GetConstruction() {
        return Construction;
    }

    /// <summary>
    /// Vérifie si le module est vide.
    /// </summary>
    /// <returns>True si le module est vide, sinon False.</returns>
    public bool IsEmpty() {
        return Construction == null;
    }

    /// <summary>
    /// Méthode appelée lorsque le pointeur entre dans le module.
    /// </summary>
    /// <param name="eventData">Les données de l'événement du pointeur.</param>
    public void OnPointerEnter(PointerEventData eventData) {
        if (!isActivate) {
            BuyHoverUI.Instance.ShowHoverUI(name, coutRessources);
            ChangeAlpha(0.5f);
        } else if (Construction) {
            ConstructionInformationsUI.Instance.ShowHoverUI(Construction);
        }
    }

    /// <summary>
    /// Méthode appelée lorsque le pointeur clique sur le module.
    /// </summary>
    /// <param name="eventData">Les données de l'événement du pointeur.</param>
    public void OnPointerDown(PointerEventData eventData) {
        if (!isActivate) {
            if (Inventory.Instance.HaveEnoughRessources(coutRessources)) {
                BuyHoverUI.Instance.HideHoverUI();
                Inventory.Instance.RemoveRessources(coutRessources);
                SoundManager.PlaySound(SoundType.BUY);

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

    /// <summary>
    /// Méthode appelée lorsque le pointeur sort du module.
    /// </summary>
    /// <param name="eventData">Les données de l'événement du pointeur.</param>
    public void OnPointerExit(PointerEventData eventData) {
        if (!isActivate) {
            BuyHoverUI.Instance.HideHoverUI();
            ChangeAlpha(0.25f);
        } else if (Construction) {
            ConstructionInformationsUI.Instance.HideHoverUI();
        }
    }

    /// <summary>
    /// Change l'alpha (transparence) des matériaux du module.
    /// </summary>
    /// <param name="alpha">La valeur de l'alpha à définir.</param>
    private void ChangeAlpha(float alpha) {
        for (int i = 0; i < colors.Count; i++) {
            Color color = colors[i];
            color.a = alpha;
            objectRenderer.materials[i].color = color;
        }
    }

    /// <summary>
    /// Applique des dégâts au module.
    /// </summary>
    /// <param name="damage">La quantité de dégâts à appliquer.</param>
    public void TakeDamage(float damage) {
        Vaisseau.Instance.TakeDamage(damage);
    }

    /// <summary>
    /// Indique si l'objet est contrôlé par le joueur.
    /// </summary>
    /// <returns>True car il s'agit d'un module du joueur.</returns>
    public bool AmIPlayer() {
        return true;
    }
}
