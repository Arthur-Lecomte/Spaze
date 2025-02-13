using UnityEngine;

public class UpgradeColonne : MonoBehaviour {
    public int value;
    public TypeUpgrade type;
    private Upgrade[] upgrades;
    
    public void Start() {
        value = 2;
        upgrades = GetComponentsInChildren<Upgrade>();
        foreach(Upgrade upgrade in upgrades) {
            upgrade.NewValue(value);
        }
    }
    
    public void Add() {
        value = Mathf.Min(value + 1, 5);
        foreach(Upgrade upgrade in upgrades) {
            upgrade.NewValue(value);
        }
        ModeBuild.Instance.ChangeUpgrade(type, value);
    }
    
    public void Remove() {
        value = Mathf.Max(value - 1, 1);
        foreach(Upgrade upgrade in upgrades) {
            upgrade.NewValue(value);
        }
        ModeBuild.Instance.ChangeUpgrade(type, value);
    }
}

public enum TypeUpgrade {
    Health,
    Damage,
    Speed,
    Extraction
}
