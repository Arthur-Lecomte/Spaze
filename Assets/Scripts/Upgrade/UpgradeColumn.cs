using UnityEngine;
using UnityEngine.UI;

public class UpgradeColumn : MonoBehaviour {
    [SerializeField] private TypeUpgrade type;
    
    [SerializeField] private Image image1;
    [SerializeField] private Image image2;
    [SerializeField] private Image image3;
    [SerializeField] private Image image4;
    [SerializeField] private Image image5;
    
    private int level;
    
    private void Awake() {
        level = 2;
        UpdateImages();
    }
    
    //DEBUG!!! Rendre Add et Remove "lent" (ne pas améliorer instantanément)
    public void Add() {
        if (level < 5) {
            if(UpgradeManager.Instance.CanUpgrade()) {
                level++;
                Vaisseau.Instance.UpgradePointsChanged(type, level);
                UpdateImages();
            }
        }
    }
    
    public void Remove() {
        if (level > 1) {
            level--;
            UpgradeManager.Instance.AddPoints();
            Vaisseau.Instance.UpgradePointsChanged(type, level);
            UpdateImages();
        }
    }
    
    private void UpdateImages() {
        image5.color = level >= 1 ? Color.green : Color.red;
        image4.color = level >= 2 ? Color.green : Color.red;
        image3.color = level >= 3 ? Color.green : Color.red;
        image2.color = level >= 4 ? Color.green : Color.red;
        image1.color = level >= 5 ? Color.green : Color.red;
    }
}
