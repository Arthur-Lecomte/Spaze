using UnityEngine;
using System.Collections.Generic;

public class RegenerationShield : Construction {
    [SerializeField] private float speed;
    
    public float Speed => speed;
    
    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
    
    protected override void SetVariableForRarity(float multiplicator) {
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Regeneration Speed", speed.ToString("F2"));

        return stats;
    }
}
