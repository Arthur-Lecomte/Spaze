using UnityEngine;

public class Extracteur : SearchTag {
    [SerializeField] private float quantity;
    
    private Animator animator;
    private static readonly int Extract = Animator.StringToHash("Extract");
    
    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        turnTransform = transform.GetChild(0);
        
        animator = GetComponentInChildren<Animator>();
    }
    
    protected override void DoAction(Transform target) {
        animator.SetBool(Extract, true);
        if (!target.gameObject.GetComponent<Structure>().Extract()) {
            InRange.Remove(target.gameObject.GetComponent<Collider>());
            StopAction();
        }
    }
    
    protected override void StopAction() {
        animator.SetBool(Extract, false);
    }
    
    public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
