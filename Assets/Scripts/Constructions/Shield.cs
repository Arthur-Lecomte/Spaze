using UnityEngine;

public class Shield : Construction {
    [SerializeField] private int quantity;
    
    public int Quantity => quantity;
    
    public override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
