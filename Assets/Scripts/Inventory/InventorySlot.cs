using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Spaze {
    public class InventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
        private Construction constructionObject;
        private Image image;
        private TextMeshProUGUI textLevel;
        private Image imageColorRarity;

        [SerializeField] private int index;
        private AddonModule module;

        /// <summary>
        /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
        /// </summary>
        private void Awake() {
            if (index == -1) {
                transform.parent.GetComponent<Canvas>().worldCamera = Camera.main;

                module = transform.parent.parent.GetComponent<AddonModule>();
                Initialisation(-1);
            }
        }

        /// <summary>
        /// Méthode appelée au démarrage. Initialise l'interface utilisateur de l'inventaire.
        /// </summary>
        private void Start() {
            if (index == -1) {
                InventoryUI.Instance.AddInventorySlots(this);
                module.onAddConstruction.AddListener(SetGameObject);
                SetActive(false);
            }
        }

        /// <summary>
        /// Initialise le slot d'inventaire avec l'index spécifié.
        /// </summary>
        /// <param name="i">L'index du slot.</param>
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

        /// <summary>
        /// Ajoute un événement au déclencheur d'événements.
        /// </summary>
        /// <param name="trigger">Le déclencheur d'événements.</param>
        /// <param name="eventType">Le type d'événement.</param>
        /// <param name="action">L'action à exécuter lors de l'événement.</param>
        private void AddEventTrigger(EventTrigger trigger, EventTriggerType eventType, System.Action<BaseEventData> action) {
            EventTrigger.Entry entry = new EventTrigger.Entry { eventID = eventType };
            entry.callback.AddListener((data) => action(data));
            trigger.triggers.Add(entry);
        }

        /// <summary>
        /// Définit l'objet de construction pour ce slot.
        /// </summary>
        /// <param name="construction">La construction à définir.</param>
        public void SetGameObject(Construction construction) {
            if (module) {
                module.SetConstruction(construction);
            }
            if (construction) {
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

        /// <summary>
        /// Définit le niveau correct pour l'affichage.
        /// </summary>
        public void SetCorrectNiveau() {
            textLevel.text = constructionObject ? constructionObject.GetNiveau() + "/5" : "";
        }

        /// <summary>
        /// Obtient l'objet de construction associé à ce slot.
        /// </summary>
        /// <returns>L'objet de construction.</returns>
        public Construction GetConstruction() {
            return constructionObject;
        }

        /// <summary>
        /// Obtient le sprite de l'image associée à ce slot.
        /// </summary>
        /// <returns>Le sprite de l'image.</returns>
        public Sprite GetSprite() {
            return image.sprite;
        }

        /// <summary>
        /// Obtient l'index de ce slot.
        /// </summary>
        /// <returns>L'index du slot.</returns>
        public int GetIndex() {
            return index;
        }

        /// <summary>
        /// Échange les objets de construction entre ce slot et un autre slot d'inventaire.
        /// </summary>
        /// <param name="inventorySlot">L'autre slot d'inventaire.</param>
        public void Exchange(InventorySlot inventorySlot) {
            SoundManager.PlaySound(SoundType.BUILDING);
            Construction construction1;
            Construction construction2;

            if (index == -1) {
                construction1 = module.GetConstruction();
                SetGameObject(null);
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

        /// <summary>
        /// Active ou désactive ce slot d'inventaire.
        /// </summary>
        /// <param name="value">True pour activer, False pour désactiver.</param>
        public void SetActive(bool value) {
            if (module) {
                gameObject.SetActive(value && module.IsActivate());
            }
        }

        /// <summary>
        /// Méthode appelée lorsque le pointeur entre dans ce slot d'inventaire.
        /// </summary>
        /// <param name="eventData">Les données de l'événement du pointeur.</param>
        public void OnPointerEnter(PointerEventData eventData) {
            if (constructionObject) {
                ConstructionInformationsUI.Instance.ShowHoverUI(constructionObject);
            }
        }

        /// <summary>
        /// Méthode appelée lorsque le pointeur sort de ce slot d'inventaire.
        /// </summary>
        /// <param name="eventData">Les données de l'événement du pointeur.</param>
        public void OnPointerExit(PointerEventData eventData) {
            if (constructionObject) {
                ConstructionInformationsUI.Instance.HideHoverUI();
            }
        }
    }
}