using UnityEngine;

public class Tir : MonoBehaviour {
    private bool fromPlayer;
    private float damage;
    private float speed;
    private float range;
    private float distanceTraveled;
    
    private float ralentissement = 1;

    public void SetInformations(bool creator, float damageValue, float speedValue, float rangeValue) {
        fromPlayer = creator;
        damage = damageValue;
        speed = speedValue;
        range = rangeValue + 1;
    }

    private void Update() {
        float distance = speed / ralentissement * Time.deltaTime;
        transform.Translate(Vector3.up * distance);
        distanceTraveled += distance;

        if (distanceTraveled >= range) {
            Destroy(gameObject);
        }
    }
    
    public bool IsFromPlayer() {
        return fromPlayer;
    }

    private void OnTriggerEnter(Collider other) {
        ICanTakeDamage hit = other.GetComponent<ICanTakeDamage>();
        if (hit != null && hit.AmIPlayer() != fromPlayer) {
            hit.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
    
    public void InSlowArea(float power) {
        ralentissement += power;
    }
}
