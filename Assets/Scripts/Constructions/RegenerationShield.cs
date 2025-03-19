using UnityEngine;
using System.Collections.Generic;

public class RegenerationShield : Construction {
    [SerializeField] private float speed;
    
    public float Speed => speed;
    
    public override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Regeneration Speed", speed.ToString("F2"));

        return stats;
    }
}
