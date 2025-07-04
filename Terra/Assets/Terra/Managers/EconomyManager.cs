using System;
using Terra.Core.Generics;
using NaughtyAttributes;
using Terra.Utils;
using UnityEngine;
using UnityEngine.Serialization;

namespace Terra.Managers
{
    public class EconomyManager : MonoBehaviourSingleton<EconomyManager>
    {
        
        [FormerlySerializedAs("maxGold")] [Foldout("Config")] [SerializeField] private int _maxGold = 200;
        [FormerlySerializedAs("initialGold")] [Foldout("Config")] [SerializeField] private int _initialGold;
        [Foldout("Debug")] [SerializeField, ReadOnly] private int _currentGold;

        public int CurrentGold => _currentGold;

        public bool CanBuy(int itemPrice) => _currentGold >= itemPrice;
        
        public event Action<int> OnGoldChanged;


        protected override void Awake()
        {
            base.Awake();
            _currentGold = _initialGold;
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
            _currentGold = Mathf.Clamp(_currentGold, 0, _maxGold);
            OnGoldChanged?.Invoke(_currentGold);
        }

        /// <summary>
        /// Sets current gold amount. Can be overloaded to carry over maximum capacity, otherwise it gets clamped to maximum.
        /// </summary>
        public void SetCurrentGoldAmount(int amount, bool overload = false)
        {
            _currentGold = amount;
            if (!overload) _currentGold = Mathf.Clamp(_currentGold, 0, _maxGold);
            else _currentGold = Mathf.Clamp(_currentGold, 0, Constants.MaximumGoldCapacity);
            
        }
        /// <summary>
        /// Sets maximum carrying gold amount, clamping max value to <see cref="Constants.MaximumGoldCapacity"/>
        /// </summary>
        public void SetMaxGoldAmount(int amount)
        {
            _maxGold = Mathf.Clamp(amount, 0, Constants.MaximumGoldCapacity);
        }
        
#if UNITY_EDITOR

        protected override void OnValidate()
        {
            base.OnValidate();
            
            if (_maxGold > Constants.MaximumGoldCapacity)
            {
                _maxGold = Constants.MaximumGoldCapacity;
                Debug.LogWarning($"Maximum carry amount of gold exceeded. Maximum capacity allowed is {Constants.MaximumGoldCapacity}");
            }
        }

#endif
        
    }
}
