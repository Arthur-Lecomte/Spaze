using System;
using UnityEngine;

public class Turret : SearchTag {
    [SerializeField] private float damage;
    [SerializeField] private float damageWithPercent;
    private static float percent;
    
    [SerializeField] private GameObject bulletPrefab;
    private LaserBeam laserBeam;
    
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
        
        laserBeam = GetComponent<LaserBeam>();
        OnPercentChanged(TypeUpgrade.Attack, percent);
    }

    protected override void DoAction(Transform target) {
        if (laserBeam) {
            if (laserBeam.IsLaserEnabled()) { // Si le laser est activé, on fait des dégâts à l'ennemi
                target.gameObject.GetComponent<Enemy>().TakeDamage(damageWithPercent);

            }
        } else {
            Transform spawnPoint = missileSpawnPoints[currentSpawnPointIndex];
            currentSpawnPointIndex = (currentSpawnPointIndex + 1) % missileSpawnPoints.Length;
        
            Vector3 direction = (target.position - transform.position).normalized;

            GameObject bullet = Instantiate(bulletPrefab, spawnPoint.position, Quaternion.LookRotation(direction));
            bullet.GetComponent<Tir>().SetInformations(Vaisseau.Instance.gameObject, damageWithPercent, range);
        }
    }
    
    protected override void DoAnimation(Transform target) {
        if (laserBeam) {
            laserBeam.EnableLaser(target);
        }
    }

    protected override void StopAnimation() {
        if (laserBeam) {
            laserBeam.DisableLaser();
        }
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