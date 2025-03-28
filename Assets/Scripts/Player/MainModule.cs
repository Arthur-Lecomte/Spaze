namespace Spaze {
    public class MainModule : Module {

        /// <summary>
        /// Méthode appelée au démarrage. Initialise les constructions et active les voisins.
        /// </summary>
        private void Start() {
            CanActivateNeighbors();

            GetComponentInChildren<Turret>().Initialisation(RarityConstruction.Common);
            GetComponentInChildren<Extracteur>().Initialisation(RarityConstruction.Common);
            GetComponentInChildren<Speed>().Initialisation(RarityConstruction.Common);

            Construction = GetComponentInChildren<Shield>();
            Construction.Initialisation(RarityConstruction.Common);
        }
    }
}