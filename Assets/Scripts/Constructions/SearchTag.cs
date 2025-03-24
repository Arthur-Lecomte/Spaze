using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public abstract class SearchTag : Construction {
    [SerializeField] private string tagTarget;
    [SerializeField] protected float range;
    [SerializeField] protected float speed;

    protected Transform turnTransform;
    protected float turnSpeed = 2f;

    private SphereCollider rangeCollider;
    private float nextActionTime;
    protected readonly List<Collider> InRange = new List<Collider>();
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
            InRange.Add(other);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag(tagTarget)) {
            if (currentTarget == other.gameObject) {
                StopAction();
            }

            InRange.Remove(other);
        }
    }

    private void Update() {
        if (InRange.Count > 0) {
            currentTarget = GetClosest();
            if (currentTarget) {
                Rotate(currentTarget.transform);
                if (Time.time >= nextActionTime && IsAlignedWithTarget(currentTarget.transform)) {
                    DoAction(currentTarget.transform);
                    nextActionTime = Time.time + 1f / speed;
                }
            }
        } else {
            currentTarget = null;
            turnTransform.rotation = Quaternion.Slerp(turnTransform.rotation, transform.parent.rotation, Time.deltaTime * turnSpeed  / 4);
        }
    }

    private GameObject GetClosest() {
        GameObject closestObject = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider c in InRange) {
            float distance = Vector3.Distance(transform.position, c.ClosestPoint(transform.position));
            if (distance < closestDistance) {
                closestDistance = distance;
                closestObject = c.gameObject;
            }
        }

        return closestObject;
    }

    private void Rotate(Transform target) {
        Vector3 direction = (target.position - turnTransform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turnTransform.rotation = Quaternion.Slerp(turnTransform.rotation, lookRotation, Time.deltaTime * turnSpeed);
    }

    private bool IsAlignedWithTarget(Transform target) {
        Vector3 directionToTarget = (target.position - turnTransform.position).normalized;
        float angle = Vector3.Angle(turnTransform.forward, directionToTarget);
        return angle < 3f;
    }

    protected virtual void DoAction(Transform target) { }

    protected virtual void StopAction() { }
}