using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionInformationsUI : MonoBehaviour {
    public static ConstructionInformationsUI Instance;
    
    [SerializeField] private TextMeshProUGUI typeText;
    [SerializeField] private TextMeshProUGUI rarityText;
    [SerializeField] private TextMeshProUGUI niveauText;
    [SerializeField] private Transform container;
    [SerializeField] private TMP_FontAsset fontAsset;
    
    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
        }
        
        HideHoverUI();
    }

    public void ShowHoverUI(Construction construction) {
        typeText.text = construction.GetType().Name;
        rarityText.text = construction.GetRarityText();
        rarityText.color = construction.GetRarityColor();
        niveauText.text = $"{construction.GetNiveau()}/5";
        
        foreach (Transform child in container) {
            Destroy(child.gameObject);
        }
        foreach (var stat in construction.GetStats()) {
            GameObject statText = new GameObject(stat.Key, typeof(TextMeshProUGUI));
            statText.transform.SetParent(container);
            statText.transform.localScale = Vector3.one;

            TextMeshProUGUI textComponent = statText.GetComponent<TextMeshProUGUI>();
            textComponent.text = $"{stat.Key}: {stat.Value}";
            textComponent.font = fontAsset;
            textComponent.fontSize = 20;
        }
        
        gameObject.SetActive(true);
    }

    public void HideHoverUI() {
        gameObject.SetActive(false);
    }
}
