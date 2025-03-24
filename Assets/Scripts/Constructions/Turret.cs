using System;
using UnityEngine;
using System.Collections.Generic;

public class Turret : SearchTag {
    [SerializeField] private float damage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;

    private Transform turretHead;
    private Transform[] missileSpawnPoints;
    private int currentSpawnPointIndex;
    
    //public delegate void EnemyKilledHandler(GameObject enemy);
    public static Action<GameObject> onEnemyKilled;


    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        
        turretHead = transform.GetChild(0).GetChild(0);
        missileSpawnPoints = new Transform[turretHead.childCount];
        for (int i = 0; i < turretHead.childCount; i++) {
            missileSpawnPoints[i] = turretHead.GetChild(i);
        }
        
        onEnemyKilled += CheckList;
    }

    protected override void Rotate(Transform target) {
        Vector3 direction = (target.position - turretHead.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        turretHead.rotation = Quaternion.Slerp(turretHead.rotation, lookRotation, Time.deltaTime * 10f);
    }
    
    protected override bool IsAlignedWithTarget(Transform target) {
        Vector3 directionToTarget = (target.position - turretHead.position).normalized;
        float angle = Vector3.Angle(turretHead.forward, directionToTarget);
        return angle < 3f;
    }

    protected override void DoAction(Transform target) {
        Transform spawnPoint = missileSpawnPoints[currentSpawnPointIndex];
        currentSpawnPointIndex = (currentSpawnPointIndex + 1) % missileSpawnPoints.Length;
        
        Vector3 direction = (target.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.LookRotation(direction)  * Quaternion.Euler(90, 0, 0));
        bullet.GetComponent<Tir>().SetInformations(Vaisseau.Instance.gameObject, damage, bulletSpeed, range);
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Damage", damage.ToString());
        stats.Add("Attack Speed", speed.ToString("F2"));
        return stats;
    }
    
    private void CheckList(GameObject enemy) {
        InRange.Remove(enemy.GetComponent<Collider>());
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}