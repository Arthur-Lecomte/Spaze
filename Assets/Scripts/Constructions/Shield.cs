using UnityEngine;
using System.Collections.Generic;

public class Shield : Construction {
    [SerializeField] private int quantity;
    
    public int Quantity => quantity;
    
    public override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Shield Quantity", quantity.ToString());
        return stats;
    }
}
