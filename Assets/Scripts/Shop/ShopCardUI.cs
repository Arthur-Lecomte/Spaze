using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopCardUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private Transform subPanel;
    [SerializeField] private TMP_FontAsset fontAsset;

    private Construction construction;
    private int index;
    
    private Dictionary<Ressource, TextMeshProUGUI> texts;

    public void Initialisation(Construction constru, int i) {
        construction = constru;
        index = i;
        transform.localPosition = new Vector3(index * 600, 0, 0);

        // Configurer l'élément UI
        Image sprite = transform.Find("PanelConstruction").Find("Sprite")?.GetComponent<Image>();
        TextMeshProUGUI rarity = transform.Find("PanelConstruction").Find("Rarity Zone")?.GetComponent<TextMeshProUGUI>();
        //DEBUG!!! seulement le dernier panel voit ces couleurs mise à jour !!!
        texts = BuyHoverUI.Instance.ShowConstructionUI(transform.Find("PanelConstruction"), construction.GetNom(), construction.GetCoutRessources());
        RessourcesUI.OnUIUpdated += UIUpdated;
        
        sprite.sprite = construction.GetSprite();
        rarity.text = construction.GetRarityText();
        rarity.color = construction.GetRarityColor();

        // Configurer le bouton d'achat
        transform.Find("PanelConstruction").Find("AcheterButton")?.GetComponent<Button>().onClick.AddListener(() => BuyConstruction());

        // Affiche les valeurs des statistiques
        subPanel = transform.Find("SubPanel");
        foreach (var stat in construction.GetStats()) {
            GameObject statText = new GameObject(stat.Key, typeof(TextMeshProUGUI));
            statText.transform.SetParent(subPanel.Find("ContainerStats"));
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
        StopAllCoroutines();
        subPanel.gameObject.SetActive(true); // Activer le panneau avant de lancer l'animation
        StartCoroutine(SubPanelCoroutine(new Vector3(index == 1 ? -370 : 370, 0, 0)));
        ShopManager.Instance.DimOtherConstructions(gameObject); // Dim other constructions
    }
    
    public void OnPointerExit(PointerEventData eventData) {
        // Démarrer une coroutine pour fermer le panneau
        StopAllCoroutines();
        StartCoroutine(SubPanelCoroutine(Vector3.zero));
        ShopManager.Instance.RestoreConstructionsVisibility(); // Restore visibility
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
        if (subPanel) {
            subPanel.gameObject.SetActive(false);
            subPanel.localPosition = Vector3.zero;
        }
    }

    // Méthode pour acheter une construction
    private void BuyConstruction() {
        // Vérifiez si le joueur à suffisamment de ressources pour acheter la construction
        if (Inventory.Instance.HaveEnoughRessources(construction.GetCoutRessources())) {
            // Si on peut ajouter la construction au vaisseau
            if (Inventory.Instance.AddConstruction(construction)) {
                // Retirer les ressources nécessaires
                Inventory.Instance.RemoveRessources(construction.GetCoutRessources());
                Destroy(gameObject);
                ShopManager.Instance.RestoreConstructionsVisibility(); // Restore visibility
            }
        }
    }

    public void OnDestroy() {
        StopAllCoroutines();
        RessourcesUI.OnUIUpdated -= UIUpdated;
    }
}
