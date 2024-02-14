using UnityEngine;

namespace FlyweightPattern{
    public class Coin : MonoBehaviour{
        public float Scale{ get; private set; }
        public float RotationSpeed{ get; private set; }
        public int Points{ get; private set; }
        public Color Color{ get; private set; }

        public void Setup(CoinTypeFlyweight coinType, int index){
            name = $"{index}";
            transform.localScale = Vector3.one * coinType.Scale;
            Material material = GetComponent<MeshRenderer>().material;
            material.color = coinType.Color;
            material.SetFloat(Shader.PropertyToID("_RotationSpeed"), coinType.RotationSpeed);
        }
    }
}