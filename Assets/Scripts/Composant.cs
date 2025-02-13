using UnityEngine;

public class Composant : MonoBehaviour {
    private Renderer objectRenderer;
    private Renderer childRenderer;
    private Color color;
    private Collider objectCollider;
    public bool isActivate;
    public bool isBuildMode;
    public int currentType;

    void Awake() {
        objectRenderer = GetComponent<Renderer>();
        childRenderer = transform.Find("Plane").GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
        objectRenderer.enabled = false;
        childRenderer.enabled = false;
        objectCollider.enabled = false;
        color = objectRenderer.material.color;
        color.a = 0.25f;
        objectRenderer.material.color = color;
        currentType = -1;
    }
    
    public void ActivateBuildMode(bool value) {
        isBuildMode = value;
        if(!isActivate) {
            TogglePreview(value);
        }
    }

    private void TogglePreview(bool isPreview) {
        objectRenderer.enabled = isPreview;
        objectCollider.enabled = isPreview;
    }
    
    void OnMouseEnter() {
        if (!isActivate) {
            color.a = 0.5f;
            objectRenderer.material.color = color;
        }
    }
    
    void OnMouseDown() {
        if (!isActivate) {
            if(ModeBuild.Instance.HasEnoughResource(10)) {
                ModeBuild.Instance.ChangeResource(-10);
                Activate();
                ModeBuild.Instance.CurrentComposant(this);
            }
        } else if(isBuildMode) {
            ModeBuild.Instance.CurrentComposant(this);
        }
    }

    void OnMouseExit() {
        if (!isActivate) {
            color.a = 0.25f;
            objectRenderer.material.color = color;
        }
    }

    private void Activate() {
        isActivate = true;
        color.a = 1f;
        objectRenderer.material.color = color;
        objectRenderer.enabled = true;
        childRenderer.enabled = true;
        objectCollider.enabled = true;
    }
    
    public void TakeDamage(int damage) {
        PlayerMove.Instance.TakeDamage(damage);
    }

    public void ChangeComposant(int type) {
        Debug.Log(currentType + " " + type);
        switch (currentType) {
            case 0:
                childRenderer.material.color = Color.red;
                PlayerMove.Instance.shootInterval.RemoveAt(PlayerMove.Instance.shootInterval.Count - 1);
                break;
            case 1:
                childRenderer.material.color = Color.blue;
                PlayerMove.Instance.maxHealth -= 50;
                PlayerMove.Instance.UpdateLife();
                break;
            case 2:
                childRenderer.material.color = Color.yellow;
                ModeBuild.Instance.nbResource -= 1;
                break;
        }
        switch (type) {
            case 0:
                childRenderer.material.color = Color.red;
                PlayerMove.Instance.shootInterval.Add(0f);
                break;
            case 1:
                childRenderer.material.color = Color.blue;
                PlayerMove.Instance.maxHealth += 50;
                PlayerMove.Instance.UpdateLife();
                break;
            case 2:
                childRenderer.material.color = Color.yellow;
                ModeBuild.Instance.nbResource += 1;
                break;
        }
        currentType = type;
    }
}
