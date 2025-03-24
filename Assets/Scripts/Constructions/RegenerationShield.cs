using UnityEngine;
using System.Collections.Generic;

public class RegenerationShield : Construction {
    [SerializeField] private float capacity;
    [SerializeField] private float regeneration;
    [SerializeField] private float range;

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Regeneration Speed", regeneration.ToString("F2"));

        return stats;
    }
}
