using System;
using UnityEngine;

namespace PrototypePattern {
    [Serializable]
    public class BodyComponents {
        public GameObject head;
        public GameObject face;
        public GameObject body;

        [Header("Arms")]
        public GameObject leftArm;
        public GameObject rightArm;

        [Header("Hands")]
        public GameObject leftHand;
        public GameObject rightHand;

        [Header("Legs")]
        public GameObject leftLeg;
        public GameObject rightLeg;
    }
}