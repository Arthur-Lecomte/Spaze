using UnityEngine;

public class Slower : Construction {
    [SerializeField] private float range;
    [SerializeField] private float power;
    
    private SphereCollider trigger;
    [SerializeField] private Transform effect;

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);

        trigger = GetComponent<SphereCollider>();
        trigger.radius = range;
        effect.localScale = new Vector3(range, range, range);
    }
    
    protected override void PerformUpgrade() {
        trigger.radius = range; //Augmente la portée de l'onde
        effect.localScale = new Vector3(range, range, range);
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
    
    public void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
