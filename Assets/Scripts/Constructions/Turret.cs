using System;
using UnityEngine;

public class Turret : SearchTag {
    [SerializeField] private float damage;
    [SerializeField] private float damageWithPercent;
    private static float percent;
    
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject bulletPrefab;
    
    private Transform[] missileSpawnPoints;
    private int currentSpawnPointIndex;
    
    public static Action<GameObject> onEnemyKilled;
    
    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        turnTransform = transform.GetChild(0).GetChild(0);
        turnSpeed = 10;
        
        missileSpawnPoints = new Transform[turnTransform.childCount];
        for (int i = 0; i < turnTransform.childCount; i++) {
            missileSpawnPoints[i] = turnTransform.GetChild(i);
        }
        
        onEnemyKilled += CheckList;
        
        OnPercentChanged(TypeUpgrade.Attack, percent);
    }

    protected override void DoAction(Transform target) {
        Transform spawnPoint = missileSpawnPoints[currentSpawnPointIndex];
        currentSpawnPointIndex = (currentSpawnPointIndex + 1) % missileSpawnPoints.Length;
        
        Vector3 direction = (target.position - transform.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.LookRotation(direction)  * Quaternion.Euler(90, 0, 0));
        bullet.GetComponent<Tir>().SetInformations(Vaisseau.Instance.gameObject, damageWithPercent, bulletSpeed, range);
    }
    
    private void CheckList(GameObject enemy) {
        if (InRange.Contains(enemy.GetComponent<Collider>())) {
            InRange.Remove(enemy.GetComponent<Collider>());
            StopAnimation();
        }
    }
    
    protected override void PerformUpgrade() {
        OnPercentChanged(TypeUpgrade.Attack, percent);
    }
    
    protected override void OnPercentChanged(TypeUpgrade typeUpgrade, float value) {
        if (value == 0) {
            UpgradeColumn.getValue(typeUpgrade);
            return;
        }
        
        if (typeUpgrade == TypeUpgrade.Attack) {
            percent = value;
            damageWithPercent = damage * percent;
        }
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}