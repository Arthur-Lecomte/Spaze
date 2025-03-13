using UnityEngine;

public class Vaisseau : MonoBehaviour {
    public Inventory inventory;

    [Header("Déplacement")]
    public float acceleration = 10f;   // Force d'accélération
    public float maxSpeed = 5f;        // Vitesse maximale
    public float rotationSpeed = 200f; // Vitesse de rotation
    public float drag = 0.99f;         // Ralentissement progressif (momentum)

    private Rigidbody rb;
    private bool isAccelerating = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearDamping = 0;  // Pas de drag par défaut (on gère la friction nous-même)
        rb.angularDamping = 5f; // Légère résistance à la rotation
    }

    void Update()
    {
        // Détection de l'accélération
        isAccelerating = Input.GetKey(KeyCode.Z);
        Debug.Log(isAccelerating);

        // Gestion de la rotation (sur l'axe Z, vue du dessus)
        float rotationInput = 0f;
        if (Input.GetKey(KeyCode.Q)) rotationInput = 1f;  // Rotation à gauche
        if (Input.GetKey(KeyCode.D)) rotationInput = -1f; // Rotation à droite

        // Appliquer la rotation sur l'axe Z
        rb.AddTorque(Vector3.forward * rotationInput * rotationSpeed * Time.deltaTime, ForceMode.Force);
    }

    void FixedUpdate()
    {
        // Appliquer une force pour avancer si on accélère
        if (isAccelerating)
        {
            rb.AddForce(transform.up * acceleration, ForceMode.Acceleration);
        }

        // Limiter la vitesse max
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // Simuler une légère friction (momentum progressif)
        rb.linearVelocity *= drag;
    }

}
