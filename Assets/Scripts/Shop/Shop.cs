using UnityEngine;
using UnityEngine.EventSystems;
namespace Spaze {
    public class Shop : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler {
        private Outline outline;
        private bool mouseOn;
        private bool isInShopRange;

        /// <summary>
        /// Méthode appelée au démarrage. Initialise les composants nécessaires.
        /// </summary>
        private void Start() {
            outline = GetComponent<Outline>();
        }

        /// <summary>
        /// Définit si le joueur est à portée du magasin et met à jour l'état du contour.
        /// </summary>
        /// <param name="value">True si le joueur est à portée, sinon False.</param>
        public void SetIsInShopRange(bool value) {
            isInShopRange = value;
            CheckOutline();

            if (!isInShopRange && ShopManager.Instance.IsShopOpen()) {
                ShopManager.Instance.ChangeShopOption(0, this);
            }
        }

        /// <summary>
        /// Méthode appelée lorsque le pointeur entre dans le magasin.
        /// </summary>
        /// <param name="eventData">Les données de l'événement du pointeur.</param>
        public void OnPointerEnter(PointerEventData eventData) {
            mouseOn = true;
            CheckOutline();
        }

        /// <summary>
        /// Méthode appelée lorsque le pointeur sort du magasin.
        /// </summary>
        /// <param name="eventData">Les données de l'événement du pointeur.</param>
        public void OnPointerExit(PointerEventData eventData) {
            mouseOn = false;
            CheckOutline();
        }

        /// <summary>
        /// Vérifie et met à jour l'état du contour en fonction de la portée et de la position du pointeur.
        /// </summary>
        private void CheckOutline() {
            outline.enabled = isInShopRange && mouseOn && !ShopManager.Instance.IsShopOpen();
        }

        /// <summary>
        /// Méthode appelée lorsque le pointeur clique sur le magasin.
        /// </summary>
        /// <param name="eventData">Les données de l'événement du pointeur.</param>
        public void OnPointerDown(PointerEventData eventData) {
            if (isInShopRange && !ShopManager.Instance.IsShopOpen()) {
                ShopManager.Instance.ChangeShopOption(-1, this);
            }
        }
    }
}