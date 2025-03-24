using UnityEngine;
using System.Collections.Generic;

public class Speed : Construction {
    [SerializeField] private float power;

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Puissance", power.ToString("F2"));
        return stats;
    }
}
