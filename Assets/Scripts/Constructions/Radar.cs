using UnityEngine;

public class Radar : Construction {
    [SerializeField] private int damage;
    [SerializeField] private float attackSpeed;

    // Propriétés spécifiques à la tourelle
    public int Damage => damage;
    public float AttackSpeed => attackSpeed;

    public override string ToString() {
        return base.ToString() + $"\nDégâts: {damage}\nVitesse d'attaque: {attackSpeed}\nNiveau: {Niveau}/{niveauMax}";
    }
    
    public override void Upgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
