using System.Collections;
using UnityEngine;

public class LaserBeam : MonoBehaviour {
    public GameObject laserPrefab;
    public GameObject firePoint;
    private GameObject laser;
    public LayerMask collisionMask; // Masque pour détecter les collisions
    public bool ignoreObjectInFront;

    private GameObject target;

    public ParticleSystem hitEffectPrefab;
    private ParticleSystem currentHitEffect;

    void Start() {
        laser = Instantiate(laserPrefab, firePoint.transform);
        currentHitEffect = Instantiate(hitEffectPrefab);
        DisableLaser();
    }

    public void EnableLaser(Transform cible) {
        target = cible.gameObject;

        if (firePoint) {
            StartCoroutine(UpdateLaser());
        }
    }

    public void DisableLaser() {
        StopCoroutine(UpdateLaser());
        laser.SetActive(false);
        currentHitEffect.Stop();
    }

    private IEnumerator UpdateLaser() {
        float maxLaserLength = 300f; // Longueur maximale du laser si rien n'est touché
        float laserMargin = 2f;
        
        while (true) {
            laser.transform.position = firePoint.transform.position;
            RaycastHit hit;
            Vector3 rayDirection = firePoint.transform.forward;

            // Si le Raycast touche un objet, ajuster la longueur du laser à la distance de l'objet touché + marge
            if (Physics.Raycast(firePoint.transform.position, rayDirection, out hit, maxLaserLength, collisionMask)) {
                if (!ignoreObjectInFront && hit.collider.gameObject != target) {
                    laser.SetActive(false);
                    currentHitEffect.Stop();
                } else {
                    maxLaserLength = hit.distance + laserMargin;

                    // Ajuster la taille du laser
                    Vector3 newScale = laser.transform.localScale;
                    newScale.z = maxLaserLength;
                    laser.transform.localScale = newScale;
                    //Particules

                    currentHitEffect.transform.position = hit.point;
                    currentHitEffect.transform.forward = hit.normal;
                    if (!currentHitEffect.isPlaying) {
                        currentHitEffect.Play();
                    }
                    
                    if (laser.activeSelf == false) {
                        laser.SetActive(true);
                    }
                }
            } else {
                // Si le Raycast ne touche rien, désactiver le laser
                DisableLaser();
            }
            yield return null;
        }
    }

    public bool IsLaserEnabled() {
        return laser.activeSelf;
    }
}
