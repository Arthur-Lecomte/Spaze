public class MainModule : Module {
    private void Start() {
        CanActivateNeighbors();
        
        GetComponentInChildren<Turret>().Initialisation(RarityConstruction.Common);
        GetComponentInChildren<Shield>().Initialisation(RarityConstruction.Common);
        GetComponentInChildren<Extracteur>().Initialisation(RarityConstruction.Common);
        GetComponentInChildren<Speed>().Initialisation(RarityConstruction.Common);
    }
}
