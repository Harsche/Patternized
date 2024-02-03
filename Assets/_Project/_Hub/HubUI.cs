using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Hub{
    public class HubUI : MonoBehaviour{
        [SerializeField] private PatternData[] _patternsData;
        [SerializeField] private Transform _patternList;
        [SerializeField] private PatternButton _patternButtonPrefab;
        [SerializeField] private GameObject _chooseGame;
        [SerializeField] private GameObject _gameInfo;

        [Header("Game Info")]
        [SerializeField] private Image _patternSceneImage;
        [SerializeField] private TMP_Text _patternDescription;
        [SerializeField] private TMP_Text _patternExplanation;
        [SerializeField] private Button _playButton;

        private string _descriptionPlaceholderText;
        private string _explanationPlaceholderText;


        private void Awake(){
            foreach (PatternData patternData in _patternsData){
                PatternButton patternButton = Instantiate(_patternButtonPrefab, _patternList);
                patternButton.SetData(patternData);
                patternButton.Button.onClick.AddListener(() => {
                    _chooseGame.SetActive(false);
                    _gameInfo.SetActive(true);
                    _patternSceneImage.sprite = patternData.PatternThumbnail;
                    _patternDescription.text = string.Format(_descriptionPlaceholderText, patternData.PatternName,
                        patternData.PatternDescription);
                    _patternExplanation.text = string.Format(_explanationPlaceholderText, patternData.HowToPlay,
                        patternData.PatternUsage);
                    _playButton.onClick.AddListener(() => SceneManager.LoadScene(patternData.SceneName));
                });
            }

            _descriptionPlaceholderText = _patternDescription.text;
            _explanationPlaceholderText = _patternExplanation.text;
        }
    }
}