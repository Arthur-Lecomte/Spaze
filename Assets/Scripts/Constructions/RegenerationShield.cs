using System.Collections.Generic;
using UnityEngine;

public class RegenerationShield : SearchShield {
    [SerializeField] private float capacity;
    [SerializeField] private float regenerationMax;
    [SerializeField] private float range;

    private float regenerationPerShield;
    public float RegenerationPerShield => regenerationPerShield;

    private Dictionary<Shield, LaserBeam> laserBeams;
    [SerializeField] private GameObject laserPrefab;
    [SerializeField] private GameObject firePoint;

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);
        range = 1; // DEBUG !!!
        TypeToSearch = typeof(Shield);
        laserBeams = new Dictionary<Shield, LaserBeam>();
        newShield += ListSearchShields;
    }

    public override void SetChildOf(Transform parent, bool onModule = true) {
        base.SetChildOf(parent, onModule);

        ListSearchShields();
    }

    private void ListSearchShields() {
        foreach (LaserBeam laserBeam in laserBeams.Values) {
            laserBeam.DisableLaser();
            Destroy(laserBeam);
        }
        laserBeams.Clear();

        foreach (SearchShield searchShield in NearbySearchShields) {
            searchShield.RemoveSearchShield(this);
        }
        NearbySearchShields = new List<SearchShield>();

        if (firePoint) {
            foreach (Transform child in firePoint.transform) {
                Destroy(child.gameObject);
            }
        }
        
        if (this == null) {
            Debug.Log("BUG");
            newShield -= ListSearchShields;
            return;
        }

        AddonModule moduleOn = transform.parent.GetComponent<AddonModule>();
        if (moduleOn != null) {
            NearbySearchShields = moduleOn.GetSearchShieldAtDistance(TypeToSearch, (int)range);
            foreach (SearchShield shield in NearbySearchShields) {
                shield.AddSearchShield(this);
            }
        }

        regenerationPerShield = Mathf.Min(regenerationMax, capacity / NearbySearchShields.Count);
        foreach (SearchShield t in NearbySearchShields) {
            LaserBeam lb = gameObject.AddComponent<LaserBeam>();
            lb.collisionMask = LayerMask.GetMask("Default");
            lb.firePoint = firePoint;
            lb.laser = Instantiate(laserPrefab, firePoint.transform);
            lb.ignoreObjectInFront = true;
            lb.DisableLaser();
            laserBeams[(Shield)t] = lb;
        }
    }

    protected override void PerformUpgrade() {
        ListSearchShields();
    }

    public void ActiveLaserBeam(Shield shield, bool active) {
        if (this == null) {
            Debug.Log("BUG2");
            return;
        }
        
        if (active) {
            laserBeams[shield].EnableLaser(shield.transform);
        } else {
            laserBeams[shield].DisableLaser();
        }
    }

    private void OnDestroy() {
        newShield -= ListSearchShields;
        
        foreach (LaserBeam laserBeam in laserBeams.Values) {
            laserBeam.DisableLaser();
            Destroy(laserBeam);
        }
        laserBeams.Clear();

        foreach (SearchShield searchShield in NearbySearchShields) {
            searchShield.RemoveSearchShield(this);
        }
        NearbySearchShields = new List<SearchShield>();

        if (firePoint) {
            foreach (Transform child in firePoint.transform) {
                Destroy(child.gameObject);
            }
        }
    }
}
