using UnityEngine;

public class Extracteur : SearchTag
{
    [SerializeField] private float quantity;
    private LaserBeamExtractor laserBeamExtractor;


    private Animator animator;
    private static readonly int Extract = Animator.StringToHash("Extract");

    public override void Initialisation(RarityConstruction rarityConstruction)
    {
        base.Initialisation(rarityConstruction);
        turnTransform = transform.GetChild(0);

        animator = GetComponentInChildren<Animator>();

        // Récupérer le composant LaserBeam
        laserBeamExtractor = GetComponentInChildren<LaserBeamExtractor>();
        if (laserBeamExtractor == null)
        {
            Debug.LogError("LaserBeam introuvable sur l'Extracteur ou ses enfants !");
        }
    }

    protected override void DoAction(Transform target)
    {
        animator.SetBool(Extract, true);

        
        laserBeamExtractor.EnableLaser();
        laserBeamExtractor.UpdateLaser(); // Met à jour la position et la rotation du laser
        


        if (!target.gameObject.GetComponent<Structure>().Extract())
        {
            InRange.Remove(target.gameObject.GetComponent<Collider>());
            StopAction();
        }
    }

    protected override void StopAction()
    {
        animator.SetBool(Extract, false);
        laserBeamExtractor.DisableLaser();
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
