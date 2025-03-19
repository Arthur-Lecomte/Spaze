using UnityEngine;

public class Extracteur : Construction {
    [SerializeField] private float speed;
    [SerializeField] private float range;
    
    public float Speed => speed;
    public float Range => range;
    
    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
