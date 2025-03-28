using UnityEngine;
namespace Spaze {
    public class FollowUV : MonoBehaviour {
        [SerializeField] public float parallax = 2f;

        /// <summary>
        /// Appelé à chaque frame. Met à jour le décalage de texture pour créer un effet de parallaxe.
        /// </summary>
        void Update() {
            MeshRenderer mr = GetComponent<MeshRenderer>();
            Material mat = mr.material;

            Vector2 offset = mat.mainTextureOffset;
            offset.x = transform.position.x / transform.localScale.x / parallax;
            offset.y = transform.position.z / transform.localScale.y / parallax;

            mat.mainTextureOffset = offset;
        }
    }
}