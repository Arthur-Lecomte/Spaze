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
        laserBeamExtractor = GetComponentInChildren<LaserBeamExtractor>();

    }

    protected override void DoAction(Transform target)
    {
        animator.SetBool(Extract, true);
        laserBeamExtractor.EnableLaser();

        if (!target.gameObject.GetComponent<Structure>().Extract())
        {
            InRange.Remove(target.gameObject.GetComponent<Collider>());
            StopAction();
        }
    }


    public new void Update()
    {
        base.Update();

        // Vérifier si l'animation d'extraction est active
        if (animator.GetBool(Extract) && laserBeamExtractor != null && laserBeamExtractor.IsLaserEnabled())
        {
            laserBeamExtractor.UpdateLaser();
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
