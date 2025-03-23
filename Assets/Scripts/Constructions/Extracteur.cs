using UnityEngine;
using System.Collections.Generic;

public class Extracteur : SearchTag {
    [SerializeField] private float quantity;

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);

        SphereCollider rangeCollider = gameObject.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = range;
    }
    
    protected override void DoAction(Transform target) { //DEBUG Jouer animation d'extraction
        if (!target.gameObject.GetComponent<Structure>().Extract()) {
            InRange.Remove(target.gameObject);
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
