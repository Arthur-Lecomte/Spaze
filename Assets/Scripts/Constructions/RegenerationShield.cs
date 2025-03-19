using UnityEngine;

public class RegenerationShield : Construction {
    [SerializeField] private float speed;
    
    public float Speed => speed;
    
    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
