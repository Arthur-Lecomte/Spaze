using UnityEngine;

public class Radar : Construction {
    [SerializeField] private float range;
    
    public float Range => range;
    
    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
