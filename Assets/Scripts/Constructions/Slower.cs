using UnityEngine;

public class Slower : Construction {
    [SerializeField] private float range;
    [SerializeField] private float power;
    
    private SphereCollider trigger;

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);

        trigger = gameObject.AddComponent<SphereCollider>();
        trigger.radius = range;
    }
    
    protected override void PerformUpgrade() {
        trigger.radius = range; //Augmente la portée de l'onde
    }

    private void OnTriggerEnter(Collider other) {
        Tir projectile = other.GetComponent<Tir>();
        if (projectile != null && !projectile.IsFromPlayer()) {
            projectile.InSlowArea(power);
        }
    }

    private void OnTriggerExit(Collider other) {
        Tir projectile = other.GetComponent<Tir>();
        if (projectile != null && !projectile.IsFromPlayer()) {
            projectile.InSlowArea(-power);
        }
    }
}
