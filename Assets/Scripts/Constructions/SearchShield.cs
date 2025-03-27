using System;
using System.Collections.Generic;

public class SearchShield : Construction {
    protected Type TypeToSearch;
    protected List<SearchShield> NearbySearchShields;

    protected static Action newShield;

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        
        NearbySearchShields = new List<SearchShield>();
    }
    
    public void AddSearchShield(SearchShield searchShield) {
        NearbySearchShields.Add(searchShield);
    }
    
    public void RemoveSearchShield(SearchShield searchShield) {
        NearbySearchShields.Remove(searchShield);
    }
}
