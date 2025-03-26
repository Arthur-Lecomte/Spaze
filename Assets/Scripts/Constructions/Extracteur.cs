using UnityEngine;

public class Extracteur : SearchTag {
    [SerializeField] private float quantity;
    [SerializeField] private float quantityWithPercent;
    private static float percent;
    
    private Animator animator;
    private static readonly int Extract = Animator.StringToHash("Extract");
    
    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        turnTransform = transform.GetChild(0);
        
        animator = GetComponentInChildren<Animator>();
        
        OnPercentChanged(TypeUpgrade.Extraction, percent);
    }
    
    protected override void DoAction(Transform target) {
        animator.SetBool(Extract, true);
        if (!target.gameObject.GetComponent<Structure>().Extract(quantityWithPercent)) {
            InRange.Remove(target.gameObject.GetComponent<Collider>());
            StopAction();
        }
    }
    
    protected override void StopAction() {
        animator.SetBool(Extract, false);
    }
    
    protected override void PerformUpgrade() {
        OnPercentChanged(TypeUpgrade.Extraction, percent);
    }
    
    protected override void OnPercentChanged(TypeUpgrade typeUpgrade, float value) {
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
