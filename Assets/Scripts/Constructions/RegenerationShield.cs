using UnityEngine;

public class RegenerationShield : SearchShield {
    [SerializeField] private float capacity;
    [SerializeField] private float regenerationMax;
    [SerializeField] private float range;
    
    private float regenerationPerShield;
    public float RegenerationPerShield => regenerationPerShield;

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        range = 1; // DEBUG !!!
        TypeToSearch = typeof(Shield);
        
        NewShield += ListSearchShields;
    }
    
    public override void SetChildOf(Transform parent, bool onModule = true) {
        base.SetChildOf(parent, onModule);

        ListSearchShields();
    }
    
    private void ListSearchShields() {
        foreach (SearchShield searchShield in NearbySearchShields) {
            searchShield.RemoveSearchShield(this);
        }

        AddonModule moduleOn = transform.parent.GetComponent<AddonModule>();
        if (moduleOn != null) {
            NearbySearchShields = moduleOn.GetSearchShieldAtDistance(TypeToSearch, (int)range);
            foreach (SearchShield shield in NearbySearchShields) {
                shield.AddSearchShield(this);
            }
        }
        
        regenerationPerShield = Mathf.Min(regenerationMax, capacity / NearbySearchShields.Count);
    }
    
    protected override void PerformUpgrade() {
        ListSearchShields();
    }
}
