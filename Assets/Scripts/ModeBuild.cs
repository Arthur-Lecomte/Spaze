using UnityEngine;
using TMPro;

public class ModeBuild : MonoBehaviour {
    public static ModeBuild Instance;

    public bool isInBuildMode;
    
    public int resourceValue;
    public TextMeshProUGUI resourceText;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        UpdateResourceText();
    }

    void UpdateResourceText() {
        if (resourceText) {
            resourceText.text = resourceValue.ToString();
        }
    }

    public void ChangeResource(int amount) {
        resourceValue += amount;
        UpdateResourceText();
    }

    public bool HasEnoughResource(int amount) {
        return resourceValue >= amount;
    }
    
    public void ActivateAllPreviews() {
        isInBuildMode = !isInBuildMode;
        Composant[] composants = FindObjectsByType<Composant>(FindObjectsSortMode.None);
        foreach (Composant composant in composants) {
            composant.TogglePreview(isInBuildMode);
        }
    }
}