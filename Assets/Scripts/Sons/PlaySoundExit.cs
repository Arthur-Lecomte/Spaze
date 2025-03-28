using UnityEngine;

namespace Spaze {
    public class PlaySoundExit : StateMachineBehaviour {
        [SerializeField] private SoundType sound;
        [SerializeField, Range(0, 1)] private float volume = 1;

        /// <summary>
        /// Méthode appelée lorsque l'état de l'Animator est quitté.
        /// Joue un son spécifié.
        /// </summary>
        /// <param name="animator">L'Animator qui a changé d'état.</param>
        /// <param name="stateInfo">Les informations sur l'état actuel de l'Animator.</param>
        /// <param name="layerIndex">L'index de la couche de l'Animator.</param>
        override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            SoundManager.PlaySound(sound, null, volume);
        }
    }
}