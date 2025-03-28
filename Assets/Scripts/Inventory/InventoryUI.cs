using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
namespace Spaze {
    public class InventoryUI : MonoBehaviour {
        public static InventoryUI Instance;

        private List<InventorySlot> inventorySlots;
        [SerializeField] private int numberSlotsMax = 6;
        private int numberSlotsMemory;
        private InventorySlot draggedSlot;

        [SerializeField] private GameObject slotPrefab;
        private RectTransform rectTransform;
        [SerializeField] private GameObject addSlotPrefab;
        [SerializeField] private RectTransform panelAddSlots;

        [SerializeField] private GameObject dragImagePrefab;
        private GameObject dragImage;
        private Image dragImageComponent;

        /// <summary>
        /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
        /// </summary>
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            rectTransform = GetComponent<RectTransform>();
            inventorySlots = new List<InventorySlot>();

            dragImage = Instantiate(dragImagePrefab, transform);
            dragImageComponent = dragImage.GetComponent<Image>();
            dragImageComponent.color = new Color(1, 1, 1, 0.5f);
            dragImage.SetActive(false);
            DisplayInventory(false);
        }

        /// <summary>
        /// Affiche ou masque l'inventaire.
        /// </summary>
        /// <param name="value">True pour afficher, False pour masquer.</param>
        public void DisplayInventory(bool value) {
            gameObject.SetActive(value);
            foreach (InventorySlot slot in inventorySlots) {
                slot.SetActive(value);
            }
            dragImage.SetActive(false);
        }

        /// <summary>
        /// Ajoute un slot d'inventaire à la liste des slots.
        /// </summary>
        /// <param name="inventorySlot">Le slot d'inventaire à ajouter.</param>
        public void AddInventorySlots(InventorySlot inventorySlot) {
            inventorySlots.Add(inventorySlot);
        }

        /// <summary>
        /// Change le nombre de slots d'inventaire affichés.
        /// </summary>
        /// <param name="numberSlots">Le nouveau nombre de slots.</param>
        public void ChangeNumberSlots(int numberSlots) {
            int columns = (numberSlots + 1) / 2;
            rectTransform.sizeDelta = new Vector2(15 + 105 * columns, 250);
            rectTransform.anchoredPosition = new Vector2(rectTransform.sizeDelta.x / 2 + 25, 150);
            for (int i = numberSlotsMemory + 1; i < numberSlots + 1; i++) {
                int column = (i + 1) / 2;
                GameObject slot = Instantiate(slotPrefab, rectTransform);
                RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
                slotRectTransform.anchoredPosition = new Vector3(-45 + 105 * column, -65 + 105 * (i % 2), 0);

                InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
                inventorySlot.Initialisation(i - 1);
                inventorySlots.Add(inventorySlot);
            }

            // Limite de nombre de slots possible
            if (numberSlots >= numberSlotsMax) return;

            // AddSlotsInventory (pour acheter des slots)
            int columnsAddSlots = (numberSlots + 2) / 2;
            panelAddSlots.sizeDelta = new Vector2(15 + 105 * columnsAddSlots, 250);
            panelAddSlots.anchoredPosition = new Vector2(panelAddSlots.sizeDelta.x / 2 + 25, 150);
            int numberSlotsMemoryAddSlots = numberSlotsMemory == 0 ? 1 : numberSlotsMemory + 2;
            for (int i = numberSlotsMemoryAddSlots; i < numberSlots + 2; i++) {
                int column = (i + 1) / 2;
                GameObject slot = Instantiate(addSlotPrefab, panelAddSlots);
                RectTransform slotRectTransform = slot.GetComponent<RectTransform>();
                slotRectTransform.anchoredPosition = new Vector3(-45 + 105 * column, -65 + 105 * (i % 2), 0);

                InventoryAddSlot inventoryAddSlot = slot.GetComponent<InventoryAddSlot>();
                inventoryAddSlot.Initialisation();
                inventoryAddSlot.IsBuy(i != numberSlots + 1);
            }

            numberSlotsMemory = numberSlots;
        }

        /// <summary>
        /// Définit la construction à l'emplacement spécifié de l'inventaire.
        /// </summary>
        /// <param name="index">L'index de l'emplacement.</param>
        /// <param name="construction">La construction à ajouter.</param>
        public void SetInventorySlot(int index, Construction construction) {
            inventorySlots.Find(slot => slot.GetIndex() == index).SetGameObject(construction);
        }

        /// <summary>
        /// Met à jour l'inventaire lorsque la construction est améliorée.
        /// </summary>
        /// <param name="construction">La construction améliorée.</param>
        public void ConstructionHaveUpdate(Construction construction) {
            List<InventorySlot> slots = new List<InventorySlot>();
            foreach (InventorySlot slot in inventorySlots) {
                if (slot.GetConstruction() != null && slot.GetConstruction().IsSameConstruction(construction)) {
                    slots.Add(slot);
                }
            }
            if (slots.Count == 2) {
                InventorySlot toUpgrade;
                InventorySlot toDestroy;
                if (slots[0].GetIndex() == slots[1].GetIndex()) {
                    if (slots[0].GetConstruction() != construction) {
                        toUpgrade = slots[0];
                        toDestroy = slots[1];
                    } else {
                        toUpgrade = slots[1];
                        toDestroy = slots[0];
                    }
                } else {
                    if (slots[0].GetIndex() < slots[1].GetIndex()) {
                        toUpgrade = slots[0];
                        toDestroy = slots[1];
                    } else {
                        toUpgrade = slots[1];
                        toDestroy = slots[0];
                    }
                }
                Destroy(toDestroy.GetConstruction().gameObject);
                toDestroy.SetGameObject(null);
                toUpgrade.GetConstruction().Upgrade();
            }

            foreach (InventorySlot slot in inventorySlots) {
                slot.SetCorrectNiveau();
            }
        }

        /// <summary>
        /// Méthode appelée lorsque le pointeur clique sur un slot d'inventaire.
        /// </summary>
        /// <param name="eventData">Les données de l'événement du pointeur.</param>
        /// <param name="inventorySlot">Le slot d'inventaire cliqué.</param>
        public void OnPointerDown(PointerEventData eventData, InventorySlot inventorySlot) {
            if (inventorySlot.GetConstruction() == null) return;
            draggedSlot = inventorySlot;

            dragImageComponent.sprite = inventorySlot.GetSprite();
            dragImage.transform.position = eventData.position;
            dragImage.transform.SetAsLastSibling();
            dragImage.SetActive(true);
        }

        /// <summary>
        /// Méthode appelée lors du glissement du pointeur.
        /// </summary>
        /// <param name="eventData">Les données de l'événement du pointeur.</param>
        public void OnDrag(PointerEventData eventData) {
            if (dragImage.activeSelf) {
                dragImage.transform.position = eventData.position;
            }
        }

        /// <summary>
        /// Méthode appelée lorsque le pointeur est relâché.
        /// </summary>
        /// <param name="eventData">Les données de l'événement du pointeur.</param>
        public void OnPointerUp(PointerEventData eventData) {
            if (dragImage.activeSelf) {
                dragImage.SetActive(false);

                PointerEventData pointerEventData = new PointerEventData(EventSystem.current) {
                    position = eventData.position
                };

                List<RaycastResult> raycastResults = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerEventData, raycastResults);

                foreach (RaycastResult result in raycastResults) {
                    InventorySlot targetSlot = result.gameObject.GetComponent<InventorySlot>();
                    if (inventorySlots.Contains(targetSlot) && draggedSlot != targetSlot) {
                        draggedSlot.Exchange(targetSlot);
                        break;
                    }
                }
            }
        }
    }
}