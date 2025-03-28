using UnityEngine;

namespace Spaze {
    public class Vaisseau : MonoBehaviour, ICanTakeDamage {
        public static Vaisseau Instance;

        private float maxHealth;
        private float actualHealth;
        private RectTransform healthBar;
        private float healthBarMaxWidth;

        [Header("Déplacement")]
        [SerializeField]
        private float acceleration = 10f; // Force appliquée à l'accélération
        [SerializeField] private float maxSpeed = 5f; // Vitesse maximale
        [SerializeField] private float rotationSpeed = 200f; // Vitesse de rotation
        [SerializeField] private float drag = 0.99f; // Ralentissement progressif (momentum)

        private AudioSource ReactorAudioSource;

        private Rigidbody rb;
        private bool isAccelerating;

        /// <summary>
        /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
        /// </summary>
        private void Awake() {
            if (Instance == null) {
                Instance = this;
            } else {
                Destroy(gameObject);
            }

            ReactorAudioSource = gameObject.AddComponent<AudioSource>();

            rb = GetComponent<Rigidbody>();
            rb.useGravity = false; // Pas de gravité pour un vaisseau spatial
            rb.angularDamping = 5f; // Réduit l'effet de rotation excessive

            maxHealth = 150;
            actualHealth = maxHealth;
            healthBar = GameObject.Find("HealthBar").GetComponent<RectTransform>();
            healthBarMaxWidth = healthBar.sizeDelta.x;
        }

        /// <summary>
        /// Méthode appelée à chaque frame pour mettre à jour l'état du vaisseau.
        /// </summary>
        void Update() {
            // Récupérer l'input pour la rotation avec Q/D
            float rotationInput = Input.GetAxis("Horizontal");
            isAccelerating = Input.GetAxis("Vertical") > 0; // Avancer avec Z

            // Appliquer la rotation (tourne autour de l'axe Y)
            if (rotationInput != 0) {
                transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);
            }
        }

        /// <summary>
        /// Méthode appelée à chaque frame fixe pour mettre à jour la physique du vaisseau.
        /// </summary>
        void FixedUpdate() {
            // Calcul de la vitesse max avec le boost
            float boostedMaxSpeed = maxSpeed + Speed.AllMaxSpeed;

            // Appliquer une force vers l'avant seulement si le joueur accélère
            if (isAccelerating) {
                rb.AddForce(transform.forward * (acceleration + Speed.AllSpeedPower), ForceMode.Acceleration);
                if (!ReactorAudioSource.isPlaying) {
                    SoundManager.PlaySoundWithFade(SoundType.REACTOR, ReactorAudioSource, 0.3f);
                }

            } else {
                SoundManager.StopSoundWithFade(ReactorAudioSource, 0.2f);
            }

            // Limiter la vitesse
            if (rb.linearVelocity.magnitude > boostedMaxSpeed) {
                rb.linearVelocity = rb.linearVelocity.normalized * boostedMaxSpeed;
            }

            // Appliquer une légère friction pour l'inertie
            rb.linearVelocity *= drag;
        }

        /// <summary>
        /// Applique des dégâts au vaisseau.
        /// </summary>
        /// <param name="damage">La quantité de dégâts à appliquer.</param>
        public void TakeDamage(float damage) {
            actualHealth = Mathf.Max(0, actualHealth - damage);
            if (actualHealth == 0) {
                DeathManager.Instance.GameOver();
            }
            healthBar.sizeDelta = new Vector2(healthBarMaxWidth * actualHealth / maxHealth, healthBar.sizeDelta.y);
        }

        /// <summary>
        /// Régénère la santé du vaisseau à son maximum.
        /// </summary>
        public void Regeneration() {
            actualHealth = maxHealth;
            healthBar.sizeDelta = new Vector2(healthBarMaxWidth, healthBar.sizeDelta.y);
        }

        /// <summary>
        /// Obtient la quantité de santé nécessaire pour être au maximum.
        /// </summary>
        /// <returns>La quantité de santé nécessaire.</returns>
        public float GetHealthForBeFull() {
            return maxHealth - actualHealth;
        }

        /// <summary>
        /// Indique si l'objet est contrôlé par le joueur.
        /// </summary>
        /// <returns>True car il s'agit du vaisseau du joueur.</returns>
        public bool AmIPlayer() {
            return true;
        }
    }
}