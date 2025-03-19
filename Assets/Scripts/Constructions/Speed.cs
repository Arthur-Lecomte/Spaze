using UnityEngine;
using System.Collections.Generic;

public class Speed : Construction {
    [SerializeField] private float additionalSpeed;
    
    public float AdditionalSpeed => additionalSpeed;

    public override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Additional Speed", additionalSpeed.ToString("F2"));
        return stats;
    }
}
