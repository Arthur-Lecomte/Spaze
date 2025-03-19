using UnityEngine;
using System.Collections.Generic;

public class Radar : Construction {
    [SerializeField] private float range;
    
    public float Range => range;
    
    public override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Range", range.ToString("F2"));
        return stats;
    }
}
