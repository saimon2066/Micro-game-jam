using System;
using Coins;
using Game;
using TMPro;
using UnityEngine;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinLabel;
        [SerializeField] private TextMeshProUGUI _timerLabel;

        private float _time;
        private bool _isGameFinished;
        
        private void OnEnable()
        {
            CoinManager.Instance.CoinPickup += OnCoinPickup;
            GameManager.Instance.GameFinish += OnGameFinished;
        }

        private void OnDisable()
        {
            CoinManager.Instance.CoinPickup -= OnCoinPickup;
            GameManager.Instance.GameFinish -= OnGameFinished;
        }

        private void Update()
        {
            if (_isGameFinished)
            {
                return;
            }
            
            _time += Time.deltaTime;
            
            float minutes = Mathf.FloorToInt(_time / 60f);
            float seconds = Mathf.RoundToInt(_time % 60);

            string minutesString = minutes.ToString().PadLeft(2, '0');
            string secondsString = seconds.ToString().PadLeft(2, '0');
            
            _timerLabel.text = $"{minutesString}:{secondsString}";
        }

        private void OnCoinPickup(int remaining)
        {
            _coinLabel.text = $"Remaining coins: {remaining}";
        }

        private void OnGameFinished()
        {
            _coinLabel.text = $"You win!";
            _isGameFinished = true;
        }
    }
}