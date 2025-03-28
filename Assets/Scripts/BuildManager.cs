using UnityEngine;
using UnityEngine.InputSystem;
namespace Spaze {
    public class BuildManager : MonoBehaviour {

        [SerializeField] public GameObject canvasParametre; // Référence du canvas

        public InputActionReference toggleMenuPause; // Référence de la touche

        /// <summary>
        /// Méthode appelée lorsque l'objet devient actif. Active l'action de basculement du menu pause.
        /// </summary>
        void OnEnable() {
            toggleMenuPause.action.performed += ToggleMenuPause;
            toggleMenuPause.action.Enable();
        }

        /// <summary>
        /// Méthode appelée lorsque l'objet devient inactif. Désactive l'action de basculement du menu pause.
        /// </summary>
        void OnDisable() {
            toggleMenuPause.action.performed -= ToggleMenuPause;
            toggleMenuPause.action.Disable();
        }

        /// <summary>
        /// Bascule l'état d'affichage du menu pause.
        /// </summary>
        /// <param name="obj">Le contexte de l'action de rappel.</param>
        private void ToggleMenuPause(InputAction.CallbackContext obj) {
            canvasParametre.SetActive(!canvasParametre.activeSelf);
            Time.timeScale = canvasParametre.activeSelf ? 0 : 1; 
        }
    }
}