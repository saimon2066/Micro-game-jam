using UnityEngine;
using Coins;

namespace Player
{
    public class PlayerCollision : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Coin"))
            {
                CoinManager.Instance.PickupCoin(other.gameObject);
            }
        }
    }
}