using UnityEngine;
using System.Collections.Generic;

public class Speed : Construction {
    [SerializeField] private float additionalSpeed;
    
    public float AdditionalSpeed => additionalSpeed;

    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Additional Speed", additionalSpeed.ToString("F2"));
        return stats;
    }
}
