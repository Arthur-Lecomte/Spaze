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
                UpdateImages();
                if(type == TypeUpgrade.Speed) {
                    Vaisseau.Instance.ChangeSpeed(level);
                }
            }
        }
    }
    
    //DEBUG!!! Interdire de mettre à 0 ???
    public void Remove() {
        if (level > 0) {
            level--;
            UpgradeManager.Instance.AddPoints();
            UpdateImages();
            if(type == TypeUpgrade.Speed) {
                Vaisseau.Instance.ChangeSpeed(level);
            }
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
