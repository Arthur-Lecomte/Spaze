using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnergyShield : MonoBehaviour, ICanTakeDamage {
    [SerializeField] private Shield shield;
    
    private MeshRenderer meshRenderer;
    [SerializeField] private RawImage lifeBar;
    
    private void Awake() {
        meshRenderer = GetComponent<MeshRenderer>();
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
        gameObject.layer = 2;
        meshRenderer.enabled = false;
        lifeBar.color = Color.yellow;
        //DEBUG!!! play sound broken shield + animation destroy shield
        
        yield return new WaitForSeconds(1);

        while (shield.GetLife() < shield.GetMaxLife()) {
            shield.ChangeLife(shield.GetRegeneration() / 2 * Time.deltaTime);
            yield return null;
        }
        
        gameObject.layer = 7;
        meshRenderer.enabled = true;
        lifeBar.color = Color.blue;
        //DEBUG!!! play sound repair shield + animation repair shield
    }
    
    public void TakeDamage(float damage) {
        StopCoroutine(nameof(RegenerateShield));
        shield.ChangeLife(-damage);
        if (shield.GetLife() == 0) {
            StopCoroutine(nameof(RegenerateShield));
            StartCoroutine(nameof(Repair));
        } else {
            StartCoroutine(nameof(RegenerateShield));
        }
    }

    public bool AmIPlayer() {
        return true;
    }
}
