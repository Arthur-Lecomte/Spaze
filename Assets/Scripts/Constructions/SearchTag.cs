using UnityEngine;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class SearchTag : Construction {
    [SerializeField] private string tagTarget;
    [SerializeField] protected float range;
    [SerializeField] protected float speed;

    private SphereCollider rangeCollider;
    private float nextActionTime;
    protected readonly List<GameObject> InRange = new List<GameObject>();
    private GameObject currentTarget;

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);

        rangeCollider = gameObject.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = range;
    }
    
    protected override void PerformUpgrade() {
        rangeCollider.radius = range;
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(tagTarget)) {
            InRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(tagTarget)) {
            InRange.Remove(other.gameObject);
        }
    }
    
    private void Update() {
        if (InRange.Count > 0) {
            currentTarget = GetClosest();
            if (currentTarget) {
                Rotate(currentTarget.transform);
                if (Time.time >= nextActionTime && IsAlignedWithTarget(currentTarget.transform)) {
                    DoAction(currentTarget.transform);
                    nextActionTime = Time.time  + 1f / speed;
                }
            }
        } else {
            currentTarget = null;
        }
    }
    
    private GameObject GetClosest() {
        GameObject closestStructure = null;
        float closestDistance = range;

        foreach (GameObject structure in InRange) {
            float distance = Vector3.Distance(transform.position, structure.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestStructure = structure;
            }
        }

        return closestStructure;
    }
    
    protected virtual void Rotate(Transform target) {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }
    
    protected virtual bool IsAlignedWithTarget(Transform target) {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float angle = Vector3.Angle(transform.forward, directionToTarget);
        return angle < 3f;
    }
    
    protected virtual void DoAction(Transform target) {
    }
    
    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Range", range.ToString("F2"));
        return stats;
    }
}
