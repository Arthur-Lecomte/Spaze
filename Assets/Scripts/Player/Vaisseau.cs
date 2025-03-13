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
        float moveInput = Input.GetAxis("Vertical"); // Prend en charge ZQSD ou WASD selon config Unity
        float rotationInput = Input.GetAxis("Horizontal"); // Prend en charge Q/D

        isAccelerating = moveInput > 0;

        rb.AddTorque(-rotationInput * rotationSpeed * Time.deltaTime * Vector3.forward, ForceMode.Force);
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
