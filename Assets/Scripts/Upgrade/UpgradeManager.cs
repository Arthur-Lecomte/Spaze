using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    public static UpgradeManager Instance;
    [SerializeField] private GameObject panelInventory;
    [SerializeField] private BuyHealth buyHealth;

    private int pointsLevel;
    [SerializeField] private TextMeshProUGUI pointsText;

    private AddonModule[] allModules;

    /// <summary>
    /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
    /// </summary>
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        allModules = FindObjectsByType<AddonModule>(FindObjectsSortMode.None);
    }

    /// <summary>
    /// Méthode appelée au démarrage. Masque l'interface de mise à niveau.
    /// </summary>
    private void Start() {
        DisplayUpgrade(false);
    }

    /// <summary>
    /// Affiche ou masque l'interface de mise à niveau.
    /// </summary>
    /// <param name="value">True pour afficher, False pour masquer.</param>
    public void DisplayUpgrade(bool value) {
        gameObject.SetActive(value);
        panelInventory.SetActive(value);
        buyHealth.Activate(value);
        BuyHoverUI.Instance.HideHoverUI();
        foreach (AddonModule module in allModules) {
            module.DisplayModule(value);
        }
    }

    /// <summary>
    /// Ajoute des points de mise à niveau.
    /// </summary>
    public void AddPoints() {
        pointsLevel += 1;
        pointsText.text = "Points: " + pointsLevel;
    }

    /// <summary>
    /// Vérifie si une mise à niveau est possible et retire un point de mise à niveau si c'est le cas.
    /// </summary>
    /// <returns>True si une mise à niveau est possible, sinon False.</returns>
    public bool CanUpgrade() {
        if (pointsLevel >= 1) {
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
