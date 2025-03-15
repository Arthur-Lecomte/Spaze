using UnityEngine;

public class Radar : Construction {
    [SerializeField] private float range;
    
    public float Range => range;
    
    public override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
