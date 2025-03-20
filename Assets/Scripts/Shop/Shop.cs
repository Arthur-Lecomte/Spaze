using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Shop : MonoBehaviour {
    private ShopOption currentOption;

    [SerializeField] private GameObject[] prefabsConstructions;
    
    [SerializeField] private Transform conteneurConstructions;
    [SerializeField] private GameObject prefabConstructionItem;
    [SerializeField] private Transform zoneForPrefab;
    [SerializeField] private GameObject shopPanel;

    private Dictionary<GameObject, bool> panelStates = new Dictionary<GameObject, bool>(); // État ouvert/fermé
    private Dictionary<GameObject, bool> panelTransitions = new Dictionary<GameObject, bool>(); // Transition en cours
    //private bool isTransitioning = false; // Indique si une transition est en cours


    private bool isShopActive = true;

    private void Start() {
        // Appeler la fonction pour sélectionner et afficher les constructions
        SelectionnerEtAfficherConstructions();
        ToggleShop(false);
    }

    public void ToggleShop(bool state) {
        if (state) {
            isShopActive = true;
            shopPanel.SetActive(isShopActive);
        } else {
            isShopActive = false;
            shopPanel.SetActive(isShopActive);
        }
    }

    private void SelectionnerEtAfficherConstructions() {
        // Liste temporaire pour stocker les instances des prefabs
        List<GameObject> instancesConstructions = new List<GameObject>();

        // Méthode locale pour recréer les instances
        List<GameObject> RecreateInstances() {
            List<GameObject> newInstances = new List<GameObject>();
            foreach (GameObject prefab in prefabsConstructions) {
                GameObject instance = Instantiate(prefab, zoneForPrefab, true);
                newInstances.Add(instance);

                // Assigner une rareté aléatoire à chaque instance
                Construction construction = instance.GetComponent<Construction>();
                if (construction != null) {
                    construction.AssignRandomRarity();
                }
            }
            return newInstances;
        }

        // Créer les premières instances
        instancesConstructions = RecreateInstances();

        // Filtrer les instances pour ne garder que les tourelles
        GameObject[] tourelleInstances = instancesConstructions
            .Where(instance => instance.GetComponent<Turret>() != null)
            .ToArray();

        // Sélectionner une tourelle aléatoire parmi les tourelles filtrées
        GameObject selectedTourelle = SelectPrefabWithRarityAndProbability(tourelleInstances);

        // Recréer les instances après la sélection
        instancesConstructions = RecreateInstances();

        // Sélectionner deux autres constructions aléatoires parmi les nouvelles instances
        GameObject[] autresInstances = instancesConstructions
            .Where(instance => instance != selectedTourelle)
            .ToArray();

        GameObject selectedInstance1 = SelectPrefabWithRarityAndProbability(autresInstances);

        // Recréer les instances une dernière fois pour garantir l'unicité
        instancesConstructions = RecreateInstances();
        autresInstances = instancesConstructions
            .Where(instance => instance != selectedTourelle && instance != selectedInstance1)
            .ToArray();

        GameObject selectedInstance2 = SelectPrefabWithRarityAndProbability(autresInstances);

        // Récupérer les composants Construction des objets instanciés
        Construction constructionTourelle = selectedTourelle.GetComponent<Construction>();
        Construction construction1 = selectedInstance1.GetComponent<Construction>();
        Construction construction2 = selectedInstance2.GetComponent<Construction>();

        constructionTourelle.AdjustStatsByRarity();
        construction1.AdjustStatsByRarity();
        construction2.AdjustStatsByRarity();

        // Afficher les trois constructions dans l'interface utilisateur
        AfficherConstruction(construction2, 1);
        AfficherConstruction(construction1, 0);
        AfficherConstruction(constructionTourelle, -1);
    }

    private GameObject SelectPrefabWithRarityAndProbability(GameObject[] instances) {
        // Créer une liste pondérée en fonction de la rareté et de la probabilité
        List<GameObject> weightedList = new List<GameObject>();
        foreach (GameObject instance in instances) {
            Construction construction = instance.GetComponent<Construction>();
            if (construction != null) {
                int weight = GetWeight(construction.GetRarity(), construction.GetProbability());
                //Debug.Log($"Instance: {instance.name}, Rarity: {construction.GetRarity()}, Probability: {construction.GetProbability()}, Weight: {weight}");
                for (int i = 0; i < weight; i++) {
                    weightedList.Add(instance);
                }
            }
        }

        if (weightedList.Count == 0) {
            Debug.LogError("La liste pondérée est vide. Aucun prefab n'a été sélectionné.");
            return null;
        }

        // Sélectionner un prefab aléatoire dans la liste pondérée
        int randomIndex = Random.Range(0, weightedList.Count);
        return weightedList[randomIndex];
    }

    private int GetWeight(RarityConstruction rarity, float probability) {
        int baseWeight = 1; // Poids de base pour les calculs
        int rarityWeight = (int)rarity; // Utiliser la valeur de l'énumération directement
        return Mathf.RoundToInt(baseWeight * probability * rarityWeight);
    }

    private void AfficherConstruction(Construction construction, int index) {
        // Instancier le prefab de l'élément UI
        GameObject constructionItem = Instantiate(prefabConstructionItem, conteneurConstructions);
        constructionItem.transform.localPosition = new Vector3(index * 600, 0, 0);

        // Configurer l'élément UI
        TextMeshProUGUI nom = constructionItem.transform.Find("PanelConstruction").Find("Name")?.GetComponent<TextMeshProUGUI>();
        Image sprite = constructionItem.transform.Find("PanelConstruction").Find("Sprite")?.GetComponent<Image>();
        TextMeshProUGUI rarity = constructionItem.transform.Find("PanelConstruction").Find("Rarity Zone")?.GetComponent<TextMeshProUGUI>();

        // Liste des champs de texte pour les ressources
        TextMeshProUGUI[] resourceTexts = new TextMeshProUGUI[] {
            constructionItem.transform.Find("PanelConstruction").Find("Cout Cuivre")?.GetComponent<TextMeshProUGUI>(),
            constructionItem.transform.Find("PanelConstruction").Find("Cout Argent")?.GetComponent<TextMeshProUGUI>(),
            constructionItem.transform.Find("PanelConstruction").Find("Cout Or")?.GetComponent<TextMeshProUGUI>(),
            constructionItem.transform.Find("PanelConstruction").Find("Cout Platine")?.GetComponent<TextMeshProUGUI>(),
            constructionItem.transform.Find("PanelConstruction").Find("Cout Noyau Energie")?.GetComponent<TextMeshProUGUI>()
        };

        if (construction != null) {
            nom.text = construction.GetNom();
            sprite.sprite = construction.GetSprite();
            rarity.text = Enum.GetName(typeof(RarityConstruction), construction.GetRarity());
            rarity.color = GetColorForRarity(construction.GetRarity());

            // Mettre à jour les coûts des ressources //DEBUG!!! à faire attention, si on met une ressource, toutes les ressources inférieures doivent être présentes !!!
            for (int i = 0; i < resourceTexts.Length; i++) {
                if (i < construction.GetCoutRessources().Count) {
                    resourceTexts[i].text = construction.GetCoutRessources()[i].quantite.ToString();
                } else {
                    resourceTexts[i].text = "0"; // Valeur par défaut si aucune ressource
                }
            }

            // Configurer le bouton d'achat
            Button acheterButton = constructionItem.transform.Find("PanelConstruction").Find("AcheterButton")?.GetComponent<Button>();
            acheterButton.onClick.AddListener(() => BuyConstruction(construction));
        } else {
            Debug.LogError("AfficherConstruction: Construction component is missing on the prefab.");
        }

        EventTrigger eventTrigger = constructionItem.AddComponent<EventTrigger>();

        // Ajouter l'événement OnPointerEnter
        EventTrigger.Entry pointerEnterEntry = new EventTrigger.Entry();
        pointerEnterEntry.eventID = EventTriggerType.PointerEnter;
        pointerEnterEntry.callback.AddListener((eventData) => {
            OpenSubPanel(constructionItem);
            DimOtherConstructions(constructionItem); // Dim other constructions
        });
        eventTrigger.triggers.Add(pointerEnterEntry);

        // Ajouter l'événement OnPointerExit 

        EventTrigger.Entry pointerExitEntry = new EventTrigger.Entry();
        pointerExitEntry.eventID = EventTriggerType.PointerExit;
        pointerExitEntry.callback.AddListener((eventData) => {
            CloseSubPanel(constructionItem);
            RestoreConstructionsVisibility(); // Restore visibility
        });
        eventTrigger.triggers.Add(pointerExitEntry);

        panelStates[constructionItem] = false; // Par défaut, le panneau est fermé
        panelTransitions[constructionItem] = false; // Par défaut, aucune transition n'est en cours

        // Display stats dynamically
        Transform subPanel = constructionItem.transform.Find("SubPanel");
        if (subPanel != null) {
            // Réinitialiser la position et la taille du SubPanel
            RectTransform rectTransform = subPanel.GetComponent<RectTransform>();
            rectTransform.localPosition = Vector3.zero; // Position par défaut
            rectTransform.sizeDelta = new Vector2(349, 551); // Taille par défaut (ajustez selon vos besoins)

            float yOffset = -50; // Start slightly below the middle top
            foreach (var stat in construction.GetStats()) {
                GameObject statText = new GameObject(stat.Key, typeof(TextMeshProUGUI));
                statText.transform.SetParent(subPanel);

                TextMeshProUGUI textComponent = statText.GetComponent<TextMeshProUGUI>();
                textComponent.text = $"{stat.Key}: {stat.Value}";
                textComponent.fontSize = 20;
                textComponent.alignment = TextAlignmentOptions.Left;

                RectTransform statRectTransform = textComponent.rectTransform;
                statRectTransform.anchorMin = new Vector2(0.6f, 1); // Middle top
                statRectTransform.anchorMax = new Vector2(0.6f, 1); // Middle top
                statRectTransform.pivot = new Vector2(0.5f, 1); // Pivot at the middle top
                statRectTransform.anchoredPosition = new Vector2(0, yOffset); // Offset from the middle top
                yOffset -= 40; // Move down for the next stat
            }
        }
    }

    private void OpenSubPanel(GameObject constructionItem) {
        // Vérifier si le panneau est déjà ouvert ou en transition
        if (panelStates.ContainsKey(constructionItem) && panelStates[constructionItem]) {
            return; // Ne rien faire si le panneau est déjà ouvert
        }
        if (panelTransitions.ContainsKey(constructionItem) && panelTransitions[constructionItem]) {
            return; // Ne rien faire si une transition est en cours
        }

        // Marquer le panneau comme ouvert
        panelStates[constructionItem] = true;

        // Démarrer une coroutine pour ouvrir le panneau
        StartCoroutine(OpenSubPanelCoroutine(constructionItem));
    }

    private IEnumerator OpenSubPanelCoroutine(GameObject constructionItem) {
        Transform subPanel = constructionItem.transform.Find("SubPanel");

        if (subPanel) {
            Debug.Log("ouverture du panneau");
            // Marquer la transition comme en cours
            panelTransitions[constructionItem] = true;

            // Activer le panneau avant de lancer l'animation
            subPanel.gameObject.SetActive(true);

            Vector3 initialPosition = subPanel.localPosition;
            Vector3 targetPosition = initialPosition + (constructionItem.transform.localPosition.x == 600 ? new Vector3(-370, 0, 0) : new Vector3(370, 0, 0)); // Reverse for index 1
            float duration = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration) {
                subPanel.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // S'assurer que la position finale est atteinte
            subPanel.localPosition = targetPosition;

            // Marquer la transition comme terminée
            panelTransitions[constructionItem] = false;
        }
    }

    private void CloseSubPanel(GameObject constructionItem) {
        // Vérifier si le panneau est déjà fermé
        if (panelStates.ContainsKey(constructionItem) && !panelStates[constructionItem]) {
            return; // Ne rien faire si le panneau est déjà fermé
        }

        // Marquer le panneau comme fermé
        panelStates[constructionItem] = false;

        // Démarrer une coroutine pour fermer le panneau
        StartCoroutine(CloseSubPanelCoroutine(constructionItem));
    }

    private IEnumerator CloseSubPanelCoroutine(GameObject constructionItem) {
        Transform subPanel = constructionItem.transform.Find("SubPanel");

        if (subPanel) {
            // Attendre que l'ouverture soit terminée
            while (panelTransitions.ContainsKey(constructionItem) && panelTransitions[constructionItem]) {
                yield return null;
            }

            // Vérifier si l'objet a été détruit
            if (!constructionItem || !subPanel) {
                yield break; // Arrêter la coroutine si l'objet n'existe plus
            }

            // Marquer la transition comme en cours
            panelTransitions[constructionItem] = true;

            Vector3 initialPosition = subPanel.localPosition;
            Vector3 targetPosition = initialPosition + (constructionItem.transform.localPosition.x == 600 ? new Vector3(370, 0, 0) : new Vector3(-370, 0, 0)); // Reverse for index 1
            float duration = 0.5f;
            float elapsedTime = 0f;

            while (elapsedTime < duration) {
                if (!subPanel) {
                    yield break; // Arrêter la coroutine si l'objet a été détruit
                }
                subPanel.localPosition = Vector3.Lerp(initialPosition, targetPosition, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            // S'assurer que la position finale est atteinte
            if (subPanel) {
                subPanel.localPosition = targetPosition;
                subPanel.gameObject.SetActive(false);
            }

            // Marquer la transition comme terminée
            panelTransitions[constructionItem] = false;
        }
    }

    // Méthode pour acheter une construction
    private void BuyConstruction(Construction construction) {
        // Vérifiez si le joueur à suffisamment de ressources pour acheter la construction
        if (!Inventory.Instance.HaveEnoughRessources(construction.GetCoutRessources())) {
            // Si on peut ajouter la construction au vaisseau
            if (Inventory.Instance.AddConstruction(construction)) {
                // Retirer les ressources nécessaires
                Inventory.Instance.RemoveRessources(construction.GetCoutRessources());
            }
        }
    }

    public void ResetConstructions() {
        // Arrêter toutes les coroutines en cours
        StopAllCoroutines();

        // Supprimer toutes les constructions affichées
        foreach (Transform child in conteneurConstructions) {
            Destroy(child.gameObject);
        }
        foreach (Transform child in zoneForPrefab) {
            Destroy(child.gameObject);
        }

        // Réinitialiser les constructions
        SelectionnerEtAfficherConstructions();
    }

    // Méthode pour obtenir la couleur en fonction de la rareté
    private Color GetColorForRarity(RarityConstruction rarity) {
        switch (rarity) {
            case RarityConstruction.Common:
                return Color.white;
            case RarityConstruction.Rare:
                return Color.blue;
            case RarityConstruction.Epic:
                return new Color(0.5f, 0f, 0.5f); // Violet pour "Epic"
            case RarityConstruction.Legendary:
                return Color.yellow;
            default:
                return Color.gray;
        }
    }

    private void DimOtherConstructions(GameObject activeConstruction) {
        foreach (Transform child in conteneurConstructions) {
            if (child.gameObject != activeConstruction) {
                CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
                if (canvasGroup == null) {
                    canvasGroup = child.gameObject.AddComponent<CanvasGroup>();
                }
                canvasGroup.alpha = 0.02f; // Make less visible
            }
        }

        // Move the active construction to the bottom of the hierarchy
        activeConstruction.transform.SetAsLastSibling();
    }

    private void RestoreConstructionsVisibility() {
        foreach (Transform child in conteneurConstructions) {
            CanvasGroup canvasGroup = child.GetComponent<CanvasGroup>();
            if (canvasGroup != null) {
                canvasGroup.alpha = 1f; // Restore visibility
            }
        }
    }

    public void ChangeOption(ShopOption option) {
        if (currentOption == option) return;
        
        ToggleShop(false);
        UpgradeManager.Instance.DisplayUpgrade(false);
        InventoryUI.Instance.DisplayInventory(false);

        currentOption = option;
        switch (option) {
            case ShopOption.PurchaseConstruction:
                ToggleShop(true);
                break;
            case ShopOption.UpgradeShip:
                UpgradeManager.Instance.DisplayUpgrade(true);
                break;
            case ShopOption.ManageInventory:
                InventoryUI.Instance.DisplayInventory(true);
                break;
        }
    }
}

public enum ShopOption {
    None,
    PurchaseConstruction,
    UpgradeShip,
    ManageInventory
}
