using UnityEngine;

public class LaserBeamExtractor : MonoBehaviour
{
    public GameObject laserPrefab;
    public GameObject firePoint;
    private GameObject laser;
    public float laserGrowthSpeed = 20f; // Vitesse d'augmentation de la taille
    public LayerMask collisionMask; // Masque pour détecter les collisions

    public ParticleSystem hitEffectPrefab; 
    private ParticleSystem currentHitEffect; 

    void Start()
    {
        laser = Instantiate(laserPrefab, firePoint.transform);
        DisableLaser();
        currentHitEffect = Instantiate(hitEffectPrefab);
        currentHitEffect.Stop();
    }

    public void EnableLaser()
    {
        laser.SetActive(true);
    }

    public void DisableLaser()
    {
        laser.SetActive(false);
        laser.transform.localScale = Vector3.one;
    }

    public void UpdateLaser()
    {
        if (firePoint != null)
        {
            laser.transform.position = firePoint.transform.position;
            RaycastHit hit;
            Vector3 rayDirection = firePoint.transform.forward;

            float maxLaserLength = 25f; // Longueur maximale du laser si rien n'est touché
            float laserMargin = 2f;

            // Si le Raycast touche un objet, ajuster la longueur du laser à la distance de l'objet touché + marge
            if (Physics.Raycast(firePoint.transform.position, rayDirection, out hit, maxLaserLength, collisionMask))
            {
                maxLaserLength = hit.distance + laserMargin;

                // Ajuster la taille du laser
                Vector3 newScale = laser.transform.localScale;
                newScale.z = maxLaserLength;
                laser.transform.localScale = newScale;
                //Particules
                
                currentHitEffect.transform.position = hit.point;
                currentHitEffect.transform.forward = hit.normal;

                if (!currentHitEffect.isPlaying)
                {
                    currentHitEffect.Play();
                }
            }
            else
            {
                // Si le Raycast ne touche rien, désactiver le laser
                DisableLaser();
                currentHitEffect.Stop();
            }
        }
    }

    public bool IsLaserEnabled()
    {
        return laser != null && laser.activeSelf;
    }
}


