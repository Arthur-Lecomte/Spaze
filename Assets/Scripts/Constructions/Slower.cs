using UnityEngine;

public class Slower : Construction {
    [SerializeField] private float range;
    [SerializeField] private float speed;
    
    public float Range => range;
    public float Speed => speed;
    
    protected override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
