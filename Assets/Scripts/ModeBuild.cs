using UnityEngine;
using TMPro;

public class ModeBuild : MonoBehaviour {
    public static ModeBuild Instance;
    public int nbResource = 1;
    public float pourcentageResource = 0.4f;

    public bool isInBuildMode;
    
    public int resourceValue;
    public TextMeshProUGUI resourceText;
    public Composant currentComposant;
    public GameObject panelBuild;
    public GameObject panelUpgrade;

    void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        resourceValue = 20;
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
    
    public void ActivateBuildMode() {
        isInBuildMode = !isInBuildMode;
        CurrentComposant(null);
        panelUpgrade.SetActive(isInBuildMode);
        Composant[] composants = FindObjectsByType<Composant>(FindObjectsSortMode.None);
        foreach (Composant composant in composants) {
            composant.ActivateBuildMode(isInBuildMode);
        }
    }

    public void CurrentComposant(Composant composant) {
        currentComposant = composant;
        panelBuild.SetActive(currentComposant);
    }

    public void ChangeUpgrade(TypeUpgrade type, float value) {
        value *= 0.2f;
        switch (type) {
            case TypeUpgrade.Health:
                PlayerMove.Instance.UpgradeLife(value);
                break;
            case TypeUpgrade.Damage:
                PlayerMove.Instance.UpgradeDamage(value);
                break;
            case TypeUpgrade.Speed:
                PlayerMove.Instance.UpgradeSpeed(value);
                break;
            case TypeUpgrade.Extraction:
                pourcentageResource = value;
                break;
        }
    }

    public void ChangeComponent(int type) {
        if (HasEnoughResource(5)) {
            ChangeResource(-5);
            currentComposant.ChangeComposant(type);
        }
        
    }
}