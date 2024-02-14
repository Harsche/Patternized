using System;
using TMPro;
using UnityEngine;

namespace FlyweightPattern{
    public class UI : MonoBehaviour{
        [SerializeField] private TMP_Text _timerText;
        [SerializeField] private TMP_Text _scoreText;

        private string timeText;
        private string scoreText;

        private void Awake(){
            timeText = _timerText.text;
            scoreText = _scoreText.text;
        }

        public void UpdateTimer(int time){
            _timerText.text = string.Format(timeText, time);
        }
        
        public void UpdateScore(int points){
            _scoreText.text = string.Format(scoreText, points);
        }
    }
}