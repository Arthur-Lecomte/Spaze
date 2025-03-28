using System;
using UnityEngine;
namespace Spaze {
    [Serializable]
    public class Ressource {
        public TypeRessource type;
        private Sprite icon;
        public int quantite;

        /// <summary>
        /// Constructeur de la classe Ressource. Initialise une nouvelle instance de Ressource avec le type et la quantité spécifiés.
        /// </summary>
        /// <param name="type">Le type de la ressource.</param>
        /// <param name="quantite">La quantité de la ressource.</param>
        public Ressource(TypeRessource type, int quantite) {
            this.type = type;
            this.quantite = quantite;
        }

        /// <summary>
        /// Obtient le sprite associé à cette ressource.
        /// </summary>
        /// <returns>Le sprite de la ressource.</returns>
        public Sprite GetSprite() {
            if (!icon) {
                icon = Resources.Load<Sprite>($"IconRessource/{type.ToString()}");
            }
            return icon;
        }

        /// <summary>
        /// Obtient le sprite associé au type de ressource spécifié.
        /// </summary>
        /// <param name="type">Le type de la ressource.</param>
        /// <returns>Le sprite de la ressource.</returns>
        public static Sprite GetRessourceSprite(TypeRessource type) {
            return Resources.Load<Sprite>($"IconRessource/{type.ToString()}");
        }
    }

    public enum TypeRessource {
        Cuivre,
        Argent,
        Or,
        Platine,
        NoyauEnergie
    }
}