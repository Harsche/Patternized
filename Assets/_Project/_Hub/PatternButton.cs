using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hub{
    public class PatternButton : MonoBehaviour{
        [field: SerializeField] public Button Button{ get; private set; }
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _name;

        public void SetData(PatternData patternData){
            _icon.sprite = patternData.PatternThumbnail;
            _name.text = patternData.PatternName;
        }
    }
}