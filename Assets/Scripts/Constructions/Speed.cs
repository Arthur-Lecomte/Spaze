using UnityEngine;
namespace Spaze {
    public class Speed : Construction {
        public static float AllSpeedPower { get; private set; }
        public static float AllMaxSpeed { get; private set; }
        private static float percent;

        [SerializeField] private float speedPower;
        [SerializeField] private float maxSpeed;

        /// <summary>
        /// Initialise la construction avec les param�tres de raret� sp�cifi�s.
        /// </summary>
        /// <param name="rarityConstruction">La raret� de la construction.</param>
        public override void Initialisation(RarityConstruction rarityConstruction) {
            base.Initialisation(rarityConstruction);

            OnPercentChanged(TypeUpgrade.Speed, percent);
        }

        /// <summary>
        /// D�finit le parent de cette construction et ajuste sa position et son �chelle.
        /// </summary>
        /// <param name="parent">Le parent � d�finir.</param>
        /// <param name="onModule">Indique si la construction est sur un module.</param>
        public override void SetChildOf(Transform parent, bool onModule = true) {
            base.SetChildOf(parent, onModule);

            OnPercentChanged(TypeUpgrade.Speed, percent);
        }

        /// <summary>
        /// Effectue des actions sp�cifiques lors de l'am�lioration de la construction.
        /// </summary>
        protected override void PerformUpgrade() {
            OnPercentChanged(TypeUpgrade.Speed, percent);
        }

        /// <summary>
        /// Calcule le bonus de vitesse en fonction des composants de vitesse pr�sents.
        /// </summary>
        private static void CalculSpeedBonus() {
            Speed[] speedComponents = Vaisseau.Instance.GetComponentsInChildren<Speed>();
            AllSpeedPower = 0;
            AllMaxSpeed = 0;
            foreach (Speed speed in speedComponents) {
                AllSpeedPower += speed.speedPower;
                AllMaxSpeed += speed.maxSpeed;
            }

        AllSpeedPower *= percent;
            AllMaxSpeed *= percent;
        }

        /// <summary>
        /// M�thode appel�e lorsque le pourcentage change.
        /// </summary>
        /// <param name="typeUpgrade">Le type d'am�lioration.</param>
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
}