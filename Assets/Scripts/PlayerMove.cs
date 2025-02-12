using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;

    void Update() {
        // Avancer
        if(Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }

        // Rotation à gauche
        if(Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.down * rotationSpeed * Time.deltaTime);
        }

        // Rotation à droite
        if(Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
        }
    }
    
    public void TakeDamage() {
        Debug.Log("TOUCHÉ !");
    }
}
