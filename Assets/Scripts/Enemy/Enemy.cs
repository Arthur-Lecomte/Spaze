using UnityEngine;

public class Enemy : MonoBehaviour {
    public delegate void EnemyDestroyedHandler(GameObject enemy);
    public event EnemyDestroyedHandler OnDestroyed;

    private void OnDestroy() {
        OnDestroyed?.Invoke(gameObject);
    }
}
