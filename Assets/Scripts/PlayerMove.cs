using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public static PlayerMove Instance;

    public float moveSpeed = 10f;
    public float rotationSpeed = 400f;
    public float pourcentageSpeed = 0.4f;

    public int maxHealth = 100;
    public float pourcentageHealth = 0.4f;
    public int health;
    private float timeSinceLastDamage;
    public TextMeshProUGUI vieText;
    public RectTransform vieVisuel;

    public GameObject projectilePrefab;
    public List<float> shootInterval;
    public float pourcentageDamage = 0.4f;

    void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        shootInterval = new List<float> {0f};
        health = (int)(maxHealth * pourcentageHealth);
        UpdateLife();
    }

    void Update() {
        // Avancer
        if(Input.GetKey(KeyCode.W)) {
            transform.Translate(Vector3.forward * moveSpeed * pourcentageSpeed * Time.deltaTime);
        }

        // Rotation à gauche
        if(Input.GetKey(KeyCode.A)) {
            transform.Rotate(Vector3.down * rotationSpeed * pourcentageSpeed * Time.deltaTime);
        }

        // Rotation à droite
        if(Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.up * rotationSpeed * pourcentageSpeed * Time.deltaTime);
        }

        // Health
        timeSinceLastDamage += Time.deltaTime;
        if(timeSinceLastDamage >= 5f) {
            health = Mathf.Min(health + 1, (int)(maxHealth * pourcentageHealth));
            UpdateLife();
            timeSinceLastDamage = 4.5f;
        }

        // Shooting
        for(int i = 0; i < shootInterval.Count; i++) {
            shootInterval[i] -= Time.deltaTime;
            if(shootInterval[i] <= 0) {
                ShootAtNearestEnemy(i);
            }
        }
    }

    public void TakeDamage(int damage) {
        health = Mathf.Max(0, health - damage);
        UpdateLife();
        timeSinceLastDamage = 0f;

        if(health <= 0) {
            Debug.Log("Game Over");
        }
    }

    public void UpgradeLife(float value) {
        pourcentageHealth = value;
        UpdateLife();
    }

    public void UpgradeDamage(float value) {
        pourcentageDamage = value;
    }

    public void UpgradeSpeed(float value) {
        pourcentageSpeed = value;
    }

    public void UpdateLife() {
        health = Mathf.Min(health, (int)(maxHealth * pourcentageHealth));
        vieVisuel.sizeDelta = new Vector2(health / (maxHealth * pourcentageHealth) * 1000, vieVisuel.sizeDelta.y);
        vieText.text = health + " / " + maxHealth * pourcentageHealth;
    }

    private void ShootAtNearestEnemy(int i) {
        Ennemi[] enemies = FindObjectsByType<Ennemi>(FindObjectsSortMode.None);
        Ennemi nearestEnemy = null;
        float nearestDistance = 15f;

        foreach(Ennemi enemy in enemies) {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if(distance < nearestDistance && enemy.timeBeforeBeingCible <= 0) {
                nearestDistance = distance;
                nearestEnemy = enemy;
            }
        }

        if(nearestEnemy) {
            Vector3 direction = (nearestEnemy.transform.position - transform.position).normalized;
            GameObject projectile = Instantiate(projectilePrefab, transform.position + direction, Quaternion.LookRotation(direction));
            projectile.GetComponent<Tire>().creator = gameObject;
            projectile.GetComponent<Tire>().damage = (int)(45 * pourcentageDamage);
            nearestEnemy.timeBeforeBeingCible = 2f;
            shootInterval[i] = 5f;
        }
    }

    void OnDrawGizmos() {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, 8f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 15f);
    }
}
