using UnityEngine;

public class Composant : MonoBehaviour {
    [SerializeField] private bool isActivate;

    [SerializeField] private int price;
    
    private Collider objectCollider;
    private Renderer objectRenderer;
    private Color color;
    public int currentType;

    void Awake() {
        objectCollider = GetComponent<Collider>();
        objectRenderer = GetComponent<Renderer>();
        color = objectRenderer.material.color;
        
        if(!isActivate) {
            objectCollider.enabled = false;
            objectRenderer.enabled = false;
            color.a = 0.25f;
            objectRenderer.material.color = color;
            currentType = -1;
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
            //DEBUG!!! Regarder si j'ai assez de ressources pour acheter le composant [WaitFor ShopManager]
            if(true){ //ShopManager.Instance.Buy(this)
                Activate();
                BuildManager.Instance.CurrentComposant(this);
            }
            
        } else if(BuildManager.Instance.InBuildMode()) {
            BuildManager.Instance.CurrentComposant(this);
        }
    }

    void OnMouseExit() {
        if (!isActivate) {
            color.a = 0.25f;
            objectRenderer.material.color = color;
        }
    }
    
    public void ToggleBuildMode(bool value) {
        if(!isActivate) {
            TogglePreview(value);
        }
    }

    private void TogglePreview(bool isPreview) {
        objectCollider.enabled = isPreview;
        objectRenderer.enabled = isPreview;
    }

    private void Activate() {
        isActivate = true;
        color.a = 1f;
        objectRenderer.material.color = color;
        objectRenderer.enabled = true;
        objectCollider.enabled = true;
    }
    
    public void TakeDamage(int damage) {
        //DEBUG!!! Renvoie les dégâts au joueur ou les absorbe (à voir avec l'équipe) [WaitFor xxx]
    }

    public void ChangeBuilding(int type) {
        //DEBUG!!! Changer la construction du vaisseau [WaitFort Building]
    }
}