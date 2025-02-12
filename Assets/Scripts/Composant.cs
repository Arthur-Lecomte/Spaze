using UnityEngine;

public class Composant : MonoBehaviour {
    private Renderer objectRenderer;
    private Collider objectCollider;
    public bool isActivate;

    void Awake() {
        objectRenderer = GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
        objectRenderer.enabled = false;
        objectCollider.enabled = false;
        Color color = objectRenderer.material.color;
        color.a = 0.25f;
        objectRenderer.material.color = color;
    }

    void Update() {
    }

    public void TogglePreview(bool isPreview) {
        if(!isActivate) {
            objectRenderer.enabled = isPreview;
        }
    }

    public void Activate() {
        objectRenderer.enabled = true;
        Color color = objectRenderer.material.color;
        color.a = 1f;
        objectRenderer.material.color = color;

        objectCollider.enabled = true;
    }
}
