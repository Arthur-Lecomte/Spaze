using UnityEngine;

public class Extracteur : SearchTag {
    [SerializeField] private float quantity;
    [SerializeField] private float quantityWithPercent;
    private static float percent;
    
    private LaserBeamExtractor laserBeamExtractor;
    
    private Animator animator;
    private static readonly int Extract = Animator.StringToHash("Extract");
    
    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        turnTransform = transform.GetChild(0);
        
        animator = GetComponentInChildren<Animator>();
        laserBeamExtractor = GetComponentInChildren<LaserBeamExtractor>();
        OnPercentChanged(TypeUpgrade.Extraction, percent);
    }
    
    protected override void DoAction(Transform target) {
        animator.SetBool(Extract, true);
        laserBeamExtractor.EnableLaser();
        if (!target.gameObject.GetComponent<Structure>().Extract(quantityWithPercent)) {
            InRange.Remove(target.gameObject.GetComponent<Collider>());
            StopAction();
        }
    }


    public new void Update() {
        base.Update();

        // Vérifier si l'animation d'extraction est active
        if (animator.GetBool(Extract) && laserBeamExtractor && laserBeamExtractor.IsLaserEnabled()) {
            laserBeamExtractor.UpdateLaser();
        }
    }


    protected override void StopAction()
    {
        animator.SetBool(Extract, false);
        laserBeamExtractor.DisableLaser();
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
