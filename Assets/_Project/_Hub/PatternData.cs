using UnityEngine;

namespace Hub{
    [CreateAssetMenu(fileName = "(PatternData) Pattern", menuName = "Patternized/Data/Pattern Data", order = 0)]
    public class PatternData : ScriptableObject{
        [field: SerializeField] public string PatternName{ get; private set; }
        [field: SerializeField] public Sprite PatternThumbnail{ get; private set; }
        [field: SerializeField, TextArea] public string PatternDescription{ get; private set; }
        [field: SerializeField, TextArea] public string HowToPlay{ get; private set; }
        [field: SerializeField, TextArea] public string PatternUsage{ get; private set; }
        [field: SerializeField] public string SceneName{ get; private set; }
    }
}