using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Spaze {
    public abstract class Construction : MonoBehaviour {
        private static ConstructionStatsManager stats;

        [SerializeField] protected string nom;
        [SerializeField] protected string description;
        [SerializeField] protected RarityConstruction rarity;
        [SerializeField] protected TypeConstruction type;
        private const int NiveauMax = 5;
        private int niveau = 1;
        [SerializeField] protected List<Ressource> coutRessources = new List<Ressource>();
        [SerializeField] protected int probability;
        [SerializeField] protected Sprite image;
        protected Transform ConstructionTransform;
        private GameObject[] pieces;

        public static Action<TypeUpgrade, float> onPercentChanged;

        /// <summary>
        /// Initialise la construction avec les paramètres de rareté spécifiés.
        /// </summary>
        /// <param name="rarityConstruction">La rareté de la construction.</param>
        public virtual void Initialisation(RarityConstruction rarityConstruction) {
            stats ??= new ConstructionStatsManager();
            ConstructionTransform = transform.GetChild(0);
            pieces = new GameObject[5];
            if (transform.childCount >= 2 && transform.GetChild(1).childCount == 5) {
                Transform allPieces = transform.GetChild(1);
                for (int i = 0; i < allPieces.childCount; i++) {
                    pieces[i] = allPieces.GetChild((i + 4) % 5).gameObject;
                }
            }

            rarity = rarityConstruction;
            SetAllVariables();

            foreach (Ressource ressource in coutRessources) {
                ressource.quantite = (int)(ressource.quantite * GetRarityMultiplier());
            }

            onPercentChanged += OnPercentChanged;
        }

        /// <summary>
        /// Obtient la liste des ressources nécessaires pour construire cette construction.
        /// </summary>
        /// <returns>Liste des ressources nécessaires.</returns>
        public List<Ressource> GetCoutRessources() {
            return coutRessources;
        }

        /// <summary>
        /// Obtient le sprite associé à cette construction.
        /// </summary>
        /// <returns>Le sprite de la construction.</returns>
        public Sprite GetSprite() {
            return image;
        }

        /// <summary>
        /// Obtient la rareté de cette construction.
        /// </summary>
        /// <returns>La rareté de la construction.</returns>
        public RarityConstruction GetRarity() {
            return rarity;
        }

        /// <summary>
        /// Obtient la probabilité de cette construction.
        /// </summary>
        /// <returns>La probabilité de la construction.</returns>
        public int GetProbability() {
            return probability;
        }

        /// <summary>
        /// Obtient le nom de cette construction.
        /// </summary>
        /// <returns>Le nom de la construction.</returns>
        public string GetNom() {
            return nom;
        }

        /// <summary>
        /// Obtient le niveau actuel de cette construction.
        /// </summary>
        /// <returns>Le niveau de la construction.</returns>
        public int GetNiveau() {
            return niveau;
        }

        /// <summary>
        /// Définit le parent de cette construction et ajuste sa position et son échelle.
        /// </summary>
        /// <param name="parent">Le parent à définir.</param>
        /// <param name="onModule">Indique si la construction est sur un module.</param>
        public virtual void SetChildOf(Transform parent, bool onModule = true) {
            transform.SetParent(parent);

            if (onModule) {
                transform.localPosition = Vector3.zero;
                transform.localRotation = Quaternion.identity;
                transform.localScale = Vector3.one;

                float taille = 1f + (niveau - 1) * (2f - 1f) / (NiveauMax - 1);
                ConstructionTransform.localPosition = new Vector3(0, -0.5f * (taille - 1f), 0);
                ConstructionTransform.localScale = new Vector3(taille, taille, taille);

                for (int i = 0; i < pieces.Length; i++) {
                    pieces[i].SetActive(i < niveau);
                }
            }
        }

        /// <summary>
        /// Vérifie si cette construction est identique à une autre construction.
        /// </summary>
        /// <param name="c">La construction à comparer.</param>
        /// <returns>True si les constructions sont identiques, sinon False.</returns>
        public bool IsSameConstruction(Construction c) {
            return c.type == type && c.niveau == niveau && c.rarity == rarity;
        }

        /// <summary>
        /// Améliore cette construction d'un niveau.
        /// </summary>
        /// <returns>True si l'amélioration a réussi, sinon False.</returns>
        public bool Upgrade() {
            if (niveau >= NiveauMax) return false;

            niveau++;
            SetChildOf(transform.parent);
            SetAllVariables();
            PerformUpgrade();
            InventoryUI.Instance.ConstructionHaveUpdate(this);
            return true;
        }

        /// <summary>
        /// Effectue des actions spécifiques lors de l'amélioration de la construction.
        /// </summary>
        protected virtual void PerformUpgrade() {
        }

        /// <summary>
        /// Obtient un multiplicateur basé sur la rareté de la construction.
        /// </summary>
        /// <returns>Le multiplicateur de rareté.</returns>
        private float GetRarityMultiplier() {
            switch (rarity) {
                case RarityConstruction.Common: return 1.0f;
                case RarityConstruction.Rare: return 1.25f;
                case RarityConstruction.Epic: return 1.5f;
                case RarityConstruction.Legendary: return 2f;
                default: return 1.0f;
            }
        }

        /// <summary>
        /// Obtient la couleur associée à la rareté de la construction.
        /// </summary>
        /// <returns>La couleur de la rareté.</returns>
        public Color GetRarityColor() {
            switch (rarity) {
                case RarityConstruction.Common: return Color.gray;
                case RarityConstruction.Rare: return Color.blue;
                case RarityConstruction.Epic: return new Color(0.5f, 0f, 0.5f);
                case RarityConstruction.Legendary: return Color.yellow;
                default: return Color.gray;
            }
        }

        /// <summary>
        /// Obtient le texte décrivant la rareté de la construction.
        /// </summary>
        /// <returns>Le texte de la rareté.</returns>
        public string GetRarityText() {
            switch (rarity) {
                case RarityConstruction.Common: return "Commun";
                case RarityConstruction.Rare: return "Rare";
                case RarityConstruction.Epic: return "Épique";
                case RarityConstruction.Legendary: return "Légendaire";
                default: return "Commun";
            }
        }

        /// <summary>
        /// Méthode appelée lorsque le pourcentage change.
        /// </summary>
        /// <param name="typeUpgrade">Le type d'amélioration.</param>
        /// <param name="percent">Le pourcentage de changement.</param>
        protected virtual void OnPercentChanged(TypeUpgrade typeUpgrade, float percent) {
        }

        /// <summary>
        /// Méthode appelée lors de la destruction de l'objet.
        /// </summary>
        private void OnDestroy() {
            onPercentChanged -= OnPercentChanged;
        }

        /// <summary>
        /// Définit toutes les variables de la construction en fonction de ses statistiques.
        /// </summary>
        private void SetAllVariables() {
            Dictionary<string, float> values = stats.GetDico(type, rarity, niveau - 1);
            foreach (KeyValuePair<string, float> kvp in values) {
                FieldInfo field = GetType().GetField(kvp.Key, BindingFlags.NonPublic | BindingFlags.Instance);
                if (field != null) {
                    field.SetValue(this, kvp.Value);
                } else {
                    Debug.LogError("Field " + kvp.Key + " not found in " + GetType().Name);
                }
            }
        }

        /// <summary>
        /// Obtient les statistiques de la construction sous forme de dictionnaire.
        /// </summary>
        /// <returns>Un dictionnaire contenant les statistiques de la construction.</returns>
        public Dictionary<Sprite, string> GetStats() {
            Dictionary<Sprite, string> dico = new Dictionary<Sprite, string> {
                { Resources.Load<Sprite>($"IconVariable/description"), description }
            };
            Dictionary<string, float> values = stats.GetDico(type, rarity, niveau - 1);
            foreach (KeyValuePair<string, float> kvp in values) {
                dico.Add(Resources.Load<Sprite>($"IconVariable/{kvp.Key}"), kvp.Value.ToString("F2"));
            }

            return dico;
        }
    }

    public enum RarityConstruction {
        Common = 0,
        Rare = 1,
        Epic = 2,
        Legendary = 3,
    }

    public enum TypeConstruction {
        Sniper, //Longue porté à cible unique
        Assault, //Dégâts de zone à moyenne portée
        Shotgun, //Puissants dégâts de zone à courte portée
        Laser, //Puissants dégâts en ligne droite (très long rechargement)
        Shield, //Augmente le bouclier du joueur
        RegenerationShield, //Augmente la régénération du bouclier du joueur
        Speed, //Augmente la vitesse de déplacement du joueur
        Slower, //Ralenti les ennemis proches
        Extracteur, //Augmente la quantité de ressources récoltées
        Radar, //Augmente la portée du radar (ennemis et/ou ressources)
    }
}
