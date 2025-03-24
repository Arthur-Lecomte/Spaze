using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shield : SearchShield, ICanTakeDamage {
    [SerializeField] private float maxLife;
    private float life;
    private float regeneration;
    private float range;

    private SphereCollider trigger;
    private LineRenderer lineRenderer;

    [SerializeField] private RawImage lifeBar;

    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);

        ChangeLife(maxLife);

        trigger = GetComponent<SphereCollider>();
        trigger.radius = range;

        lineRenderer = gameObject.GetComponent<LineRenderer>();
        DrawCircle();

        TypeToSearch = typeof(RegenerationShield);
    }
    
    public override void SetChildOf(Transform parent, bool onModule = true) {
        base.SetChildOf(parent, onModule);

        NewShield?.Invoke();
    }

    private void ChangeLife(float quantity) {
        life = Mathf.Clamp(life + quantity, 0, maxLife);
        lifeBar.rectTransform.sizeDelta = new Vector2(life / maxLife * 100, 20);
    }

    private void DrawCircle() {
        int segments = 100;
        lineRenderer.positionCount = segments;
        float angle = 0f;

        for (int i = 0; i < segments; i++) {
            float x = Mathf.Sin(Mathf.Deg2Rad * angle) * range;
            float z = Mathf.Cos(Mathf.Deg2Rad * angle) * range;
            lineRenderer.SetPosition(i, new Vector3(x, 0, z));
            angle += 360f / segments;
        }
    }

    protected override void PerformUpgrade() {
        life = maxLife; //Régénère entièrement le shield en s'améliorant
        trigger.radius = range; //Augmente la portée du shield
        DrawCircle(); //Redessine le cercle
    }

    private IEnumerator RegenerateShield() {
        float elapsedTime = 0f;

        while (elapsedTime < 3f && life < maxLife) {
            FirstPhaseRegenerateShield();
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        
        while (life < maxLife) {
            SecondePhaseRegenerateShield();
            yield return null;
        }
    }

    private void FirstPhaseRegenerateShield() {
        float regenerationBonus = 0;
        foreach (SearchShield searchShield in NearbySearchShields) {
            RegenerationShield regenerationShield = (RegenerationShield)searchShield;
            regenerationBonus += regenerationShield.RegenerationPerShield;
        }

        ChangeLife(regenerationBonus * Time.deltaTime);
    }

    private void SecondePhaseRegenerateShield() {
        float regenerationBonus = 0;
        foreach (SearchShield searchShield in NearbySearchShields) {
            RegenerationShield regenerationShield = (RegenerationShield)searchShield;
            regenerationBonus += regenerationShield.RegenerationPerShield;
        }

        ChangeLife((regeneration + regenerationBonus) * Time.deltaTime);
    }

    private IEnumerator Repair() {
        //DEBUG!!! play sound broken shield + animation destroy shield
        GetComponent<Collider>().enabled = false;
        lineRenderer.enabled = false;
        lifeBar.color = Color.yellow;
        yield return new WaitForSeconds(1);

        while (life < maxLife) {
            ChangeLife(regeneration / 2 * Time.deltaTime);
            yield return null;
        }

        lifeBar.color = Color.blue;
        GetComponent<Collider>().enabled = true;
        lineRenderer.enabled = true;
        //DEBUG!!! play sound repair shield + animation repair shield
    }

    public void TakeDamage(float damage) {
        StopCoroutine(nameof(RegenerateShield));
        ChangeLife(-damage);
        if (life == 0) {
            StopCoroutine(nameof(RegenerateShield));
            StartCoroutine(Repair());
        } else {
            StartCoroutine(nameof(RegenerateShield));
        }
    }

    public bool AmIPlayer() {
        return true;
    }
}
