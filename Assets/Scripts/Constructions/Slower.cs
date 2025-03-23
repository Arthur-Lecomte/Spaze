using UnityEngine;
using System.Collections.Generic;

public class Slower : Construction {
    [SerializeField] private float range;
    [SerializeField] private float power;

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Range", range.ToString("F2"));
        stats.Add("Puissance", power.ToString("F2"));
        return stats;
    }
}
