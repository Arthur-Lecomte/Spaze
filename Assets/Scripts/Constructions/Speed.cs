using UnityEngine;

public class Speed : Construction {
    public static float AllPower { get; private set; }
    public static float AllMaxSpeed { get; private set; }
    private static float percent;

    [SerializeField] private float power;
    [SerializeField] private float maxSpeed;

    /// <summary>
    /// Initialise la construction avec les paramètres de rareté spécifiés.
    /// </summary>
    /// <param name="rarityConstruction">La rareté de la construction.</param>
    public override void Initialisation(RarityConstruction rarityConstruction) {
        base.Initialisation(rarityConstruction);

        OnPercentChanged(TypeUpgrade.Speed, percent);
    }

    /// <summary>
    /// Définit le parent de cette construction et ajuste sa position et son échelle.
    /// </summary>
    /// <param name="parent">Le parent à définir.</param>
    /// <param name="onModule">Indique si la construction est sur un module.</param>
    public override void SetChildOf(Transform parent, bool onModule = true) {
        base.SetChildOf(parent, onModule);

        OnPercentChanged(TypeUpgrade.Speed, percent);
    }

    /// <summary>
    /// Effectue des actions spécifiques lors de l'amélioration de la construction.
    /// </summary>
    protected override void PerformUpgrade() {
        OnPercentChanged(TypeUpgrade.Speed, percent);
    }

    /// <summary>
    /// Calcule le bonus de vitesse en fonction des composants de vitesse présents.
    /// </summary>
    private static void CalculSpeedBonus() {
        Speed[] speedComponents = Vaisseau.Instance.GetComponentsInChildren<Speed>();
        AllPower = 0;
        AllMaxSpeed = 0;
        foreach (Speed speed in speedComponents) {
            AllPower += speed.power;
            AllMaxSpeed += speed.maxSpeed;
        }

        AllPower *= percent;
        AllMaxSpeed *= percent;
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

        if (typeUpgrade == TypeUpgrade.Speed) {
            percent = value;
            CalculSpeedBonus();
        }
    }
}
