using UnityEngine;

public class Tire : MonoBehaviour {
    public float speed = 20f;
    public float lifetime = 10f;
    public GameObject creator;

    void Start() {
        Destroy(gameObject, lifetime);
    }

    void Update() {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other) {
        if(other.gameObject == creator) {
            return;
        }
        
        PlayerMove player = other.GetComponent<PlayerMove>();
        if(player != null) {
            player.TakeDamage();
            Destroy(gameObject);
            return;
        }
        
        Ennemi ennemi = other.GetComponent<Ennemi>();
        if(ennemi != null) {
            ennemi.TakeDamage();
            Destroy(gameObject);
            return;
        }
        
        Composant composant = other.GetComponent<Composant>();
        if(composant != null) {
            if(composant.isActivate) {
                composant.TakeDamage();
                Destroy(gameObject);
            }
            return;
        }
    }
}
