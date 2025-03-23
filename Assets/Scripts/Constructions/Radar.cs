using UnityEngine;
using System.Collections.Generic;

public class Radar : Construction {
    [SerializeField] private float range;

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Range", range.ToString("F2"));
        return stats;
    }
}
