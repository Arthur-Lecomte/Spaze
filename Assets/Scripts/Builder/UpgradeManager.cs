using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour {
    public static UpgradeManager Instance;

    private int pointsLevel;
    [SerializeField] private TextMeshProUGUI pointsText;

    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
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
