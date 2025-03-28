using System.Collections;
using UnityEngine;
namespace Spaze {
    public class LaserBeam : MonoBehaviour {
        public GameObject laserPrefab;
        public GameObject firePoint;
        public GameObject laser;
        public LayerMask collisionMask; // Masque pour détecter les collisions
        public bool ignoreObjectInFront;
        private Transform parentTransform;

        private AudioSource extractorAudioSource;

        private GameObject target;

        public ParticleSystem hitEffectPrefab;
        private ParticleSystem currentHitEffect;

        /// <summary>
        /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
        /// </summary>
        private void Awake() {
            extractorAudioSource = gameObject.AddComponent<AudioSource>();
            parentTransform = transform.GetChild(0);
        }

        /// <summary>
        /// Méthode appelée au démarrage. Initialise le laser et les effets de particules.
        /// </summary>
        void Start() {
            if (laserPrefab) {
                laser = Instantiate(laserPrefab, firePoint.transform);
                if (hitEffectPrefab) {
                    currentHitEffect = Instantiate(hitEffectPrefab);
                }
                DisableLaser();
            }
        }

        /// <summary>
        /// Active le laser et le dirige vers la cible spécifiée.
        /// </summary>
        /// <param name="cible">La cible à viser.</param>
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

        /// <summary>
        /// Désactive le laser et arrête les effets de particules.
        /// </summary>
        public void DisableLaser() {
            StopAllCoroutines();
            laser.SetActive(false);
            if (hitEffectPrefab) {
                currentHitEffect.Stop();
            }

            if (extractorAudioSource.isPlaying) {
                SoundManager.StopSoundWithFade(extractorAudioSource, 0.3f);
            }
        }

        /// <summary>
        /// Coroutine pour mettre à jour la position et la direction du laser.
        /// </summary>
        /// <returns>Un IEnumerator pour la coroutine.</returns>
        private IEnumerator UpdateLaser() {
            float maxLaserLength = 300f; // Longueur maximale du laser si rien n'est touché
            float laserMargin = 2f;

            while (true) {
                laser.transform.position = firePoint.transform.position;
                if (ignoreObjectInFront) {
                    Vector3 rayDirection = (target.transform.position + new Vector3(0f, 0.5f, 0f) - firePoint.transform.position);
                    laser.transform.rotation = Quaternion.LookRotation(rayDirection);

                    // Ajuster la taille du laser
                    maxLaserLength = Vector3.Distance(transform.position, target.transform.position + new Vector3(0f, 0.5f, 0f));
                    Vector3 newScale = laser.transform.localScale;
                    newScale.z = maxLaserLength / parentTransform.localScale.z;
                    laser.transform.localScale = newScale;
                } else {
                    Vector3 rayDirection = (target.transform.position - firePoint.transform.position).normalized;
                    laser.transform.rotation = Quaternion.LookRotation(rayDirection);
                    // Si le Raycast touche un objet, ajuster la longueur du laser à la distance de l'objet touché + marge
                    if (Physics.Raycast(firePoint.transform.position, rayDirection, out RaycastHit hit, maxLaserLength, collisionMask)) {
                        if (hit.collider.gameObject != target) {
                            laser.SetActive(false);
                            currentHitEffect.Stop();
                        } else {
                            maxLaserLength = hit.distance + laserMargin;

                            // Ajuster la taille du laser
                            Vector3 newScale = laser.transform.localScale;
                            newScale.z = maxLaserLength;
                            laser.transform.localScale = newScale;

                            // Particules
                            if (hitEffectPrefab) {
                                currentHitEffect.transform.position = hit.point;
                                currentHitEffect.transform.forward = hit.normal;
                                if (!currentHitEffect.isPlaying) {
                                    currentHitEffect.Play();
                                }
                            }

                            // Son
                            if (!extractorAudioSource.isPlaying) {
                                SoundManager.PlaySoundWithFade(SoundType.EXTRACTOR, extractorAudioSource, 0.5f);
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

        /// <summary>
        /// Vérifie si le laser est activé.
        /// </summary>
        /// <returns>True si le laser est activé, sinon False.</returns>
        public bool IsLaserEnabled() {
            return laser.activeSelf;
        }

        /// <summary>
        /// Dessine des gizmos pour visualiser la direction du laser dans l'éditeur.
        /// </summary>
        private void OnDrawGizmos() {
            if (firePoint != null) {
                Gizmos.color = Color.magenta;
                if (target) {
                    Vector3 rayDirection = (target.transform.position - firePoint.transform.position).normalized;
                    Gizmos.DrawRay(firePoint.transform.position, rayDirection * 300);
                }
            }
        }
    }
}