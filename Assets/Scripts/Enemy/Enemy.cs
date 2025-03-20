using UnityEngine;

public class Enemy : MonoBehaviour, ICanTakeDamage {
    public int Level { get; set; } = 1;

    public delegate void EnemyDestroyedHandler(GameObject enemy);
    public event EnemyDestroyedHandler OnDestroyed;

    private void OnDestroy() {
        OnDestroyed?.Invoke(gameObject);
    }
    
    public void TakeDamage(float damage) {
        //TakeDamage
    }

    public GameObject WhoAmI() {
        return gameObject;
    }
}
