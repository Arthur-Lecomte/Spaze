using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe responsable de la gestion du mode construction.
/// </summary>
public class BuildManager : MonoBehaviour {
    /// <summary>
    /// Instance singleton de BuildManager.
    /// </summary>
    public static BuildManager Instance;

    /// <summary>
    /// Référence à l'action d'entrée pour basculer le mode construction.
    /// </summary>
    public InputActionReference toggleBuildModeAction;

    /// <summary>
    /// Indique si le mode construction est activé.
    /// </summary>
    private bool isInBuildMode;
    
    private void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        isInBuildMode = false;
    }
    
    void OnEnable() {
        toggleBuildModeAction.action.performed += ToggleBuildMode;
        toggleBuildModeAction.action.Enable();
    }
    
    void OnDisable() {
        toggleBuildModeAction.action.performed -= ToggleBuildMode;
        toggleBuildModeAction.action.Disable();
    }

    /// <summary>
    /// Active/Desactive le monde construction via une touche du clavier.
    /// </summary>
    /// <param name="obj">Contexte de rappel de l'action d'entrée.</param>
    private void ToggleBuildMode(InputAction.CallbackContext obj) {
        ToggleBuildMode();
    }

    /// <summary>
    /// Active/Desactive le monde construction via un bouton sur l'UI.
    /// </summary>
    public void ToggleBuildMode() {
        isInBuildMode = !isInBuildMode;
        if (isInBuildMode) {
            Debug.Log("Build Mode On");
        } else {
            Debug.Log("Build Mode Off");
        }
    }
}
