using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Classe responsable de la gestion du mode construction.
/// </summary>
public class BuildManager : MonoBehaviour {
    public static BuildManager Instance;

    public InputActionReference toggleBuildModeAction; //Référence de la touche
    private bool isInBuildMode;
    private Composant[] allComposants;
    private Composant currentComposant;
    
    [SerializeField] private GameObject panelUpgrade;
    
    private void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        isInBuildMode = false;
        
        allComposants = FindObjectsByType<Composant>(FindObjectsSortMode.None);
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
    /// Indique si le mode construction est activé.
    /// </summary>
    public bool InBuildMode() {
        return isInBuildMode;
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
        //DEBUG!!! Voir les conditions pour l'activer (proche d'un shop ?) [WaitFor ShopManager]
        isInBuildMode = !isInBuildMode;
        
        //On affiche les différents panels
        panelUpgrade.SetActive(isInBuildMode);
        //panelComposant.SetActive(currentComposant && isInBuildMode); //DEBUG!!! à créer
        foreach (Composant composant in allComposants) {
            composant.ToggleBuildMode(isInBuildMode);
        }
        
        if (isInBuildMode) {
            Debug.Log("Build Mode On");
        } else {
            Debug.Log("Build Mode Off");
        }
    }
    
    public void CurrentComposant(Composant composant) {
        currentComposant = composant;
        //panelComposant.SetActive(currentComposant && isInBuildMode); //DEBUG!!! à créer
    }
}
