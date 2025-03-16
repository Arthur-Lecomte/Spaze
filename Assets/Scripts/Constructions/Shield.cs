using UnityEngine;

public class Shield : Construction, ICanTakeDamage {
    [SerializeField] private int maxLife = 50;
    [SerializeField] private int life;
    [SerializeField] private float range = 25;
    private bool isDestroy;

    private SphereCollider trigger;
    private LineRenderer lineRenderer;
    
    private void Awake() {
        life = maxLife;
        
        trigger = GetComponent<SphereCollider>();
        trigger.radius = range;
        
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        DrawCircle();
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

    public override void PerformUpgrade() {
        maxLife += 10;
        life += 10;
        range += 10;
    }
    
    private void IsDestroy(bool destroyed) {
        isDestroy = destroyed;
        
        GetComponent<Collider>().enabled = isDestroy;
        if (isDestroy) {
            //DEBUG!!! Animation de destruction / Changer couleur et fonction de la barre de vie / jouer son cassage
        } else {
            life = maxLife;
        }
    }
    
    public void TakeDamage(int damage) {
        life -= damage;
        if (life <= 0) {
            IsDestroy(true);
        }
    }
}
