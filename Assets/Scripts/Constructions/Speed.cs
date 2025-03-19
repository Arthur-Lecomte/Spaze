using UnityEngine;

public class Speed : Construction {
    [SerializeField] private float additionalSpeed;
    
    public float AdditionalSpeed => additionalSpeed;

    protected override void PerformUpgrade() {
        Debug.LogWarning(GetType() +" PerformUpgrade Not Implemented"); // Augmenter le niveau de la construction (changer damage, attackSpeed...)
    }
}
