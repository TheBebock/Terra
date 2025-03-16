using System;
using Terra.Core.Generics;
using NaughtyAttributes;
using Terra.Constants;
using UnityEngine;

namespace Terra.Managers
{
    public class EconomyManager : MonoBehaviourSingleton<EconomyManager>
    {
        
        [Foldout("Config")] [SerializeField] private int maxGold = 200;
        [Foldout("Config")] [SerializeField] private int initialGold = 0;
        [Foldout("Debug")] [SerializeField, ReadOnly] private int _currentGold;
        public int CurrentGold => _currentGold;

        public bool CanBuy(int itemPrice) => _currentGold >= itemPrice;

        protected override void Awake()
        {
            base.Awake();
            // Set initial gold
            _currentGold = initialGold;
        }

        /// <summary>
        /// Method checks for item price, buying it if there is enough money.
        /// </summary>
        public void TryToBuy(int itemPrice)
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
            else _currentGold = Mathf.Clamp(_currentGold, 0, Utils.MAXIMUM_GOLD_CAPACITY);
            
        }
        /// <summary>
        /// Sets maximum carrying gold amount, clamping max value to <see cref="Utils.MAXIMUM_GOLD_CAPACITY"/>
        /// </summary>
        public void SetMaxGoldAmount(int amount)
        {
            maxGold = Mathf.Clamp(amount, 0, Utils.MAXIMUM_GOLD_CAPACITY);
        }

        private void OnValidate()
        {
            if (maxGold > Utils.MAXIMUM_GOLD_CAPACITY)
            {
                maxGold = Utils.MAXIMUM_GOLD_CAPACITY;
                Debug.LogWarning($"Maximum carry amount of gold exceeded. Maximum capacity allowed is {Utils.MAXIMUM_GOLD_CAPACITY}");
            }
        }
    }
}
