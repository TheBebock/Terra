using System;
using Terra.EffectsSystem.Abstract.Definitions;

namespace Terra.EffectsSystem.Abstract
{
    /// <summary>
    ///     Represents status effect, that is based on amount of usages
    /// </summary>
    public abstract class NumeralStatusEffect<TNumeralStatusData> : StatusEffect<TNumeralStatusData>
    where TNumeralStatusData : NumeralEffectData
    {
        protected override bool CanBeRemoved => _usagesLeft == 0 && !IsInfinite;

        private int _usagesLeft;
        private bool IsInfinite => _usagesLeft == -1;
        public int UsagesLeft => _usagesLeft;
        public float NormalizedUsage => ((float)UsagesLeft / Data.amountOfUsages) * 100; 
        protected override void OnApply()
        {
            _usagesLeft = Data.amountOfUsages;
            SubscribeToTrigger();
        }

         protected sealed override void OnUpdate()
        {
            //Noop
        }

        private void SubscribeToTrigger()
        {
            Action trigger = GetTriggerAction();
            if (trigger != null)
                trigger += HandleTriggeredUsage;
        }



        private void HandleTriggeredUsage()
        {
            OnUsage();
            if(IsInfinite) return;
            
             _usagesLeft--;
        }
        protected override void OnReset()
        {
            _usagesLeft = Data.amountOfUsages;
        }
        protected abstract ref Action GetTriggerAction();
        
        protected abstract void OnUsage();

        private void UnsubscribeFromTrigger()
        {
            Action trigger = GetTriggerAction();
            if (trigger != null)
                trigger -= HandleTriggeredUsage;
        }
        protected override void OnRemove()
        {
            UnsubscribeFromTrigger();    
        }
    }
}