using UnityEngine;

public class Turret : Construction {
    [SerializeField] private int damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float range;

    // Propriétés spécifiques à la tourelle
    public int Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float Range => range;

    public override string ToString() {
        return base.ToString() + $"\nDégâts: {damage}\nVitesse d'attaque: {attackSpeed}\nPortée: {range}";
    }
    
    public override void Upgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
