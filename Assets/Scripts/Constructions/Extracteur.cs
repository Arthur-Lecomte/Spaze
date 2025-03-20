using UnityEngine;
using System.Collections.Generic;

public class Extracteur : Construction {
    [SerializeField] private float speed;
    [SerializeField] private float range;
    
    public float Speed => speed;
    public float Range => range;
    
    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Speed", speed.ToString("F2"));
        stats.Add("Range", range.ToString("F2"));
        return stats;
    }
}
