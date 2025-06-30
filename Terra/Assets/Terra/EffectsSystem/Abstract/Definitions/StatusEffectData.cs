namespace Terra.EffectsSystem.Abstract.Definitions
{
    public abstract class StatusEffectData : EffectData
    {
        public sealed override float GetEffectPower()
        {
            return CalculateEffectPower();
        }
        
        protected abstract float CalculateEffectPower();
    }
}

