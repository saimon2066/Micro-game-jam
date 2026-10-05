using System;
using Coins;
using UnityEngine;

namespace Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        public event Action GameFinish;
        
        private void OnEnable()
        {
            CoinManager.Instance.CoinPickup += OnCoinPickup;
        }

        private void OnDisable()
        {
            CoinManager.Instance.CoinPickup -= OnCoinPickup;
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnCoinPickup(int coinCount)
        {
            if (coinCount <= 0)
            {
                GameFinish?.Invoke();
            }
        }
    }
}