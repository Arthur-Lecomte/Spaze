using UnityEngine;

public class Enemy : MonoBehaviour {
    public int Level { get; set; } = 1;

    public delegate void EnemyDestroyedHandler(GameObject enemy);
    public event EnemyDestroyedHandler OnDestroyed;

    private void OnDestroy() {
        OnDestroyed?.Invoke(gameObject);
    }
}
