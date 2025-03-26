using UnityEngine;

public class Speed : Construction {
    public static float AllPower { get; private set; }
    public static float AllMaxSpeed { get; private set; }
    private static float percent;
    
    [SerializeField] private float power;
    [SerializeField] private float maxSpeed;
    
    [SerializeField] private bool onAModule;

    public override void SetChildOf(Transform parent, bool onModule = true) {
        base.SetChildOf(parent, onModule);

        onAModule = onModule;
        
        OnPercentChanged(TypeUpgrade.Speed, percent);
    }
    
    protected override void PerformUpgrade() {
        OnPercentChanged(TypeUpgrade.Speed, percent);
    }
    
    private static void CalculSpeedBonus() {
        Speed[] speedComponents = Vaisseau.Instance.GetComponentsInChildren<Speed>();
        AllPower = 0;
        AllMaxSpeed = 0;
        foreach (Speed speed in speedComponents) {
            AllPower += speed.power;
            AllMaxSpeed += speed.maxSpeed;
        }
        
        AllPower *= percent;
        AllMaxSpeed *= percent;
    }
    
    protected override void OnPercentChanged(TypeUpgrade typeUpgrade, float value) {
        if (value == 0) {
            UpgradeColumn.getValue(typeUpgrade);
            return;
        }
        
        if (typeUpgrade == TypeUpgrade.Speed) {
            percent = value;
            CalculSpeedBonus();
        }
    }
}
