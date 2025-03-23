using UnityEngine;
using System.Collections.Generic;

public class Extracteur : SearchTag {
    [SerializeField] private float quantity;
    
    protected override void DoAction(Transform target) { //DEBUG Jouer animation d'extraction
        if (!target.gameObject.GetComponent<Structure>().Extract()) {
            InRange.Remove(target.gameObject.GetComponent<Collider>());
        }
    }
    
    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        return stats;
    }
    
    public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
