using SmallHedge.SoundManager;
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
    
    private void Start() {
        level = 2;
        SendInformation();
    }
    
    public void Add() {
        if (level < 5) {
            if(UpgradeManager.Instance.CanUpgrade()) {
                level++;
                SoundManager.PlaySound(SoundType.CLICK);
                SendInformation();
            }
        }
    }
    
    public void Remove() {
        if (level > 1) {
            level--;
            UpgradeManager.Instance.AddPoints();
            SoundManager.PlaySound(SoundType.CLICK);
            SendInformation();
        }
    }
    
    private void SendInformation() {
        switch (type) {
            case TypeUpgrade.Speed:
                Vaisseau.Instance.SetSpeedSkillPercentage(level/5f);
                break;
        }
        
        image5.color = level >= 1 ? Color.green : Color.red;
        image4.color = level >= 2 ? Color.green : Color.red;
        image3.color = level >= 3 ? Color.green : Color.red;
        image2.color = level >= 4 ? Color.green : Color.red;
        image1.color = level >= 5 ? Color.green : Color.red;
    }
}
