using UnityEngine;

public class Speed : Construction {
    [SerializeField] private float additionalSpeed;
    
    public float AdditionalSpeed => additionalSpeed;

    public override void PerformUpgrade() {
        throw new System.NotImplementedException(); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
