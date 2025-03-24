using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shield : Construction, ICanTakeDamage {
    [SerializeField] private float maxLife = 50;
    private float life;
    private float regeneration = 5;
    [SerializeField] private float range = 3;

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
    }

    private void ChangeLife(float quantity) {
        life = Mathf.Clamp(life + quantity, 0, maxLife);
        lifeBar.rectTransform.sizeDelta = new Vector2(life / maxLife * 150, 25);
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
    }

    private IEnumerator RegenerateShield() {
        yield return new WaitForSeconds(3);

        while (life < maxLife) {
            ChangeLife(regeneration * Time.deltaTime);
            yield return null;
        }
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
