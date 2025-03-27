using System.Collections;
using System.Collections.Generic;
using SmallHedge.SoundManager;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Construction construction;
    private int index;
    
    private Dictionary<Ressource, TextMeshProUGUI> texts;
        
    [SerializeField] private Transform subPanel;
    [SerializeField] private Transform subPanelContainer;
    private Coroutine subPanelCoroutine;
    [SerializeField] private TMP_FontAsset fontAsset;

    [SerializeField] private GameObject noSpace;
    private Coroutine noMoreSpaceCoroutine;
    
    [SerializeField] private Image spritePlace;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private Transform panelConstruction;
    
    private CanvasGroup canvasGroup;
    
    public void Create(int i) {
        index = i;
        transform.localPosition = new Vector3((index - 1) * 600, 0, 0);
        canvasGroup = GetComponent<CanvasGroup>();
    }
    
    public void SetCanvasGroup(float alpha) {
        canvasGroup.alpha = alpha;
    }

    public void Initialisation(Construction constru) {
        construction = constru;
        
        texts = BuyHoverUI.Instance.ShowConstructionUI(panelConstruction, construction.GetNom(), construction.GetCoutRessources());
        RessourcesUI.OnUIUpdated += UIUpdated;
        
        spritePlace.sprite = construction.GetSprite();
        rarityText.text = construction.GetRarityText();
        rarityText.color = construction.GetRarityColor();
        
        foreach (Transform child in subPanelContainer) {
            Destroy(child.gameObject);
        }
        foreach (var stat in construction.GetStats()) {
            GameObject statText = new GameObject(stat.Key, typeof(TextMeshProUGUI));
            statText.transform.SetParent(subPanelContainer);
            statText.transform.localScale = Vector3.one;

            TextMeshProUGUI textComponent = statText.GetComponent<TextMeshProUGUI>();
            textComponent.text = $"{stat.Key}: {stat.Value}";
            textComponent.font = fontAsset;
            textComponent.fontSize = 20;
        }
    }
    
    private void UIUpdated() {
        BuyHoverUI.Instance.UpdateColorUI(texts);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        // Démarrer une coroutine pour ouvrir le panneau
        if (subPanelCoroutine != null) {
            StopCoroutine(subPanelCoroutine);
        }
        subPanel.gameObject.SetActive(true); // Activer le panneau avant de lancer l'animation
        subPanelCoroutine = StartCoroutine(SubPanelCoroutine(new Vector3(index == 2 ? -370 : 370, 0, 0)));
        ShopManager.Instance.DimOtherConstructions(this); // Dim other constructions
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        // Démarrer une coroutine pour fermer le panneau
        if (subPanelCoroutine != null) {
            StopCoroutine(subPanelCoroutine);
        }
        subPanelCoroutine = StartCoroutine(SubPanelCoroutine(Vector3.zero));
        ShopManager.Instance.RestoreConstructionsVisibility(null); // Restore visibility
    }

    private IEnumerator SubPanelCoroutine(Vector3 targetPosition ) {
        if (subPanel) {
            Vector3 initialPosition = subPanel.localPosition;
            float duration = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration) {
                subPanel.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // S'assurer que la position finale est atteinte
            subPanel.localPosition = targetPosition;
            
            if (targetPosition == Vector3.zero) {
                subPanel.gameObject.SetActive(false); // Désactiver le panneau après l'animation
            }
        }
    }
    
    public void HideSubPanel() {
        subPanel.gameObject.SetActive(false);
        subPanel.localPosition = Vector3.zero;
    }

    // Méthode pour acheter une construction
    public void BuyConstruction() {
        // Vérifiez si le joueur à suffisamment de ressources pour acheter la construction
        if (Inventory.Instance.HaveEnoughRessources(construction.GetCoutRessources())) {
            // Si on peut ajouter la construction au vaisseau
            if (Inventory.Instance.AddConstruction(construction)) {
                // Retirer les ressources nécessaires
                Inventory.Instance.RemoveRessources(construction.GetCoutRessources());
                SoundManager.PlaySound(SoundType.BUY);
                StopAllCoroutines();
                gameObject.SetActive(false);
                ShopManager.Instance.IsBuy(index);
                ShopManager.Instance.RestoreConstructionsVisibility(null); // Restore visibility
            } else {
                if (noMoreSpaceCoroutine != null) {
                    StopCoroutine(noMoreSpaceCoroutine);
                }
                noMoreSpaceCoroutine = StartCoroutine(NoMoreSpaZe());
            }
        }
    }
    
    private IEnumerator NoMoreSpaZe() {
        noSpace.SetActive(true);
        yield return new WaitForSeconds(5);
        noSpace.SetActive(false);
    }
}
