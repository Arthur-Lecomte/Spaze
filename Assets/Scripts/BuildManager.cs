using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour {
    //DEBUG!!! Script gardé pour avoir une utilisation de InputActionReference sous la main.
    //À supprimer du Canvas une fois le code copié

    public InputActionReference toggleBuildModeAction; //Référence de la touche

    void OnEnable() {
        toggleBuildModeAction.action.performed += ToggleBuildMode;
        toggleBuildModeAction.action.Enable();
    }

    void OnDisable() {
        toggleBuildModeAction.action.performed -= ToggleBuildMode;
        toggleBuildModeAction.action.Disable();
    }
    
    //DEBUG!!! Utilisation de ceci pour tester ShopOption
    int currentOption;
    private void ToggleBuildMode(InputAction.CallbackContext obj) {
        currentOption = (currentOption + 1) % 3;
        
        // Désactiver le panel des constructions achetables (à Baptiste de remplir)
        UpgradeManager.Instance.DisplayUpgrade(false);
        InventoryUI.Instance.DisplayInventory(false);

        switch(currentOption) {
            case 0:
                // Afficher le panel des constructions achetables (à Baptiste de remplir)
                break;
            case 1:
                UpgradeManager.Instance.DisplayUpgrade(true);
                break;
            case 2:
                InventoryUI.Instance.DisplayInventory(true);
                break;
        }
    }
}
