using UnityEngine;
using System.Collections.Generic;

public class Turret : Construction {
    [SerializeField] private int damage;
    [SerializeField] private float attackSpeed;

    // Propriétés spécifiques à la tourelle
    public int Damage => damage;
    public float AttackSpeed => attackSpeed;
    
    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }

    public override void AdjustStatsByRarity() {
        base.AdjustStatsByRarity(); 

        // Ajuster les dégâts et la vitesse d'attaque
        damage = Mathf.RoundToInt(damage * GetRarityMultiplier());
        attackSpeed *= GetRarityMultiplier();
    }

    public override Dictionary<string, string> GetStats() {
        var stats = base.GetStats();
        stats.Add("Damage", damage.ToString());
        stats.Add("Attack Speed", attackSpeed.ToString("F2"));
        return stats;
    }
}
