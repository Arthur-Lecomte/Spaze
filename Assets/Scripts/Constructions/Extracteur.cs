using UnityEngine;
using System.Collections.Generic;

public class Extracteur : Construction {
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private Transform extracteur;
    
    private float nextExtractTime;
    private List<GameObject> structureInRange = new();
    private Structure currentTarget;

    protected override void Awake() {
        base.Awake();

        SphereCollider rangeCollider = gameObject.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = range;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.GetComponent<Structure>() != null) {
            structureInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.GetComponent<Structure>() != null) {
            structureInRange.Remove(other.gameObject);
        }
    }
    
    private void Update() {
        if (structureInRange.Count > 0) {
            currentTarget = GetClosestStructure();
            if (currentTarget) {
                //RotateExtracteur(currentTarget.transform); //DEBUG!!! à faire
                if (Time.time >= nextExtractTime) { //DEBUG!!! && IsAlignedWithTarget(currentTarget.transform)
                    currentTarget.Extract();
                    nextExtractTime = Time.time  + 1f / speed;
                }
            }
        } else {
            currentTarget = null;
        }
    }
    
    private Structure GetClosestStructure() {
        GameObject closestStructure = null;
        float closestDistance = range;

        foreach (GameObject structure in structureInRange) {
            float distance = Vector3.Distance(transform.position, structure.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestStructure = structure;
            }
        }

        return closestStructure.GetComponent<Structure>();
    }
    
    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
    
    protected override void SetVariableForRarity(float multiplicator) {
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Speed", speed.ToString("F2"));
        stats.Add("Range", range.ToString("F2"));
        return stats;
    }
    
    public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
