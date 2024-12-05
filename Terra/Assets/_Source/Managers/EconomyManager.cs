using Constants;
using UnityEngine;

namespace _Source.Managers
{
    public class EconomyManager : MonoBehaviour
    {
        private int _currentGold;
        public int CurrentGold => _currentGold;
        [SerializeField] private int maxGold = 100000;
        [SerializeField]private int initialGold;
        void Awake()
        {
            _currentGold = initialGold;
        }

        public bool CanBuy(int itemPrice) => _currentGold >= itemPrice;

        public void Buy(int itemPrice)
        {
            if (CanBuy(itemPrice))
            {
                ModifyCurrentGoldAmount(-itemPrice);
            }
        }
        /// <summary>
        /// Modifies currently held gold amount by given value and then clamps it to maximum carrying capacity
        /// </summary>
        public void ModifyCurrentGoldAmount(int amount)
        {
            _currentGold += amount;
            _currentGold = Mathf.Clamp(_currentGold, 0, maxGold);
        }

        /// <summary>
        /// Sets current gold amount. Can be overloaded to carry over maximum capacity, otherwise it gets clamped to maximum.
        /// </summary>
        public void SetCurrentGoldAmount(int amount, bool overload = false)
        {
            _currentGold = amount;
            if (!overload) _currentGold = Mathf.Clamp(_currentGold, 0, maxGold);
            else _currentGold = Mathf.Clamp(_currentGold, 0, ConstantsStaticUtils.MAXIMUM_GOLD_CAPACITY);
            
        }
        /// <summary>
        /// Sets maximum carrying gold amount, clamping max value to <see cref="StaticUtils.MAXIMUM_GOLD_CAPACITY"/>
        /// </summary>
        public void SetMaxGoldAmount(int amount)
        {
            maxGold = Mathf.Clamp(amount, 0, ConstantsStaticUtils.MAXIMUM_GOLD_CAPACITY);
        }
    }
}
