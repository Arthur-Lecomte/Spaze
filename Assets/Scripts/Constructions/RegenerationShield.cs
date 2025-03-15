using UnityEngine;

public class RegenerationShield : Construction {
    [SerializeField] private float speed;
    
    public float Speed => speed;
    
    public override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
