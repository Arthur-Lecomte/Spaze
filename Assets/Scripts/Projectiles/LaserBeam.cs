using System.Collections;
using UnityEngine;

public class LaserBeam : MonoBehaviour {
    public GameObject laserPrefab;
    public GameObject firePoint;
    public GameObject laser;
    public LayerMask collisionMask; // Masque pour détecter les collisions
    public bool ignoreObjectInFront;

    private GameObject target;

    public ParticleSystem hitEffectPrefab;
    private ParticleSystem currentHitEffect;

    void Start() {
        if (laserPrefab) {
            laser = Instantiate(laserPrefab, firePoint.transform);
            if (hitEffectPrefab) {
                currentHitEffect = Instantiate(hitEffectPrefab);
            }
            DisableLaser();
        }
    }

    public void EnableLaser(Transform cible) {
        target = cible.gameObject;

        if (firePoint) {
            if (ignoreObjectInFront) {
                StopAllCoroutines();
                laser.SetActive(true);
            }
            
            StartCoroutine(UpdateLaser());
        }
    }

    public void DisableLaser() {
        StopAllCoroutines();
        laser.SetActive(false);
        if (hitEffectPrefab) {
            currentHitEffect.Stop();
        }
    }

    private IEnumerator UpdateLaser() {
        float maxLaserLength = 300f; // Longueur maximale du laser si rien n'est touché
        float laserMargin = 2f;
        
        while (true) {
            laser.transform.position = firePoint.transform.position;
            RaycastHit hit;
            Vector3 rayDirection = firePoint.transform.forward;

            if (ignoreObjectInFront) {
                Vector3 targetPos = target.transform.position + new Vector3(0f, 0.5f, 0f);
                
                laser.transform.rotation = Quaternion.LookRotation(targetPos - firePoint.transform.position);

                // Ajuster la taille du laser
                maxLaserLength = Vector3.Distance(transform.position, targetPos);
                Vector3 newScale = laser.transform.localScale;
                newScale.z = maxLaserLength;
                laser.transform.localScale = newScale;
            } else {
                // Si le Raycast touche un objet, ajuster la longueur du laser à la distance de l'objet touché + marge
                if (Physics.Raycast(firePoint.transform.position, rayDirection, out hit, maxLaserLength, collisionMask)) {
                    if (hit.collider.gameObject != target) {
                        laser.SetActive(false);
                        currentHitEffect.Stop();
                    } else {
                        maxLaserLength = hit.distance + laserMargin;

                        // Ajuster la taille du laser
                        Vector3 newScale = laser.transform.localScale;
                        newScale.z = maxLaserLength;
                        laser.transform.localScale = newScale;
                        
                        //Particules
                        if (hitEffectPrefab) {
                            currentHitEffect.transform.position = hit.point;
                            currentHitEffect.transform.forward = hit.normal;
                            if (!currentHitEffect.isPlaying) {
                                currentHitEffect.Play();
                            }
                        }
                    
                        if (laser.activeSelf == false) {
                            laser.SetActive(true);
                        }
                    }
                } else {
                    // Si le Raycast ne touche rien, désactiver le laser
                    DisableLaser();
                }
            }
            yield return null;
        }
    }

    public bool IsLaserEnabled() {
        return laser.activeSelf;
    }
}
