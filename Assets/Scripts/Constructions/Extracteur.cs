using UnityEngine;

public class Extracteur : SearchTag {
    [SerializeField] private float quantity;
    
    protected override void DoAction(Transform target) { //DEBUG Jouer animation d'extraction
        if (!target.gameObject.GetComponent<Structure>().Extract()) {
            InRange.Remove(target.gameObject.GetComponent<Collider>());
        }
    }
    
    public void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
