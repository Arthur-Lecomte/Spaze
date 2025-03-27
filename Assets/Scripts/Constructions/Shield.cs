using System.Collections.Generic;
using SmallHedge.SoundManager;
using UnityEngine;
using UnityEngine.UI;

public class Shield : SearchShield {
    [SerializeField] private float maxLife;
    [SerializeField] private float maxLifeWithPercent;
    private static float percent;
    [SerializeField] private float life;
    private float regeneration;
    private float range;

    [SerializeField] private Transform trigger;

    [SerializeField] private RawImage lifeBar;

    public float GetLife() {
        return life;
    }
    
    public float GetMaxLife() {
        return maxLifeWithPercent;
    }
    
    public float GetRegeneration() {
        return regeneration;
    }

    public List<SearchShield> GetNearbySearchShields() {
        return NearbySearchShields;
    }
    
    public void ActiveLaserBeam(bool active) {
        foreach (SearchShield searchShield in NearbySearchShields) {
            RegenerationShield rs = (RegenerationShield)searchShield;
            rs.ActiveLaserBeam(this, active);
        }
    }
    
    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);

        OnPercentChanged(TypeUpgrade.Shield, percent);
        ChangeLife(maxLifeWithPercent);
        
        trigger.localScale = Vector3.one * (range * 2 / ConstructionTransform.localScale.x);

        TypeToSearch = typeof(RegenerationShield);
    }
    
    public override void SetChildOf(Transform parent, bool onModule = true) {
        base.SetChildOf(parent, onModule);

        newShield?.Invoke();
    }

    public void ChangeLife(float quantity) {
        life = Mathf.Clamp(life + quantity, 0, maxLifeWithPercent);
        lifeBar.rectTransform.sizeDelta = new Vector2(life / maxLifeWithPercent * 100, 20);
    }

    protected override void PerformUpgrade() {
        OnPercentChanged(TypeUpgrade.Shield, percent);
        life = maxLifeWithPercent; //Régénère entièrement le shield en s'améliorant
        ChangeLife(0);
        trigger.localScale = Vector3.one * (range * 2 / ConstructionTransform.localScale.x); //Augmente la portée du shield
    }
    
    protected override void OnPercentChanged(TypeUpgrade typeUpgrade, float value) {
        if (value == 0) {
            UpgradeColumn.getValue(typeUpgrade);
            return;
        }
        
        if (typeUpgrade == TypeUpgrade.Shield) {
            percent = value;
            maxLifeWithPercent = maxLife * percent;
            ChangeLife(0);
        }
    }
}
