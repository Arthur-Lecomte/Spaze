using UnityEngine;
using System.Collections.Generic;

public class Slower : Construction {
    [SerializeField] private float range;
    [SerializeField] private float speed;
    
    public float Range => range;
    public float Speed => speed;
    
    public override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Range", range.ToString("F2"));
        stats.Add("Speed", speed.ToString("F2"));
        return stats;
    }
}
