using UnityEngine;

public interface ICanTakeDamage {
    public void TakeDamage(float damage);
    public bool AmIPlayer();
}
