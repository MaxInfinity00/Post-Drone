using UnityEngine;

namespace DefaultNamespace {
    public class Player : MonoBehaviour {
        public static Player instance;

        private void Awake() {
            instance = this;
        }
    }
}