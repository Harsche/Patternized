using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypePattern {
    public class CharacterManager : MonoBehaviour {
        [SerializeField] private BodyComponents _bodyComponents;
        void Start() {

            // Apenas um teste de load de sprite
            SpriteRenderer spr = _bodyComponents.head.GetComponent<SpriteRenderer>();
            
            string path = FilePaths.GetPathToResource(FilePaths.resources_artist, "Head");
            Sprite loadedSprite = Resources.Load<Sprite>(path);
            if (loadedSprite == null) {
                Debug.LogError($"Sprite não encontrada no caminho: {path}");
            } else {
                spr.sprite = loadedSprite;
            }
        }
    }
}