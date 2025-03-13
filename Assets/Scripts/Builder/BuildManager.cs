using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

/// <summary>
/// Classe responsable de la gestion du mode construction.
/// </summary>
public class BuildManager : MonoBehaviour {
    public static BuildManager Instance;

    public InputActionReference toggleBuildModeAction; //Référence de la touche
    private bool isInBuildMode;
    private VaisseauModule[] allModules;
    private Construction currentConstruction;

    [SerializeField] private GameObject[] panels;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        allModules = FindObjectsByType<VaisseauModule>(FindObjectsSortMode.None);
    }

    private void Start() {
        ToggleBuildMode();
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
        foreach(GameObject panel in panels) {
            panel.SetActive(isInBuildMode);
        }

        //panelDescription.SetActive(currentConstruction && isInBuildMode); //DEBUG!!! à créer
        foreach(VaisseauModule module in allModules) {
            module.ToggleBuildMode(isInBuildMode);
        }
    }

    public void CurrentConstruction(Construction construction) {
        currentConstruction = construction;
        //panelDescription.SetActive(currentConstruction && isInBuildMode); //DEBUG!!! à créer
    }
}
