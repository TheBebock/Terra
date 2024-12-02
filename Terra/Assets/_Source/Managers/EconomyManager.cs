using UnityEngine;

namespace _Source.Managers
{
    public class EconomyManager : MonoBehaviour
    {
        private int _currentGold;
        [SerializeField]
        // Ilosc zlota poczatkowa ktora moze miec postac
        public int initialGold;
        void Start()
        {
            _currentGold = initialGold;
        }

        public bool CanBuy(int itemPrice)
        {
            if (_currentGold >= itemPrice)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Buy(int itemPrice)
        {
            if (CanBuy(itemPrice))
            {
                _currentGold -= itemPrice;
            }
        }

        public void Sell(int itemPrice)
        {
            _currentGold += itemPrice;
        }

        public void DecreaseGold(int amount)
        {
            _currentGold -= amount;
            if (_currentGold < 0)
            {
                _currentGold = 0;
            }
        }

        public void AddGold(int amount)
        {
            _currentGold += amount;
        }
    }
}
