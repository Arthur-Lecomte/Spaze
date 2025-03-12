using UnityEngine;

public class Slower : Construction {
    [SerializeField] private int damage;
    [SerializeField] private float attackSpeed;

    // Propriétés spécifiques à la tourelle
    public int Damage => damage;
    public float AttackSpeed => attackSpeed;
    
    public override void Upgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
