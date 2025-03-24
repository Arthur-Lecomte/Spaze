public class MainModule : Module {
    private void Start() {
        CanActivateNeighbors();
        
        GetComponentInChildren<Turret>().Initialisation(RarityConstruction.Common);
        GetComponentInChildren<Extracteur>().Initialisation(RarityConstruction.Common);
        GetComponentInChildren<Speed>().Initialisation(RarityConstruction.Common);
        
        Construction = GetComponentInChildren<Shield>();
        Construction.Initialisation(RarityConstruction.Common);
    }
}
