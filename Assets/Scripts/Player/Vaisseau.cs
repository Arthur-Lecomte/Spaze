using SmallHedge.SoundManager;
using UnityEngine;

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

    [Header("Boost")][SerializeField] private float speedSkillPercentage = 2; // Nombre de points de vitesse appliqués 
    private AudioSource ReactorAudioSource;

    private Rigidbody rb;
    private bool isAccelerating;

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

    public void SetSpeedSkillPercentage(float percentage) {
        speedSkillPercentage = percentage;
    }

    void Update() {
        // Récupérer l'input pour la rotation avec Q/D
        float rotationInput = Input.GetAxis("Horizontal");
        isAccelerating = Input.GetAxis("Vertical") > 0; // Avancer avec Z

        // Appliquer la rotation (tourne autour de l'axe Y)
        if (rotationInput != 0) {
            transform.Rotate(Vector3.up * rotationInput * rotationSpeed * Time.deltaTime);
        }
    }

    void FixedUpdate() {
        // Calcul de la vitesse max avec le boost
        float boostedMaxSpeed = maxSpeed + (Speed.AllMaxSpeed * speedSkillPercentage);

        // Appliquer une force vers l'avant seulement si le joueur accélère
        if (isAccelerating) {
            rb.AddForce(transform.forward * (acceleration + (Speed.AllPower * speedSkillPercentage)), ForceMode.Acceleration);
            if(!ReactorAudioSource.isPlaying){
                SoundManager.PlaySoundWithFade(SoundType.REACTOR,ReactorAudioSource,0.3f);
            }
            
        } else{
            SoundManager.StopSoundWithFade(ReactorAudioSource,0.2f);
        }

        
        // Limiter la vitesse
        if (rb.linearVelocity.magnitude > boostedMaxSpeed) {
            rb.linearVelocity = rb.linearVelocity.normalized * boostedMaxSpeed;
        }

        // Appliquer une légère friction pour l'inertie
        rb.linearVelocity *= drag;
    }

    public void TakeDamage(float damage) {
        actualHealth = Mathf.Max(0, actualHealth - damage);
        if (actualHealth == 0) {
            //GameManager.Instance.GameOver();
            
        }
        healthBar.sizeDelta = new Vector2(healthBarMaxWidth * actualHealth / maxHealth, healthBar.sizeDelta.y);
    }
    
    public void Regeneration() {
        actualHealth = maxHealth;
        healthBar.sizeDelta = new Vector2(healthBarMaxWidth, healthBar.sizeDelta.y);
    }
    
    public float GetHealthForBeFull() {
        return maxHealth - actualHealth;
    }
    
    public bool AmIPlayer() {
        return true;
    }
}
