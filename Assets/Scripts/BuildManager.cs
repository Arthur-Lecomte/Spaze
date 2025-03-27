using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : MonoBehaviour {
    
    [SerializeField] public GameObject canvasParametre; //Référence du canvas

    public InputActionReference toggleMenuPause; //Référence de la touche

    void OnEnable() {
        toggleMenuPause.action.performed += ToggleMenuPause;
        toggleMenuPause.action.Enable();
    }

    void OnDisable() {
        toggleMenuPause.action.performed -= ToggleMenuPause;
        toggleMenuPause.action.Disable();
    }
    
    private void ToggleMenuPause(InputAction.CallbackContext obj) {
        canvasParametre.SetActive(!canvasParametre.activeSelf);
    }
}
