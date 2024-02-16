using System;
using System.Collections;
using FlyweightPattern.ECS;
using TMPro;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace FlyweightPattern{
    public class UI : MonoBehaviour{
        [SerializeField] private TMP_Text _timerTMP;
        [SerializeField] private TMP_Text _scoreTMP;
        [SerializeField] private Button _startButton;
        [SerializeField] private GameData _gameData;

        private GameSystem _gameSystem;
        private string _timeText;
        private string _scoreText;
        private bool _isGameRunning;

        private void Awake(){
            _timeText = _timerTMP.text;
            _scoreText = _scoreTMP.text;
            
            _timerTMP.text = string.Format(_timeText, _gameData.MinigameTime);
            _scoreTMP.text = string.Format(_scoreText, 0);
        }

        public void StartButton(){
            _isGameRunning = true;
            _gameSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<GameSystem>();
            var playerSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PlayerSystem>();
            
            playerSystem.ResetPlayerPosition();
            _gameSystem.StartGame();

            _startButton.gameObject.SetActive(false);
            StartCoroutine(MinigameTimer());
            StartCoroutine(UpdateScore());
        }

        private IEnumerator MinigameTimer(){
            WaitForSeconds wait = new(1);
            while (_gameSystem.ElapsedTime <= _gameData.MinigameTime){
                int timeLeft = Mathf.CeilToInt(_gameData.MinigameTime - _gameSystem.ElapsedTime);
                _timerTMP.text = string.Format(_timeText, timeLeft);
                yield return wait;
            }
            _startButton.gameObject.SetActive(true);
            _timerTMP.text = string.Format(_timeText, _gameData.MinigameTime);
            _gameSystem.EndGame();
            _isGameRunning = false;
        }
        
        private IEnumerator UpdateScore(){
            WaitForSeconds wait = new(0.1f);
            while (_isGameRunning){
                _scoreTMP.text = string.Format(_scoreText, _gameSystem.TotalScore);
                yield return wait;
            }
        }
    }
}