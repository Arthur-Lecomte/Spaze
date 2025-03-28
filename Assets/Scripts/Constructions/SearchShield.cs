using System;
using System.Collections.Generic;
namespace Spaze {
    public class SearchShield : Construction {
        protected Type TypeToSearch;
        protected List<SearchShield> NearbySearchShields;

        protected static Action newShield;

        /// <summary>
        /// Initialise le bouclier de recherche avec les paramètres de rareté spécifiés.
        /// </summary>
        /// <param name="rarityConstruction">La rareté de la construction.</param>
        public override void Initialisation(RarityConstruction rarityConstruction) {
            base.Initialisation(rarityConstruction);

            NearbySearchShields = new List<SearchShield>();
        }

        /// <summary>
        /// Ajoute un bouclier de recherche à la liste des boucliers à proximité.
        /// </summary>
        /// <param name="searchShield">Le bouclier de recherche à ajouter.</param>
        public void AddSearchShield(SearchShield searchShield) {
            NearbySearchShields.Add(searchShield);
        }

        /// <summary>
        /// Supprime un bouclier de recherche de la liste des boucliers à proximité.
        /// </summary>
        /// <param name="searchShield">Le bouclier de recherche à supprimer.</param>
        public void RemoveSearchShield(SearchShield searchShield) {
            NearbySearchShields.Remove(searchShield);
        }
    }
}