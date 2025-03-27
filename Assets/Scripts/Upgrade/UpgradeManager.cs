using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    public static UpgradeManager Instance;
    [SerializeField] private GameObject panelInventory;
    [SerializeField] private BuyHealth buyHealth;

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
    }
    
    private void Start() {
        DisplayUpgrade(false);
    }
    
    public void DisplayUpgrade(bool value) {
        gameObject.SetActive(value);
        panelInventory.SetActive(value);
        buyHealth.Activate(value);
        BuyHoverUI.Instance.HideHoverUI();
        foreach(AddonModule module in allModules) {
            module.DisplayModule(value);
        }
    }
    
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
