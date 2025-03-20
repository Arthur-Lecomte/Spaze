using UnityEngine;

public class Vaisseau : MonoBehaviour, ICanTakeDamage {
    public static Vaisseau Instance;

    [Header("Déplacement")]
    [SerializeField]
    private float acceleration = 10f; // Force appliquée à l'accélération
    [SerializeField] private float maxSpeed = 5f; // Vitesse maximale
    [SerializeField] private float rotationSpeed = 200f; // Vitesse de rotation
    [SerializeField] private float drag = 0.99f; // Ralentissement progressif (momentum)

    [Header("Boost")] [SerializeField] private int speedSkillCount; // Nombre de points de vitesse appliqués 
    [SerializeField] private float pourcentageBoost = 0.3f; // Pourcentage de boost appliqué par point 

    private Rigidbody rb;
    private bool isAccelerating = false;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        speedSkillCount = 2; //DEBUG!!! Ne pas faire une initialisation manuelle
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // Pas de gravité pour un vaisseau spatial
        rb.angularDamping = 5f; // Réduit l'effet de rotation excessive
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
        float boostedMaxSpeed = maxSpeed * (1 + pourcentageBoost * speedSkillCount);

        // Appliquer une force vers l'avant seulement si le joueur accélère
        if (isAccelerating) {
            rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);
        }

        // Limiter la vitesse
        if (rb.linearVelocity.magnitude > boostedMaxSpeed) {
            rb.linearVelocity = rb.linearVelocity.normalized * boostedMaxSpeed;
        }

        // Appliquer une légère friction pour l'inertie
        rb.linearVelocity *= drag;
    }
    
    //DEBUG!!! Le faire pour tous les types
    public void UpgradePointsChanged(TypeUpgrade type, int points) {
        switch (type) {
            case TypeUpgrade.Speed:
                speedSkillCount = points;
                break;
        }
    }

    public void TakeDamage(float damage) {
        //DEBUG!!! Appliquer les dégâts
    }
}
