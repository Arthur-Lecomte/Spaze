using UnityEngine;

public class Speed : Construction {
    public static float AllPower { get; private set; }
    public static float AllMaxSpeed { get; private set; }
    
    [SerializeField] private float power;
    [SerializeField] private float maxSpeed;
    
    [SerializeField] private bool onAModule;

    public override void SetChildOf(Transform parent, bool onModule = true) {
        base.SetChildOf(parent, onModule);

        onAModule = onModule;
        CalculSpeedBonus();
    }
    
    private static void CalculSpeedBonus() {
        Speed[] speedComponents = Vaisseau.Instance.GetComponentsInChildren<Speed>();
        AllPower = 0;
        AllMaxSpeed = 0;
        foreach (Speed speed in speedComponents) {
            AllPower += speed.power;
            AllMaxSpeed += speed.maxSpeed;
        }
    }
}
