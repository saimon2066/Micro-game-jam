using System;
using System.Linq;
using UnityEngine;

namespace Coins
{
    public class CoinManager : MonoBehaviour
    {
        public static CoinManager Instance { get; private set; }
        
        public event Action<int> CoinPickup;

        private GameObject[] _coinObjects;
        private int _coinCount;

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

            _coinObjects = GameObject.FindGameObjectsWithTag("Coin");
        }

        private void Start()
        {
            _coinCount = _coinObjects.Length;
            CoinPickup?.Invoke(_coinCount);
        }

        public void PickupCoin(GameObject coinObject)
        {
            Destroy(coinObject);
            _coinCount--;
            CoinPickup?.Invoke(_coinCount);
        }
    }
}