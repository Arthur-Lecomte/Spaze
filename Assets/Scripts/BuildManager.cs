using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour {
    //DEBUG!!! Script gardé pour avoir une utilisation de InputActionReference sous la main. Ne pas utiliser et à supprimer du Canvas une fois le code copié

    public InputActionReference toggleBuildModeAction; //Référence de la touche

    void OnEnable() {
        toggleBuildModeAction.action.performed += ToggleBuildMode;
        toggleBuildModeAction.action.Enable();
    }

    void OnDisable() {
        toggleBuildModeAction.action.performed -= ToggleBuildMode;
        toggleBuildModeAction.action.Disable();
    }
    
    private void ToggleBuildMode(InputAction.CallbackContext obj) {
    }
}
