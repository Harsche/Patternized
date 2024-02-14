using UnityEngine;

namespace FlyweightPattern{
    public class FlyweightCoin : MonoBehaviour{
        public int GetCoinTypeIndex(){
            return int.Parse(name);
        }

        public void Setup(CoinTypeFlyweight coinType, int index){
            name = $"{index}";
            transform.localScale = Vector3.one * coinType.Scale;
            Material material = GetComponent<MeshRenderer>().material;
            material.color = coinType.Color;
            material.SetFloat(Shader.PropertyToID("_RotationSpeed"), coinType.RotationSpeed);
        }
    }
}