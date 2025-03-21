using UnityEngine;

public class Tir : MonoBehaviour {
    private GameObject creator;
    private int damage;
    private float speed;
    private float range;
    private float distanceTraveled;

    public void SetInformations(GameObject objectCreator, int damageValue, float speedValue, float rangeValue) {
        creator = objectCreator;
        damage = damageValue;
        speed = speedValue;
        range = rangeValue;
    }

    private void Update() {
        float distance = speed * Time.deltaTime;
        transform.Translate(Vector3.up * distance);
        distanceTraveled += distance;

        if (distanceTraveled >= range) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (!other.isTrigger) {
            ICanTakeDamage hit = other.GetComponent<ICanTakeDamage>();
            if (hit != null && hit.WhoAmI() != creator) {
                hit.TakeDamage(damage);
                Destroy(gameObject);
            }
        }
    }
}