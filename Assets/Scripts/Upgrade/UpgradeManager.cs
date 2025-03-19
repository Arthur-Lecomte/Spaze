using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    public static UpgradeManager Instance;
    [SerializeField] private GameObject panelInventory;

    private int pointsLevel;
    [SerializeField] private TextMeshProUGUI pointsText;
    
    private AddonModule[] allModules;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        allModules = FindObjectsByType<AddonModule>(FindObjectsSortMode.None);
        gameObject.SetActive(false);
    }
    
    public void DisplayUpgrade(bool value) {
        gameObject.SetActive(value);
        panelInventory.SetActive(value); //Debug!!! Créer un faux inventaire pour acheter de nouveau slot
        foreach(AddonModule module in allModules) {
            module.DisplayModule(value);
        }
    }

    //DEBUG!!! Récupérer le point après un certain temps
    public void AddPoints() {
        pointsLevel += 1;
        pointsText.text = "Points: " + pointsLevel;
    }

    public bool CanUpgrade() {
        if(pointsLevel >= 1) {
            pointsLevel -= 1;
            pointsText.text = "Points: " + pointsLevel;
            return true;
        }
        return false;
    }
}

public enum TypeUpgrade {
    Attack,
    Shield,
    Speed,
    Extraction
}
