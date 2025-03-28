using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Spaze {
    public class ConstructionInformationsUI : MonoBehaviour {
        public static ConstructionInformationsUI Instance;

        [SerializeField] private TextMeshProUGUI typeText;
        [SerializeField] private TextMeshProUGUI rarityText;
        [SerializeField] private TextMeshProUGUI niveauText;
        [SerializeField] private Transform container;
        [SerializeField] private TMP_FontAsset fontAsset;

        [SerializeField] private GameObject variableGraphique;

        /// <summary>
        /// M�thode appel�e lors de l'initialisation de l'objet. Initialise l'instance et cache l'UI.
        /// </summary>
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            HideHoverUI();
        }

        /// <summary>
        /// Affiche l'UI de survol avec les informations de la construction sp�cifi�e.
        /// </summary>
        /// <param name="construction">La construction dont les informations doivent �tre affich�es.</param>
        public void ShowHoverUI(Construction construction) {
            typeText.text = construction.GetType().Name;
            rarityText.text = construction.GetRarityText();
            rarityText.color = construction.GetRarityColor();
            niveauText.text = $"{construction.GetNiveau()}/5";

            foreach (Transform child in container) {
                Destroy(child.gameObject);
            }
            foreach (var stat in construction.GetStats()) {
                GameObject statText = Instantiate(variableGraphique, container);
                statText.GetComponentInChildren<Image>().sprite = stat.Key;
                statText.GetComponentInChildren<TextMeshProUGUI>().text = stat.Value;
            }

            gameObject.SetActive(true);
        }

        /// <summary>
        /// Cache l'UI de survol.
        /// </summary>
        public void HideHoverUI() {
            gameObject.SetActive(false);
        }
    }
}