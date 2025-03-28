using System;
using UnityEngine;
using UnityEngine.UI;
namespace Spaze {
    public class UpgradeColumn : MonoBehaviour {
        [SerializeField] private TypeUpgrade type;

        [SerializeField] private Image image1;
        [SerializeField] private Image image2;
        [SerializeField] private Image image3;
        [SerializeField] private Image image4;
        [SerializeField] private Image image5;

        private int level;

        public static Action<TypeUpgrade> getValue;

        /// <summary>
        /// Méthode appelée lors de l'initialisation de l'objet. Initialise les composants nécessaires.
        /// </summary>
        private void Awake() {
            level = 2;
            getValue += WantValue;
        }

        /// <summary>
        /// Ajoute un niveau à l'amélioration si possible.
        /// </summary>
        public void Add() {
            if (level < 5) {
                if (UpgradeManager.Instance.CanUpgrade()) {
                    level++;
                    SoundManager.PlaySound(SoundType.CLICK);
                    SendInformation();
                }
            }
        }

        /// <summary>
        /// Retire un niveau à l'amélioration si possible.
        /// </summary>
        public void Remove() {
            if (level > 1) {
                level--;
                UpgradeManager.Instance.AddPoints();
                SoundManager.PlaySound(SoundType.CLICK);
                SendInformation();
            }
        }

        /// <summary>
        /// Méthode appelée pour obtenir la valeur de l'amélioration.
        /// </summary>
        /// <param name="typeUpgrade">Le type d'amélioration.</param>
        private void WantValue(TypeUpgrade typeUpgrade) {
            if (typeUpgrade == type) {
                SendInformation();
            }
        }

        /// <summary>
        /// Envoie les informations de l'amélioration et met à jour les couleurs des images.
        /// </summary>
        private void SendInformation() {
            Construction.onPercentChanged?.Invoke(type, 0.5f + level / 10.0f);

            image5.color = level >= 1 ? Color.green : Color.red;
            image4.color = level >= 2 ? Color.green : Color.red;
            image3.color = level >= 3 ? Color.green : Color.red;
            image2.color = level >= 4 ? Color.green : Color.red;
            image1.color = level >= 5 ? Color.green : Color.red;
        }
    }
}