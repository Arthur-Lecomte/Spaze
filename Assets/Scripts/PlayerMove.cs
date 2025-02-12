using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public static PlayerMove Instance;

    public float moveSpeed = 5f;
    public float rotationSpeed = 200f;

    public int health = 500;
    private float timeSinceLastDamage;
    public TextMeshProUGUI vieText;
    public RectTransform vieVisuel;
    
    public GameObject projectilePrefab;
    public float shootInterval = 5f;

    void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }

        UpdateLife();
    }

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

        // Health
        timeSinceLastDamage += Time.deltaTime;
        if(timeSinceLastDamage >= 5f) {
            health = Mathf.Min(health + 1, 500);
            UpdateLife();
            timeSinceLastDamage = 4.8f;
        }
        
        // Shooting
        shootInterval -= Time.deltaTime;
        if (shootInterval <= 0) {
            ShootAtNearestEnemy();
        }
    }

    public void TakeDamage() {
        health = Mathf.Max(0, health-100);
        UpdateLife();
        timeSinceLastDamage = 0f;
        
        if(health <= 0) {
            Debug.Log("Game Over");
        }
    }
    
    private void UpdateLife() {
        float healthRatio = health;
        vieVisuel.sizeDelta = new Vector2(healthRatio, vieVisuel.sizeDelta.y);
        vieText.text = health.ToString();
    }
    
    private void ShootAtNearestEnemy() {
        Ennemi[] enemies = FindObjectsByType<Ennemi>(FindObjectsSortMode.None);
        Ennemi nearestEnemy = null;
        float nearestDistance = 15f;

        foreach (Ennemi enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < nearestDistance) {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy) {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position + direction, Quaternion.LookRotation(direction));
            projectile.GetComponent<Tire>().creator = gameObject;
            shootInterval = 5f;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 8f);
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, 15f);
    }
}
