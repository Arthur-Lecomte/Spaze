using UnityEngine;

public class Extracteur : SearchTag {
    [SerializeField] private float quantity;
    [SerializeField] private float quantityWithPercent;
    private static float percent;

    private LaserBeam laserBeam;

    private Animator animator;
    private static readonly int Extract = Animator.StringToHash("Extract");

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        turnTransform = transform.GetChild(0);

        animator = GetComponentInChildren<Animator>();
        laserBeam = GetComponent<LaserBeam>();
        OnPercentChanged(TypeUpgrade.Extraction, percent);
    }

    protected override void DoAction(Transform target) {
        if (laserBeam.IsLaserEnabled()) { // Si le laser est activé, on extrait les ressources
            if (!target.gameObject.GetComponent<Structure>().Extract(quantityWithPercent)) {
                //Il n'y a plus de ressource à extraire
                InRange.Remove(target.gameObject.GetComponent<Collider>());
                StopAnimation();
            }
        }
    }
    
    protected override void DoAnimation(Transform target) {
        animator.SetBool(Extract, true);
        laserBeam.EnableLaser(target);
    }

    protected override void StopAnimation() {
        animator.SetBool(Extract, false);
        laserBeam.DisableLaser();
    }

    protected override void PerformUpgrade() {
        OnPercentChanged(TypeUpgrade.Extraction, percent);
    }

    protected override void OnPercentChanged(TypeUpgrade typeUpgrade, float value) {
        if (value == 0) {
            UpgradeColumn.getValue(typeUpgrade);
            return;
        }

        if (typeUpgrade == TypeUpgrade.Extraction) {
            percent = value;
            quantityWithPercent = quantity * percent;
        }
    }

    public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
