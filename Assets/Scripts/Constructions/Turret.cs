using UnityEngine;
using System.Collections.Generic;

public class Turret : Construction {
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float range;
    [SerializeField] private GameObject bulletPrefab;

    private Transform turretHead;
    private Transform[] missileSpawnPoints;
    private int currentSpawnPointIndex;
    private float nextFireTime;
    private List<GameObject> enemiesInRange = new();
    private GameObject currentTarget;

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        turretHead = transform.GetChild(0).GetChild(0);
        missileSpawnPoints = new Transform[turretHead.childCount];
        for (int i = 0; i < turretHead.childCount; i++) {
            missileSpawnPoints[i] = turretHead.GetChild(i);
        }

        SphereCollider rangeCollider = gameObject.AddComponent<SphereCollider>();
        rangeCollider.isTrigger = true;
        rangeCollider.radius = range;
    }

    private void Update() {
        if (enemiesInRange.Count > 0) {
            currentTarget = GetClosestEnemy();
            if (currentTarget) {
                RotateTurretHead(currentTarget.transform);
                if (Time.time >= nextFireTime && IsAlignedWithTarget(currentTarget.transform)) {
                    Fire(currentTarget);
                    nextFireTime = Time.time + 1f / attackSpeed;
                }
            }
        } else {
            currentTarget = null;
        }
    }
    
    private bool IsAlignedWithTarget(Transform target) {
        Vector3 directionToTarget = (target.position - turretHead.position).normalized;
        float angle = Vector3.Angle(turretHead.forward, directionToTarget);
        return angle < 3f;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemiesInRange.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Enemy")) {
            enemiesInRange.Remove(other.gameObject);
        }
    }

    private GameObject GetClosestEnemy() {
        GameObject closestEnemy = null;
        float closestDistance = range;

        foreach (GameObject enemy in enemiesInRange) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance) {
                closestDistance = distance;
                closestEnemy = enemy;
            }
        }

        return closestEnemy;
    }

    private void RotateTurretHead(Transform target) {
        Vector3 direction = (target.position - turretHead.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, Time.deltaTime * 10f);
    }

    private void Fire(GameObject target) {
        Transform spawnPoint = missileSpawnPoints[currentSpawnPointIndex];
        currentSpawnPointIndex = (currentSpawnPointIndex + 1) % missileSpawnPoints.Length;

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, spawnPoint.rotation);
        Tir bulletScript = bullet.GetComponent<Tir>();
        bulletScript.SetInformations(Vaisseau.Instance.gameObject, damage, bulletSpeed, range);
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Damage", damage.ToString());
        stats.Add("Attack Speed", attackSpeed.ToString("F2"));
        return stats;
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}