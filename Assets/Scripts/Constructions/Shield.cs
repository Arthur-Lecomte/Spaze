using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : SearchShield {
    [SerializeField] private float maxLife;
    [SerializeField] private float maxLifeWithPercent;
    private static float percent;
    [SerializeField] private float life;
    private float regeneration;
    private float range;

    [SerializeField] private Transform trigger;

    [SerializeField] private RawImage lifeBar;

    /// <summary>
    /// Obtient la vie actuelle du bouclier.
    /// </summary>
    /// <returns>La vie actuelle du bouclier.</returns>
    public float GetLife() {
        return life;
    }

    /// <summary>
    /// Obtient la vie maximale du bouclier avec le pourcentage appliqué.
    /// </summary>
    /// <returns>La vie maximale du bouclier.</returns>
    public float GetMaxLife() {
        return maxLifeWithPercent;
    }

    /// <summary>
    /// Obtient la régénération du bouclier.
    /// </summary>
    /// <returns>La régénération du bouclier.</returns>
    public float GetRegeneration() {
        return regeneration;
    }

    /// <summary>
    /// Obtient la liste des boucliers de recherche à proximité.
    /// </summary>
    /// <returns>Liste des boucliers de recherche à proximité.</returns>
    public List<SearchShield> GetNearbySearchShields() {
        return NearbySearchShields;
    }

    /// <summary>
    /// Active ou désactive le faisceau laser pour tous les boucliers de régénération à proximité.
    /// </summary>
    /// <param name="active">Indique si le faisceau laser doit être activé ou désactivé.</param>
    public void ActiveLaserBeam(bool active) {
        foreach (SearchShield searchShield in NearbySearchShields) {
            RegenerationShield rs = (RegenerationShield)searchShield;
            rs.ActiveLaserBeam(this, active);
        }
    }

    /// <summary>
    /// Initialise le bouclier avec les paramètres de rareté spécifiés.
    /// </summary>
    /// <param name="rarityConstruction">La rareté de la construction.</param>
    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);

        OnPercentChanged(TypeUpgrade.Shield, percent);
        ChangeLife(maxLifeWithPercent);

        trigger.localScale = Vector3.one * (range * 2 / ConstructionTransform.localScale.x);

        TypeToSearch = typeof(RegenerationShield);
    }

    /// <summary>
    /// Définit le parent de cette construction et ajuste sa position et son échelle.
    /// </summary>
    /// <param name="parent">Le parent à définir.</param>
    /// <param name="onModule">Indique si la construction est sur un module.</param>
    public override void SetChildOf(Transform parent, bool onModule = true) {
        base.SetChildOf(parent, onModule);

        newShield?.Invoke();
    }

    /// <summary>
    /// Change la vie du bouclier en ajoutant ou en soustrayant une quantité spécifiée.
    /// </summary>
    /// <param name="quantity">La quantité à ajouter ou à soustraire.</param>
    public void ChangeLife(float quantity) {
        life = Mathf.Clamp(life + quantity, 0, maxLifeWithPercent);
        lifeBar.rectTransform.sizeDelta = new Vector2(life / maxLifeWithPercent * 100, 20);
    }

    /// <summary>
    /// Effectue des actions spécifiques lors de l'amélioration du bouclier.
    /// </summary>
    protected override void PerformUpgrade() {
        OnPercentChanged(TypeUpgrade.Shield, percent);
        life = maxLifeWithPercent; // Régénère entièrement le bouclier en s'améliorant
        ChangeLife(0);
        trigger.localScale = Vector3.one * (range * 2 / ConstructionTransform.localScale.x); // Augmente la portée du bouclier
    }

    /// <summary>
    /// Méthode appelée lorsque le pourcentage change.
    /// </summary>
    /// <param name="typeUpgrade">Le type d'amélioration.</param>
    /// <param name="value">Le pourcentage de changement.</param>
    protected override void OnPercentChanged(TypeUpgrade typeUpgrade, float value) {
        if (value == 0) {
            UpgradeColumn.getValue(typeUpgrade);
            return;
        }

        if (typeUpgrade == TypeUpgrade.Shield) {
            percent = value;
            maxLifeWithPercent = maxLife * percent;
            ChangeLife(0);
        }
    }
}
