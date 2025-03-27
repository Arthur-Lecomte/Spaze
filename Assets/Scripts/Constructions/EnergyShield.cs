using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using SmallHedge.SoundManager;

public class EnergyShield : MonoBehaviour, ICanTakeDamage {
    [SerializeField] private Shield shield;
    
    private MeshRenderer meshRenderer;
    [SerializeField] private RawImage lifeBar;
    
    private Collider trigger;
    
    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
        trigger = GetComponent<Collider>();
    }
    
    public IEnumerator RegenerateShield() {
        float elapsedTime = 0f;

        while (elapsedTime < 3f && shield.GetLife() < shield.GetMaxLife()) {
            FirstPhaseRegenerateShield();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        while (shield.GetLife() < shield.GetMaxLife()) {
            SecondePhaseRegenerateShield();
            yield return null;
        }
        shield.ActiveLaserBeam(false);
    }

    private void FirstPhaseRegenerateShield() {
        float regenerationBonus = 0;
        foreach (SearchShield searchShield in shield.GetNearbySearchShields()) {
            RegenerationShield regenerationShield = (RegenerationShield)searchShield;
            regenerationBonus += regenerationShield.RegenerationPerShield;
        }

        shield.ChangeLife(regenerationBonus * Time.deltaTime);
    }

    private void SecondePhaseRegenerateShield() {
        float regenerationBonus = 0;
        foreach (SearchShield searchShield in shield.GetNearbySearchShields()) {
            RegenerationShield regenerationShield = (RegenerationShield)searchShield;
            regenerationBonus += regenerationShield.RegenerationPerShield;
        }

        shield.ChangeLife((shield.GetRegeneration() + regenerationBonus) * Time.deltaTime);
    }

    public IEnumerator Repair() {
        meshRenderer.enabled = false;
        lifeBar.color = Color.yellow;
        SoundManager.PlaySound(SoundType.BREAKSHIELD);
        
        yield return new WaitForSeconds(1);

        while (shield.GetLife() < shield.GetMaxLife()) {
            shield.ChangeLife(shield.GetRegeneration() / 2 * Time.deltaTime);
            yield return null;
        }
        
        trigger.enabled = true;
        meshRenderer.enabled = true;
        lifeBar.color = Color.blue;
        SoundManager.PlaySound(SoundType.SHIELDRECHARGE);
    }
    
    public void TakeDamage(float damage) {
        StopCoroutine(nameof(RegenerateShield));
        shield.ChangeLife(-damage);
        if (shield.GetLife() == 0) {
            shield.ActiveLaserBeam(false);
            trigger.enabled = false;
            StopCoroutine(nameof(RegenerateShield));
            StartCoroutine(nameof(Repair));
        } else {
            StartCoroutine(nameof(RegenerateShield));
            shield.ActiveLaserBeam(true);
        }
    }

    public bool AmIPlayer() {
        return true;
    }
}
