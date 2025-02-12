using UnityEngine;

public class Composant : MonoBehaviour {
    private Renderer objectRenderer;
    private Color color;
    private Collider objectCollider;
    public bool isActivate;

    void Awake() {
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
        objectRenderer.enabled = false;
        objectCollider.enabled = false;
        color = objectRenderer.material.color;
        color.a = 0.25f;
        objectRenderer.material.color = color;
    }

    public void TogglePreview(bool isPreview) {
        if(!isActivate) {
            objectRenderer.enabled = isPreview;
            objectCollider.enabled = isPreview;
        }
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
            }
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
        objectCollider.enabled = true;
    }
    
    public void TakeDamage() {
        PlayerMove.Instance.TakeDamage();
    }
}
